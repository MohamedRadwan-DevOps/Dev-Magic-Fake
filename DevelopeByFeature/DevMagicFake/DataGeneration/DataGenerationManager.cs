// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGenerationManager.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The data generation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using M.Radwan.DevMagicFake.Configuration;
using M.Radwan.DevMagicFake.FakeRepositories;
using M.Radwan.DevMagicFake.Utilities;

#endregion

namespace M.Radwan.DevMagicFake.DataGeneration
{
    /// <summary>
    /// The data generation.
    /// </summary>
    internal class DataGenerationManager
    {
        #region Constants and Fields

        /// <summary>
        ///   The framework settings.
        /// </summary>
        internal static readonly FrameworkSettings FrameworkSettings = FrameworkSettings.FrameworkSettingsInstance;

        #endregion

        #region Methods

        /// <summary>
        /// The create object, generate it's data and save it to the memoryDb
        /// </summary>
        /// <param name="mainType">
        /// The main type.
        /// </param>
        /// <param name="assemblyToSearch">
        /// The assembly to search.
        /// </param>
        /// <param name="numberOfItemPerCollection">
        /// The number of item per collection. that we want to generate for example if the customer has collection of order how many order we want to generate?
        /// </param>
        /// <param name="saveObject">
        /// This flag will used so save object to MemoryDB or not
        /// </param>
        /// <returns>
        /// The object that created included the generated data.
        /// </returns>
        /// <exception cref="Exception">
        /// This method will throw exception in case of the object has nested type of collection that not supported by the Dev Magic Fake framework
        /// </exception>
        internal static dynamic CreateObjectWithGeneratedData(Type mainType, Assembly assemblyToSearch, int numberOfItemPerCollection, bool saveObject)
        {
            int objectGraphLevel = 0;
            dynamic instance = Activator.CreateInstance(mainType);
            var propertyInfos = mainType.GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                string propertyName = propertyInfo.Name;
                string typeName = propertyInfo.PropertyType.FullName;
                Type declaringType = propertyInfo.DeclaringType;
                Type subType = Type.GetType(typeName);
                if (subType == null)
                {
                    subType = assemblyToSearch.GetType(typeName);
                }

                // get the attributes of this property to see the data annotation attributes 
                object[] propertyAttributes = propertyInfo.GetCustomAttributes(true);

                if ((subType.IsPrimitive || typeName == "System.String" || typeName == "System.Decimal" || typeName == "System.DateTime") && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(instance, GenerateDataBasedOnPrimaryRules(declaringType, subType, propertyName, FrameworkSettings.DataGenerationPrimaryRules, propertyAttributes), null);
                }
                else if (propertyInfo.PropertyType.GetProperty("Id") != null)
                {
                    // I will check if the type has property Id that's mean it is custom type and business class
                    objectGraphLevel++;
                    if (objectGraphLevel == FrameworkSettings.MaximumObjectGraphLevel && saveObject)
                    {
                        RepositoryUtilities.Save(instance, assemblyToSearch);
                        return instance;
                    }

                    dynamic subInstance = CreateObjectWithGeneratedData(propertyInfo.PropertyType, assemblyToSearch, numberOfItemPerCollection, saveObject);
                    propertyInfo.SetValue(instance, subInstance, null);
                    if (saveObject)
                    {
                        RepositoryUtilities.Save(subInstance, assemblyToSearch);
                    }
                }
                else if (propertyInfo.PropertyType.Name == "List`1" || propertyInfo.PropertyType.Name == "IEnumerable`1" || propertyInfo.PropertyType.Name == "IList`1")
                {
                    // that's mean this property is generic collection new section for collection
                    Type itemType = null;
                    if (subType.IsGenericType && (subType.GetGenericTypeDefinition() == typeof(IEnumerable<>) || (subType.GetGenericTypeDefinition() == typeof(List<>)) || (subType.GetGenericTypeDefinition() == typeof(IList<>))))
                    {
                        itemType = subType.GetGenericArguments()[0];
                    }

                    var collectionType = typeof(List<>);
                    Type[] typeArgs = { itemType };
                    var genericCollection = collectionType.MakeGenericType(typeArgs);
                    dynamic collection = Activator.CreateInstance(genericCollection);
                    propertyInfo.SetValue(instance, collection, null);
                    for (int i = 0; i < numberOfItemPerCollection; i++)
                    {
                        // I should send the insider object of the collection not the collection:done
                        dynamic subInstance = CreateObjectWithGeneratedData(itemType, assemblyToSearch, numberOfItemPerCollection, saveObject);
                        collection.Add(subInstance);
                        if (saveObject)
                        {
                            RepositoryUtilities.Save(subInstance, assemblyToSearch);
                        }
                    }
                }
                else if (propertyInfo.PropertyType.BaseType != null)
                {
                    // that's mean this is a Custom collection
                    if (propertyInfo.PropertyType.BaseType.Name == "List`1" || propertyInfo.PropertyType.BaseType.Name == "IEnumerable`1")
                    {
                        throw new Exception("Custom collection not implemented");
                    }
                }
            }

