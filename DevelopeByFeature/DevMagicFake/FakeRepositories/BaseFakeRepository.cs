// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseFakeRepository.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The abstract fake repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

using M.Radwan.DevMagicFake.Configuration;
using M.Radwan.DevMagicFake.DataGeneration;
using M.Radwan.DevMagicFake.Utilities;

#endregion

namespace M.Radwan.DevMagicFake.FakeRepositories
{
    /// <summary>
    /// The abstract fake repository.
    /// </summary>
    public abstract class BaseFakeRepository
    {
        #region Constants and Fields

        /// <summary>
        /// The framework settings.
        /// </summary>
        protected internal FrameworkSettings frameworkSettings = FrameworkSettings.FrameworkSettingsInstance;

        /// <summary>
        ///   The memory db.
        /// </summary>
        private Dictionary<string, List<dynamic>> memoryDb = MemoryStorage.MemoryDb;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets MemoryDb, this is the place that hold all objects that has been saved or updated
        /// </summary>
        public Dictionary<string, List<dynamic>> MemoryDb
        {
            get
            {
                return this.memoryDb;
            }

            set
            {
                this.memoryDb = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The Add method one of the main feature of the Dev Magic Fake, it doe's many of the magic stuff so it's save a new instance if it's Id is 0 or if it's Id is not exist, it update the instance if it's Id existing in the MemoryDb
        /// </summary>
        /// <param name="entity">
        /// The object that we want to save or update.
        /// </param>
        /// <typeparam name="T">
        /// The business class type that we need to save
        /// </typeparam>
        /// <returns>
        /// return the object itself, if this is a new object DevMagicFake will assign a new Id incremental for each type,  if this is an existing object, it will be  updated only
        /// </returns>
        public T Add<T>(T entity)
        {
            string typeFullName = typeof(T).FullName;
            RepositoryUtilities.SaveObject(this.MemoryDb, entity, typeFullName);
            return entity;
        }

        /// <summary>
        /// The same as the normal Add method but this method take a list for fast saving collection
        /// </summary>
        /// <typeparam name="T">
        /// The type that we need to be saved into the repository
        /// </typeparam>
        /// <param name="list">
        /// The list that we want to save to MemroyDb.
        /// </param>
        /// <returns>
        /// The list of the objects that saved, included it's Id property for each object with the new assigned Id from Dev Magic Fake in case of new object
        /// </returns>
        public IEnumerable<T> Add<T>(List<T> list)
        {
            string typeName = typeof(T).FullName;
            foreach (T obj in list)
            {
                RepositoryUtilities.SaveObject(this.MemoryDb, obj, typeName);
            }

            return list;
        }

        /// <summary>
        /// The clear all configuration.
        /// </summary>
        /// <returns>
        /// </returns>
        public BaseFakeRepository ClearPrimaryRules()
        {
            this.frameworkSettings.DataGenerationPrimaryRules.Clear();
            return this;
        }

        /// <summary>
        /// The configure.
        /// </summary>
        /// <param name="option">
        /// The option.
        /// </param>
        /// <returns>
        /// </returns>
        public BaseFakeRepository Configure(Expression<Action<ConfigurationOption>> option)
        {
            var methodCallExpression = (MethodCallExpression)option.Body;
            var parmAssemblyName = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "SetAssemblyNameThatContainClasses", 0);
            if (parmAssemblyName != null)
            {
                this.frameworkSettings.EntitiesAssembly = (string)parmAssemblyName;
            }

            var parmNamespace = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "SetNamesapceThatContainClass", 0);
            if (parmNamespace != null)
            {
                this.frameworkSettings.EntitiesNamespace = (string)parmNamespace;
            }

            var parmUseFakeable = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "SetUseFakeableAttribute", 0);
            if (parmUseFakeable != null)
            {
                this.frameworkSettings.UseFakeableAttribute = (bool)parmUseFakeable;
            }

            var parmUseNotFakeable = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "SetUseNotFakeableAttribute", 0);
            if (parmUseNotFakeable != null)
            {
                this.frameworkSettings.UseNotFakeableAttribute= (bool)parmUseNotFakeable;
            }

