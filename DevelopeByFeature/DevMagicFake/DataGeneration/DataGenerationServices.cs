// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGenerationServices.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The data generation services.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;

using M.Radwan.DevMagicFake.Attributes;
using M.Radwan.DevMagicFake.Configuration;
using M.Radwan.DevMagicFake.Utilities;

#endregion

namespace M.Radwan.DevMagicFake.DataGeneration
{
    /// <summary>
    /// The data generation services.
    /// </summary>
    public class DataGenerationServices
    {
        #region Constants and Fields

        /// <summary>
        /// The framework settings.
        /// </summary>
        internal static readonly FrameworkSettings FrameworkSettings = FrameworkSettings.FrameworkSettingsInstance;

        #endregion

        #region Public Methods

        /// <summary>
        /// This Method will generate data for all assembly classes with complex reference generation, we can use configuration to control the classes and data that will be generated or work with the default values
        /// </summary>
        /// <param name="numberOfRounds">
        /// The number of rounds for the loop that iterate over the existing classes to generate.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        /// <exception cref="Exception">
        /// </exception>
        public void GenerateFakingData(int numberOfRounds)
        {
            // TODO: check this method and add all collection possible because this method not suppose to generate data for collection class itself
            // if the singleTypes contain collection that's mean the method will create collection and start set it's property which are read only like count
            var types = FrameworkSettings.Assembly.GetTypes();

            // get single Types
            var generatedTypes = types.Where(t => (t.BaseType.Name != "List`1") && (t.BaseType.Name != "IEnumerable`1") && (t.BaseType.Name != "IQueryable`1`1")).ToList();
            if (!string.IsNullOrEmpty(FrameworkSettings.EntitiesNamespace))
            {
                generatedTypes = generatedTypes.Where(t => t.Namespace == FrameworkSettings.EntitiesNamespace).ToList();
            }

            if (FrameworkSettings.UseFakeableAttribute)
            {
                generatedTypes = generatedTypes.Where(type => Attribute.IsDefined(type, typeof(Fakeable))).ToList();
            }

            if (FrameworkSettings.UseNotFakeableAttribute)
            {
                generatedTypes = DataGenerationUtilities.GetTypesExceptNotFakebale(generatedTypes).ToList();

            }

            for (int i = 0; i < numberOfRounds; i++)
            {
                foreach (Type type in generatedTypes)
                {
                    try
                    {
                        DataGenerationManager.CreateObjectWithGeneratedData(type, FrameworkSettings.Assembly, numberOfRounds, true);
                    }
                    catch (MissingMethodException e)
                    {
                        throw new Exception(string.Format("The Type {0} not supported for data generation, you may need to add parameter less constructor ", type.Name), e);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("The Type {0} not supported for data generation, you need to exclude from the data generation target or contact M.Radwan, see the inner exception for more detials", type.Name), e);
                    }
                }
            }
        }

        /// <summary>
        /// This Method will generate data for existing types only for example if we already save student and professor in DevMagicFake, so this method will generate data for student and professor only even if the assembly has many other classes
        /// </summary>
        /// <param name="numberOfRounds">
        /// The number of rounds for the loop that iterate over the existing classes to generate.
        /// </param>
        /// <param name="memoryDb">
        /// The memory db, the variable that will hold all saved classes, it will be MemoryStorage.MemoryDb
        /// </param>
        public void GenerateFakingData(int numberOfRounds, Dictionary<string, List<dynamic>> memoryDb)
        {
            for (int i = 0; i < numberOfRounds; i++)
            {
                foreach (KeyValuePair<string, List<dynamic>> pair in memoryDb)
                {
                    Type type = FrameworkSettings.Assembly.GetType(pair.Key);
                    DataGenerationManager.CreateObjectWithGeneratedData(type, FrameworkSettings.Assembly, numberOfRounds, true);
                }
            }
        }

        #endregion
    }
}