            if (saveObject)
            {
                RepositoryUtilities.Save(instance, assemblyToSearch);
            }

            return instance;
        }

        /// <summary>
        /// The build range items.
        /// </summary>
        /// <param name="initialRangeItems">
        /// The initial range items.
        /// </param>
        /// <param name="dataGenerationType">
        /// The data generation type.
        /// </param>
        /// <returns>
        /// </returns>
        private static List<string> BuildFinalRangeItems(string[] initialRangeItems, Type dataGenerationType)
        {
            /* I am thinking of cashing this range after build, so no need to create them again in case run it another time with the same DataGenerationRule
             * I am think of making another dictionary with the key of the RuleUsesClassProperty and the value will be the list itself and before I generate a list I look at the dictionary 
             * if I found the key I just return the value or the needed range items
             */
            var finalRangeItems = new List<string>();
            GenerationType generationType = DataGenerationUtilities.GetDataGenerationType(initialRangeItems);
            if (generationType == GenerationType.Random)
            {

                finalRangeItems = CreateRandomListFromRandom(initialRangeItems, dataGenerationType);
            }

            if (generationType == GenerationType.Range)
            {
                finalRangeItems = CreateRandomListFromRange(DataGenerationUtilities.GetRangeFrom(initialRangeItems), DataGenerationUtilities.GetRangeTo(initialRangeItems), initialRangeItems, dataGenerationType);
            }

            if (generationType == GenerationType.List)
            {
                finalRangeItems = CreateRandomListFromList(initialRangeItems, dataGenerationType);
            }

            if (generationType == GenerationType.Value)
            {
                finalRangeItems = CreateRandomListFromValue(initialRangeItems, dataGenerationType);
            }

            return finalRangeItems;
        }

        /// <summary>
        /// The create random list from list.
        /// </summary>
        /// <param name="initialRangeItems">
        /// The initial range items.
        /// </param>
        /// <param name="dataGenerationType">
        /// The data generation type.
        /// </param>
        /// <returns>
        /// </returns>
        private static List<string> CreateRandomListFromList(string[] initialRangeItems, Type dataGenerationType)
        {
            var finalRangeItems = new List<string>();
            string nullValue = DataGenerationUtilities.GetNullValueForDataType(dataGenerationType);
            double nullPercentage = DataGenerationUtilities.GetNullPercentage(initialRangeItems);
            double valuePercentage = 1 - nullPercentage;
            List<string> cleanList = DataGenerationUtilities.GetCleanList(initialRangeItems);
            int numberOfRealValuePercentage = cleanList.Count();
            var numberOfNullPercentageWithFraction = (nullPercentage * numberOfRealValuePercentage) / valuePercentage;
            int numberOfNullPercentage = Convert.ToInt32(Math.Ceiling(numberOfNullPercentageWithFraction));

            int n = 0;
            for (int i = 0; i < numberOfRealValuePercentage; i++)
            {
                finalRangeItems.Add(cleanList[n]);
                if (n == numberOfRealValuePercentage)
                {
                    n = 0;
                    continue;
                }

                n++;
            }

            for (int j = 0; j < numberOfNullPercentage; j++)
            {
                finalRangeItems.Add(nullValue);
            }

            finalRangeItems = finalRangeItems.OrderBy(v => FrameworkSettings.CurrentRandom.Next()).ToList(); // random sort the list I can use Guid.NewGuid() but this will give dynamic sorting
            return finalRangeItems;
        }

