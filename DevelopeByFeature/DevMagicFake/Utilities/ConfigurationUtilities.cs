// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationUtilities.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The configuration utilities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Configuration;

#endregion

namespace M.Radwan.DevMagicFake.Utilities
{
    /// <summary>
    /// The configuration utilities.
    /// </summary>
    internal class ConfigurationUtilities
    {
        #region Methods

        /// <summary>
        /// The get assembly name from configuration.
        /// </summary>
        /// <returns>
        /// The  assembly name from return from configuration.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        internal static string GetAssemblyNameFromConfig()
        {
            return ConfigurationManager.AppSettings["EntitiesAssembly"];
        }

        /// <summary>
        /// Get class property values from configuration.
        /// </summary>
        /// <returns>
        /// The collection of all Class_property_Values that set in the Configuration file that control the data generation by using class name and property name
        /// </returns>
        internal static Dictionary<string, string> GetDataGenerationPrimaryRulesFromConfig()
        {
            Dictionary<string, string> dataGenerationPrimaryRules = new Dictionary<string, string>();
            string classPropertyValues = ConfigurationManager.AppSettings["Class_Property_Values"];
            if (classPropertyValues != null)
            {
                var arrayOfclassPropertyValues = classPropertyValues.Split(new[] { '$' });
                for (int i = 0; i < arrayOfclassPropertyValues.Length; i++)
                {
                    arrayOfclassPropertyValues[i] = arrayOfclassPropertyValues[i].Trim();
                    var classPropertyandItsValue = arrayOfclassPropertyValues[i].Split(new[] { '-' });
                    dataGenerationPrimaryRules.Add(classPropertyandItsValue[0], classPropertyandItsValue[1]);
                }
            }

            return dataGenerationPrimaryRules;
        }

        /// <summary>
        /// Get class property values from configuration.
        /// </summary>
        /// <returns>
        /// The collection of all Data Type Values that set in the Configuration file that control the data generation by using data generation for each .Net Type
        /// </returns>
        internal static Dictionary<string, string> GetDataGenerationSecondaryRulesFromConfig()
        {
            var dataGenerationRulesFromConfig = new Dictionary<string, string>();
            string dataTypeValue = ConfigurationManager.AppSettings["Type"];
            if (dataTypeValue != null)
            {
                var arrayOfDataTypeValue = dataTypeValue.Split(new[] { '$' });
                for (int i = 0; i < arrayOfDataTypeValue.Length; i++)
                {
                    arrayOfDataTypeValue[i] = arrayOfDataTypeValue[i].Trim();
                    var dataTypeandItsValue = arrayOfDataTypeValue[i].Split(new[] { '-' });
                    dataGenerationRulesFromConfig.Add(dataTypeandItsValue[0], dataTypeandItsValue[1]);
                }
            }

            return dataGenerationRulesFromConfig;
        }

        /// <summary>
        /// The get entities namespace from configuration.
        /// </summary>
        /// <returns>
        /// The entities namespace returned from configuration.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        internal static string GetEntitiesNamespaceFromConfig()
        {
            return ConfigurationManager.AppSettings["EntitiesNamespace"];
        }

        /// <summary>
        /// The get maximum object graph from configuration.
        /// </summary>
        /// <returns>
        /// The maximum object graph value returned from configuration.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        internal static int GetMaximumObjectGraphFromConfig()
        {
            string returnValue = ConfigurationManager.AppSettings["MaximumObjectGraphLevel"];
            if (returnValue == null)
            {
                return 0;
            }

            return Int32.Parse(returnValue);
        }

        /// <summary>
        /// The get use fakeable from configuration.
        /// </summary>
        /// <returns>
        /// The return value from configuration.
        /// </returns>
        internal static bool GetUseFakeableFromConfig()
        {
            string returnValue = ConfigurationManager.AppSettings["UseFakeableAttribute"];
            if (returnValue == null)
            {
                return false;
            }

            return Boolean.Parse(returnValue);
        }

        /// <summary>
        /// The get use not fakeable from configuration.
        /// </summary>
        /// <returns>
        /// The return value from configuration
        /// </returns>
        internal static bool GetUseNotFakeableFromConfig()
        {
            string returnValue = ConfigurationManager.AppSettings["UseNotFakeableAttribute"];
            if (returnValue == null)
            {
                return false;
            }

            return Boolean.Parse(returnValue);
        }

        #endregion
    }
}