            var parmMaximumObjectGraph = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "SetMaximumObjectGraphLevel", 0);
            if (parmMaximumObjectGraph != null)
            {
                this.frameworkSettings.MaximumObjectGraphLevel= (int)parmMaximumObjectGraph;
            }

            var parmAssemblyPath = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "SetAssemblyPathThatContainClasses", 0);
            if (parmAssemblyPath != null)
            {
                this.frameworkSettings.CurrentExecutionPath= (string)parmAssemblyPath;
            }

            var isMethodDynmicRandomCalled = ExpressionUtilities.IsMethodCalled(methodCallExpression, "SetCurrentRandomToDynamic");
            if (isMethodDynmicRandomCalled)
            {
                this.frameworkSettings.CurrentRandom=this.frameworkSettings.RandomDynamicSeed;
            }

            var isMethodFixedRandomCalled = ExpressionUtilities.IsMethodCalled(methodCallExpression, "SetCurrentRandomToFixed");
            if (isMethodFixedRandomCalled)
            {
                this.frameworkSettings.CurrentRandom=this.frameworkSettings.RandomFixedSeed;
            }
            


            return this;
        }

        /// <summary>
        /// The create method will create instance of the T type and generate it's data based on the data generation configuration
        /// </summary>
        /// <typeparam name="T">
        /// The business class type that we want to create
        /// </typeparam>
        /// <returns>
        /// The generated object populated with generated data
        /// </returns>
        public T Create<T>()
        {
            return DataGenerationManager.CreateObjectWithGeneratedData(typeof(T), this.frameworkSettings.Assembly, 1, false);
        }

        /// <summary>
        /// The create method will create many instances of the T type and generate their data based on the data generation configuration
        /// </summary>
        /// <param name="numberOfObject">
        /// The number of object we want to generate for each type.
        /// </param>
        /// <typeparam name="T">
        /// The business class type that we want to create
        /// </typeparam>
        /// <returns>
        /// The generated list of objects populated with generated data
        /// </returns>
        public IEnumerable<T> Create<T>(int numberOfObject)
        {
            var list = new List<T>();
            for (int i = 0; i < numberOfObject; i++)
            {
                list.Add(DataGenerationManager.CreateObjectWithGeneratedData(typeof(T), this.frameworkSettings.Assembly, numberOfObject, false));
            }

            return list;
        }

        /// <summary>
        /// The generate data for all assembly types, This Method will generate data for all assembly classes with complex reference generation, we can use configuration to control the classes and data that will be generated or work with the default values for more information read the user guide on how to control data generation
        /// </summary>
        /// <param name="numberOfRound">
        /// The number of rounds for the loop that iterate over the existing classes to generate.
        /// </param>
        public void GenerateDataForAllAssemblyTypes(int numberOfRound)
        {
            var dataGenerationServices = new DataGenerationServices();
            dataGenerationServices.GenerateFakingData(numberOfRound);
        }

        /// <summary>
        /// The generate data for created types, This Method will generate data for existing types only, for example if we already save student and professor in DevMagicFake, so this method will generate data for student and professor only even if the assembly has many other classes
        /// </summary>
        /// <param name="numberOfRound">
        /// The number of rounds for the loop that iterate over the existing classes to generate.
        /// </param>
        public void GenerateDataForCreatedTypes(int numberOfRound)
        {
            var dataGenerationServices = new DataGenerationServices();
            dataGenerationServices.GenerateFakingData(numberOfRound, this.memoryDb);
        }

        /// <summary>
        /// The Get method will get an instance based on Expression tree
        /// </summary>
        /// <param name="where">
        /// The where condition that filter the object from the repository as an Expression tree
        /// </param>
        /// <typeparam name="T">
        /// The business class type that we want to get
        /// </typeparam>
        /// <returns>
        /// Instance of type T
        /// </returns>
        public T Get<T>(Expression<Func<T, bool>> where)
        {
            string typeName = typeof(T).FullName;
            var list = new List<T>();
            RepositoryUtilities.GetAllObjects(this.MemoryDb, typeName, list);
            return list.Where(where.Compile()).FirstOrDefault();
        }

        /// <summary>
        /// Get All object of Type T from MemoryDb, even if we create Fake of T, we don't need this method we can direct query the MemoryStorge <see cref="MemoryStorage"/> using LINQ
        /// </summary>
        /// <typeparam name="T">
        /// The type that we want to get all it's objects
        /// </typeparam>
        /// <returns>
        /// Return a list of all object from Type T in the MemoryDb
        /// </returns>
        public IEnumerable<T> GetAll<T>()
        {
            string typeName = typeof(T).FullName;
            var list = new List<T>();
            RepositoryUtilities.GetAllObjects(this.MemoryDb, typeName, list);
            return list;
        }

        /// <summary>
        /// Get an object by code, but remember the object must has a property it's name is code
        /// </summary>
        /// <typeparam name="T">
        /// The business class type that we want to get
        /// </typeparam>
        /// <param name="code">
        /// The code that we will use to search to get the object that has it
        /// </param>
        /// <returns>
        /// The object that match the code parameter
        /// </returns>
        public T GetByCode<T>(string code)
        {
            return RepositoryUtilities.GetObjectByCode<T>(this.MemoryDb, code);
        }

        /// <summary>
        /// Get an object by Id, but remember the object must has a property it's name is Id and of type long
        /// </summary>
        /// <param name="id">
        /// The id that we want to search by.
        /// </param>
        /// <typeparam name="T">
        /// The business class type that we want to get and instance of that match the Id parameter
        /// </typeparam>
        /// <returns>
        /// The object that match the Id and from Type T
        /// </returns>
        public T GetById<T>(long id)
        {
            return RepositoryUtilities.GetObjectById<T>(this.MemoryDb, id);
        }

        /// <summary>
        /// The GetMany method will get many of objects or list of objects based on Expression tree
        /// </summary>
        /// <param name="where">
        /// The where condition that filter the object from the repository as an Expression tree
        /// </param>
        /// <typeparam name="T">
        /// The business class type that we want to get
        /// </typeparam>
        /// <returns>
        /// List of objects from type T
        /// </returns>
        public IEnumerable<T> GetMany<T>(Expression<Func<T, bool>> where)
        {
            string typeName = typeof(T).FullName;
            var list = new List<T>();
            RepositoryUtilities.GetAllObjects(this.MemoryDb, typeName, list);
            return list.Where(where.Compile());
        }

        /// <summary>
        /// The remove method will delete an object from our repository
        /// </summary>
        /// <param name="entity">
        /// The entity that we want to remove
        /// </param>
        /// <typeparam name="T">
        /// The business class type that we want to remove
        /// </typeparam>
        public void Remove<T>(T entity)
        {
            RepositoryUtilities.DeleteObjectById(this.MemoryDb, entity);
        }

        /// <summary>
        /// The remove method will delete an object from our repository
        /// </summary>
        /// <param name="where">
        /// The Expression that will be evaluated, the method will delete any object evaluate true for this expression
        /// </param>
        /// <typeparam name="T">
        /// The business class type that we want to remove from
        /// </typeparam>
        public void Remove<T>(Expression<Func<T, bool>> where)
        {
            string typeName = typeof(T).FullName;
            var AllItems = new List<T>();
            RepositoryUtilities.GetAllObjects(this.MemoryDb, typeName, AllItems);
            var DesiredList = AllItems.Where(where.Compile());
            foreach (var item in DesiredList)
            {
                this.Remove(item);
            }

        }

        /// <summary>
        /// The reset to default configuration.
        /// </summary>
        /// <returns>
        /// </returns>
        public BaseFakeRepository ResetToDefaultConfiguration()
        {
            this.frameworkSettings.SetDefaultSettings();
            return this;
        }

        public BaseFakeRepository RuleUsesClassProperty<T, M>(Expression<Func<T, M>> predicate, Expression<Func<DataGenerationOption, List<M>>> option)
        {
            // this rule by class name and property
            var body = (MemberExpression)predicate.Body;
            var className = body.Expression.Type.Name;
            var propertyName = body.Member.Name;

            var methodCallExpression = (MethodCallExpression)option.Body;
            var paramOfNullPercentage = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "NullPercentage", 1);

            var parmOfGeneratFromList = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GeneratFromList", 0);
            if (parmOfGeneratFromList != null)
            {
                this.frameworkSettings.AddRule(className + "|" + propertyName, StringUtilities.ConcatenateValuesWithSeparator(parmOfGeneratFromList, paramOfNullPercentage, "List", null));

            }

            var parmOfGeneratFromRangeFrom = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GenerateFromRange", 0);
            var parmOfGeneratFromRangeTo = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GenerateFromRange", 1);

            if (parmOfGeneratFromRangeFrom != null && parmOfGeneratFromRangeTo != null)
            {
                this.frameworkSettings.AddRule(className + "|" + propertyName, StringUtilities.ConcatenateValuesWithSeparator(new List<object> { parmOfGeneratFromRangeFrom, parmOfGeneratFromRangeTo }, paramOfNullPercentage, "Range", null));
            }

            var parmOfGeneratFromValueValue = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GenerateFromValue", 0);
            var parmOfGeneratFromValueLength = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GenerateFromValue", 1);

            if (parmOfGeneratFromValueValue != null && parmOfGeneratFromValueLength != null)
            {
                this.frameworkSettings.AddRule(className + "|" + propertyName, StringUtilities.ConcatenateValuesWithSeparator(new List<object> { parmOfGeneratFromValueValue }, paramOfNullPercentage, "Value", parmOfGeneratFromValueLength));
            }

            var isMethodCalled = ExpressionUtilities.IsMethodCalled(methodCallExpression, "GenerateFromRandom");
            if (isMethodCalled)
            {
                this.frameworkSettings.AddRule(className + "|" + propertyName, StringUtilities.ConcatenateValuesWithSeparator(null, paramOfNullPercentage, "Random", null));
            }

            return this;
        }

        /// <summary>
        /// The rule uses data type.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <param name="option">
        /// The option.
        /// </param>
        /// <typeparam name="M">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public BaseFakeRepository RuleUsesDataType<M>(Expression<Func<DataGenerationType, M>> predicate, Expression<Func<DataGenerationOption, List<M>>> option)
        {
            // this rule by class name and property
            var dataTypeName = predicate.Body.Type.Name;

            var methodCallExpression = (MethodCallExpression)option.Body;
            var paramOfNullPercentage = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "NullPercentage", 1);

            var parmOfGeneratFromList = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GeneratFromList", 0);
            if (parmOfGeneratFromList != null)
            {
                this.frameworkSettings.AddRule(dataTypeName, StringUtilities.ConcatenateValuesWithSeparator(parmOfGeneratFromList, paramOfNullPercentage, "List", null));

            }

            var parmOfGeneratFromRangeFrom = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GenerateFromRange", 0);
            var parmOfGeneratFromRangeTo = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GenerateFromRange", 1);

            if (parmOfGeneratFromRangeFrom != null && parmOfGeneratFromRangeTo != null)
            {
                this.frameworkSettings.AddRule(dataTypeName, StringUtilities.ConcatenateValuesWithSeparator(new List<object> { parmOfGeneratFromRangeFrom, parmOfGeneratFromRangeTo }, paramOfNullPercentage, "Range", null));

            }

            var parmOfGeneratFromValueValue = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GenerateFromValue", 0);
            var parmOfGeneratFromValueLength = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GenerateFromValue", 1);

            if (parmOfGeneratFromValueValue != null && parmOfGeneratFromValueLength != null)
            {
                this.frameworkSettings.AddRule(dataTypeName, StringUtilities.ConcatenateValuesWithSeparator(new List<object> { parmOfGeneratFromValueValue }, paramOfNullPercentage, "Value", parmOfGeneratFromValueLength));
            }

            var isMethodCalled = ExpressionUtilities.IsMethodCalled(methodCallExpression, "GenerateFromRandom");
            if (isMethodCalled)
            {
                this.frameworkSettings.AddRule(dataTypeName, StringUtilities.ConcatenateValuesWithSeparator(null, paramOfNullPercentage, "Random", null));
            }

            return this;
        }

        /// <summary>
        /// The rule uses property only.
        /// </summary>
        /// <param name="propertyName">
        /// The property Name.
        /// </param>
        /// <param name="option">
        /// The option.
        /// </param>
        /// <typeparam name="TM">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public BaseFakeRepository RuleUsesPropertyOnly<TM>(Expression<Func<string, string>> propertyName, Expression<Func<DataGenerationOption, TM>> option)
        {
            string property = ExpressionUtilities.GetValueFromExpression(propertyName.Body).ToString();
            var methodCallExpression = (MethodCallExpression)option.Body;
            var paramOfNullPercentage = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "NullPercentage", 1);

            var parmOfGeneratFromList = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GeneratFromList", 0);
            if (parmOfGeneratFromList != null)
            {
                this.frameworkSettings.AddRule(property, StringUtilities.ConcatenateValuesWithSeparator(parmOfGeneratFromList, paramOfNullPercentage, "List", null));

            }

            var parmOfGeneratFromRangeFrom = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GenerateFromRange", 0);
            var parmOfGeneratFromRangeTo = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GenerateFromRange", 1);

            if (parmOfGeneratFromRangeFrom != null && parmOfGeneratFromRangeTo != null)
            {
                this.frameworkSettings.AddRule(property, StringUtilities.ConcatenateValuesWithSeparator(new List<object> { parmOfGeneratFromRangeFrom, parmOfGeneratFromRangeTo }, paramOfNullPercentage, "Range", null));

            }

            var parmOfGeneratFromValueValue = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GenerateFromValue", 0);
            var parmOfGeneratFromValueLength = ExpressionUtilities.GetParamterFromMethod(methodCallExpression, "GenerateFromValue", 1);

            if (parmOfGeneratFromValueValue != null && parmOfGeneratFromValueLength != null)
            {
                this.frameworkSettings.AddRule(property, StringUtilities.ConcatenateValuesWithSeparator(new List<object> { parmOfGeneratFromValueValue }, paramOfNullPercentage, "Value", parmOfGeneratFromValueLength));
            }

            var isMethodCalled = ExpressionUtilities.IsMethodCalled(methodCallExpression, "GenerateFromRandom");
            if (isMethodCalled)
            {
                this.frameworkSettings.AddRule(property, StringUtilities.ConcatenateValuesWithSeparator(null, paramOfNullPercentage, "Random", null));
            }

            return this;

        }

        /// <summary>
        /// Just hide from intellisense  
        /// </summary>
        /// <param name="obj">
        /// The object parameters.
        /// </param>
        /// <returns>
        /// The same object 
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Just hide from intellisense 
        /// </summary>
        /// <returns>
        /// The same has code
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Just hide from intellisense 
        /// </summary>
        /// <returns>
        /// The same ToString()
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }
        
        #endregion
    }
}