        /// <summary>
        /// The create random list from random.
        /// </summary>
        /// <param name="initialRangeItems">
        /// The initial range items.
        /// </param>
        /// <param name="dataGenerationType">
        /// The data generation type.
        /// </param>
        /// <returns>
        /// </returns>
        private static List<string> CreateRandomListFromRandom(string[] initialRangeItems, Type dataGenerationType)
        {
            var finalRangeItems = new List<string>();
            string nullValue = DataGenerationUtilities.GetNullValueForDataType(dataGenerationType);
            double nullPercentage = DataGenerationUtilities.GetNullPercentage(initialRangeItems);
            double valuePercentage = 1 - nullPercentage;
            var numberOfRealValuePercentage = valuePercentage * 100F; // I can increate 100F to be 1000F if I want to increate the variation, but take care of memory this why I said I am thinking of cash the list
            var numberOfNullPercentage = nullPercentage * 100F;
            for (int i = 0; i < numberOfRealValuePercentage; i++)
            {
                finalRangeItems.Add(DataGenerationUtilities.GenerateDefalutRandomDataFromType(dataGenerationType.Name).ToString());
            }

            for (int j = 0; j < numberOfNullPercentage; j++)
            {
                finalRangeItems.Add(nullValue);
            }

            finalRangeItems = finalRangeItems.OrderBy(v => FrameworkSettings.CurrentRandom.Next()).ToList(); // random sort the list I can use Guid.NewGuid() but this will give dynamic sorting
            return finalRangeItems;
        }

        /// <summary>
        /// The create random list from range.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <param name="initialRangeItems">
        /// The initial range items.
        /// </param>
        /// <param name="dataGenerationType">
        /// The data generation type.
        /// </param>
        /// <returns>
        /// </returns>
        private static List<string> CreateRandomListFromRange(string from, string to, string[] initialRangeItems, Type dataGenerationType)
        {
            var finalRangeItems = new List<string>();
            string nullValue = DataGenerationUtilities.GetNullValueForDataType(dataGenerationType);
            double nullPercentage = DataGenerationUtilities.GetNullPercentage(initialRangeItems);
            double valuePercentage = 1 - nullPercentage;
            var numberOfRealValuePercentage = valuePercentage * 100F; // I can increate 100F to be 1000F if I want to increate the variation, but take care of memory this why I said I am thinking of cash the list
            var numberOfNullPercentage = nullPercentage * 100F;
            for (int i = 0; i < numberOfRealValuePercentage; i++)
            {
                finalRangeItems.Add(DataGenerationUtilities.GenerateRandomDataFromRange(from, to, dataGenerationType.Name).ToString());
            }

            for (int j = 0; j < numberOfNullPercentage; j++)
            {
                finalRangeItems.Add(nullValue);
            }

            finalRangeItems = finalRangeItems.OrderBy(v => FrameworkSettings.CurrentRandom.Next()).ToList(); // random sort the list I can use Guid.NewGuid() but this will give dynamic sorting
            return finalRangeItems;
        }

        /// <summary>
        /// The create random list from value.
        /// </summary>
        /// <param name="initialRangeItems">
        /// The initial range items.
        /// </param>
        /// <param name="dataGenerationType">
        /// The data generation type.
        /// </param>
        /// <returns>
        /// </returns>
        private static List<string> CreateRandomListFromValue(string[] initialRangeItems, Type dataGenerationType)
        {
            var finalRangeItems = new List<string>();
            string nullValue = DataGenerationUtilities.GetNullValueForDataType(dataGenerationType);
            double nullPercentage = DataGenerationUtilities.GetNullPercentage(initialRangeItems);
            double valuePercentage = 1 - nullPercentage;
            string charRange = DataGenerationUtilities.GetCleanList(initialRangeItems).FirstOrDefault();
            int length = DataGenerationUtilities.GetLength(initialRangeItems);
            var numberOfRealValuePercentage = valuePercentage * 100F; // I can increate 100F to be 1000F if I want to increate the variation, but take care of memory this why I said I am thinking of cash the list
            var numberOfNullPercentage = nullPercentage * 100F;
            for (int i = 0; i < numberOfRealValuePercentage; i++)
            {
                finalRangeItems.Add((string)DataGenerators.RandomValues(charRange, length, FrameworkSettings.CurrentRandom));
            }

            for (int j = 0; j < numberOfNullPercentage; j++)
            {
                finalRangeItems.Add(nullValue);
            }

            finalRangeItems = finalRangeItems.OrderBy(v => FrameworkSettings.CurrentRandom.Next()).ToList(); // random sort the list I can use Guid.NewGuid() but this will give dynamic sorting
            return finalRangeItems;
        }

