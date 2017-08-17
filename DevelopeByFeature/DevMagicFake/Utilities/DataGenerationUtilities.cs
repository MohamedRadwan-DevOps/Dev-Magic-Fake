// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGenerationUtilities.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The data generation utilities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using M.Radwan.DevMagicFake.Attributes;
using M.Radwan.DevMagicFake.DataGeneration;

#endregion

namespace M.Radwan.DevMagicFake.Utilities
{
    /// <summary>
    /// The data generation utilities.
    /// </summary>
    internal class DataGenerationUtilities
    {
        #region Methods

        /// <summary>
        /// The generate default random data.
        /// </summary>
        /// <param name="typeName">
        /// The type name.
        /// </param>
        /// <returns>
        /// The generate default random data value.
        /// </returns>
        internal static object GenerateDefalutRandomDataFromType(string typeName)
        {
            switch (typeName)
            {
                case "Int16":
                    return DataGenerators.RandomShort(1, 999, DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "UInt16":
                    return DataGenerators.RandomShort(1, 999, DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "Int32":
                    return DataGenerators.RandomInt(1, 10000, DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "Int64":
                    return DataGenerators.RandomLong(1, 1000000, DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "String":
                    return DataGenerators.RandomString(12, DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "DateTime":
                    return DataGenerators.RandomDateTime(DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "Decimal":
                    return DataGenerators.RandomDecimal(DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "Boolean":
                    return DataGenerators.RandomBool(DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "SByte":
                    return DataGenerators.RandomInt(-128, 127, DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "Byte":
                    return DataGenerators.RandomInt(0, 255, DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "Double":
                    return DataGenerators.RandomDecimal(DataGenerationManager.FrameworkSettings.CurrentRandom);
                default:
                    return null;
            }
        }

        /// <summary>
        /// The get clean list.
        /// </summary>
        /// <param name="initialRangeItems">
        /// The initial range items.
        /// </param>
        /// <returns>
        /// </returns>
        internal static List<string> GetCleanList(string[] initialRangeItems)
        {
            var list = initialRangeItems.ToList();
            
            var indexItems = list.Select(x => Regex.IsMatch(x, "Null:") || Regex.IsMatch(x, "GenerationType:")).ToList();
            int i = 0;
            foreach (var item in indexItems)
            {
                if (item)
                {
                    list.RemoveAt(i);
                    i--;
                }

                i++;
            }

            if (list.Count > 100)
            {
                // this will remove any items if the user configure the list more than 100 items, this save for memory and performance
                var returnedList = list.Take(100);
                return returnedList.ToList();
            }

            return list;
        }

        /// <summary>
        /// The get data generation option.
        /// </summary>
        /// <param name="initialRangeItems">
        /// The initial range items.
        /// </param>
        /// <returns>
        /// </returns>
        internal static GenerationType GetDataGenerationType(string[] initialRangeItems)
        {
            var list = initialRangeItems.ToList();
            var generationTypeValue = list.Where(x => x.Contains("GenerationType:")).FirstOrDefault();
            string[] optionArray = generationTypeValue.Split(new[] { ':' });
            if (optionArray[1] == "List")
            {
                return GenerationType.List;
            }

            if (optionArray[1] == "Range")
            {
                return GenerationType.Range;
            }

            if (optionArray[1] == "Value")
            {
                return GenerationType.Value;
            }

            return GenerationType.Random;
        }

        /// <summary>
        /// The get length.
        /// </summary>
        /// <param name="initialRangeItems">
        /// The initial range items.
        /// </param>
        /// <returns>
        /// The get length.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        internal static int GetLength(string[] initialRangeItems)
        {
            var list = initialRangeItems.ToList();
            var length = list.Where(x => x.Contains("Length:")).FirstOrDefault();
            string[] nullArray = length.Split(new[] { ':' });
            int returnvalue;
            Int32.TryParse(nullArray[1], out returnvalue);
            if (returnvalue > 100 || returnvalue < 1)
            {
                throw new ArgumentException("Max length must be between 1 and 100");
            }

            return returnvalue;
        }

        /// <summary>
        /// The get null percentage.
        /// </summary>
        /// <param name="initialRangeItems">
        /// The initial range items.
        /// </param>
        /// <returns>
        /// The get null percentage.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        internal static double GetNullPercentage(string[] initialRangeItems)
        {
            var list = initialRangeItems.ToList();
            var nullText = list.Where(x => x.Contains("Null:")).FirstOrDefault();
            string[] nullArray = nullText.Split(new[] { ':' });
            double returnvalue;
            Double.TryParse(nullArray[1], out returnvalue);
            if (returnvalue > 1 || returnvalue < 0)
            {
                throw new ArgumentException("Null Percentage can't be greater than 1.0 and small than 0.0");
            }

            return returnvalue;
        }

        /// <summary>
        /// The get null value for data type.
        /// </summary>
        /// <param name="dataGenerationType">
        /// The data generation type.
        /// </param>
        /// <returns>
        /// The get null value for data type.
        /// </returns>
        internal static string GetNullValueForDataType(Type dataGenerationType)
        {
            switch (dataGenerationType.Name)
            {
                case "Int16":
                    return "0";
                case "UInt16":
                    return "0";
                case "Int32":
                    return "0";
                case "Int64":
                    return "0";
                case "String":
                    return "null";
                case "DateTime":
                    return "1/1/0001 12:00:00 AM";
                case "Decimal":
                    return "0";
                case "Boolean":
                    return "false";
                case "SByte":
                    return "0";
                case "Byte":
                    return "0";
                case "Double":
                    return "0.0";
                default:
                    return null;
            }
        }

        /// <summary>
        /// The get range from.
        /// </summary>
        /// <param name="initialRangeItems">
        /// The initial range items.
        /// </param>
        /// <returns>
        /// The get range from.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        internal static string GetRangeFrom(string[] initialRangeItems)
        {
            var items = GetCleanList(initialRangeItems);
            if (items.Count() != 2)
            {
                throw new InvalidOperationException("Range Should only have from and to");
            }

            return items.FirstOrDefault();
        }

        /// <summary>
        /// The get range to.
        /// </summary>
        /// <param name="initialRangeItems">
        /// The initial range items.
        /// </param>
        /// <returns>
        /// The get range to.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        internal static string GetRangeTo(string[] initialRangeItems)
        {
            var items = GetCleanList(initialRangeItems);
            if (items.Count() != 2)
            {
                throw new InvalidOperationException("Range Should only have from and to");
            }

            return items.LastOrDefault();
        }

        /// <summary>
        /// This method will exclude all types that marked with NotFakeable Attribute
        /// </summary>
        /// <param name="generatedTypes">
        /// The generated types.
        /// </param>
        /// <returns>
        /// list of all types except the types that marked with NotFakeable Attribute
        /// </returns>
        internal static IEnumerable<Type> GetTypesExceptNotFakebale(IEnumerable<Type> generatedTypes)
        {
            List<Type> notFakeableTypes = generatedTypes.Where(type => Attribute.IsDefined(type, typeof(NotFakeable))).ToList();
            return generatedTypes.Except(notFakeableTypes);
        }

        /// <summary>
        /// The generate random data from range.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <param name="typeName">
        /// The type name.
        /// </param>
        /// <returns>
        /// The generate random data from range.
        /// </returns>
        internal static object GenerateRandomDataFromRange(string from, string to, string typeName)
        {
            switch (typeName)
            {
                case "Int16":
                    return DataGenerators.RandomShort(Int16.Parse(from), Int16.Parse(to), DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "UInt16":
                    return DataGenerators.RandomUShort(UInt16.Parse(from), UInt16.Parse(to), DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "Int32":
                    return DataGenerators.RandomInt(Int32.Parse(from), Int32.Parse(to), DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "Int64":
                    return DataGenerators.RandomLong(Int32.Parse(from), Int32.Parse(to), DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "String":
                    return DataGenerators.RandomString(new List<string> { from, to }, DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "DateTime":
                    return DataGenerators.RandomDateTime(DateTime.Parse(from), DateTime.Parse(to), DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "Decimal":
                    return DataGenerators.RandomDecimal(Decimal.Parse(from), Decimal.Parse(to), DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "Boolean":
                    return DataGenerators.RandomBool(DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "SByte":
                    return DataGenerators.RandomSByte(SByte.Parse(from), SByte.Parse(to), DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "Byte":
                    return DataGenerators.RandomByte(Byte.Parse(from), Byte.Parse(to), DataGenerationManager.FrameworkSettings.CurrentRandom);
                case "Double":
                    return DataGenerators.RandomDouble(Double.Parse(from), Double.Parse(to), DataGenerationManager.FrameworkSettings.CurrentRandom);
                default:
                    return null;
            }
        }
        
        #endregion
    }
}