        /// <summary>
        /// The generate data based on data annotations.
        /// </summary>
        /// <param name="declaringType">
        /// The declaring type.
        /// </param>
        /// <param name="subType">
        /// The sub type.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="propertyAttributes">
        /// The property attributes.
        /// </param>
        /// <returns>
        /// The generate data based on data annotations value.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private static object GenerateDataBasedOnDataAnnotations(Type declaringType, Type subType, string propertyName, object[] propertyAttributes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The generate data based on range.
        /// </summary>
        /// <param name="declaringType">
        /// The declaring type.
        /// </param>
        /// <param name="subType">
        /// The sub type.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="dataGenerationPrimaryRules">
        /// The properties values.
        /// </param>
        /// <param name="propertyAttributes">
        /// The property Attributes.
        /// </param>
        /// <returns>
        /// The generate data based on range value.
        /// </returns>
        private static object GenerateDataBasedOnPrimaryRules(Type declaringType, Type subType, string propertyName, Dictionary<string, string> dataGenerationPrimaryRules, object[] propertyAttributes)
        {
            /* Remember the ordering is very important because we will choose which one apply first, the data annotation call very important because we don't know 
             * if the user put data annotation what that mean??? is that's mean he need to generate using this data annotation or just he puts them as data validation  
             * for his business, so we will not overwrite his configuration with the data annotation unless he explicitly specify that he wants data annotation to overwrite
             * Remember also the final paramter of the method maybe it's not used till now but it's very importatnt becaue this will hold the data anotaion that maybe needed
             * to send to the genration method in case if the user configure and I finid it, and this will be avalible for the caller only since he just read the property
             */
            string key = declaringType.Name + "|" + propertyName;
            string value;
            bool keyHasValue = dataGenerationPrimaryRules.TryGetValue(key, out value);
            if (keyHasValue)
            {
                string[] initialRangeItems = value.Split(new[] { '|' });
                List<string> finalRanageItems = BuildFinalRangeItems(initialRangeItems, subType);
                object genratedValue = DataGenerators.RandomString(finalRanageItems, FrameworkSettings.CurrentRandom);
                var returnedValue = Convert.ChangeType(genratedValue, subType);
                return returnedValue;
            }

            key = propertyName;
            keyHasValue = dataGenerationPrimaryRules.TryGetValue(key, out value);
            if (keyHasValue)
            {
                string[] initialRangeItems = value.Split(new[] { '|' });
                List<string> finalRanageItems = BuildFinalRangeItems(initialRangeItems, subType);
                object genratedValue = DataGenerators.RandomString(finalRanageItems, FrameworkSettings.CurrentRandom);
                var returnedValue = Convert.ChangeType(genratedValue, subType);
                return returnedValue;
            }

            // this is a new method version that depend on the DataGenerationPrimaryRules that populated by Expression tree feature base on data type of the property int32, string, decimal, etc.
            key = subType.Name;
            keyHasValue = dataGenerationPrimaryRules.TryGetValue(key, out value);
            if (keyHasValue)
            {
                string[] initialRangeItems = value.Split(new[] { '|' });
                List<string> finalRanageItems = BuildFinalRangeItems(initialRangeItems, subType);
                object genratedValue = DataGenerators.RandomString(finalRanageItems, FrameworkSettings.CurrentRandom);
                var returnedValue = Convert.ChangeType(genratedValue, subType);
                return returnedValue;
            }

            // before call GenerateDefalutRandomData I maybe should call GenerateDataBasedOnDataAnnotation but it is not finished yet
            // and GenerateDefalutRandomData will be called from GenerateDataBasedOnDataAnnotation 
            return DataGenerationUtilities.GenerateDefalutRandomDataFromType(subType.Name);
        }

        #endregion
    }
}
