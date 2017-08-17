// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringUtilities.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The string utilities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace M.Radwan.DevMagicFake.Utilities
{
    /// <summary>
    /// The string utilities.
    /// </summary>
    internal class StringUtilities
    {
        #region Public Methods

        /// <summary>
        /// The concatenate values with separator.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <param name="nullPercentage">
        /// The null percentage.
        /// </param>
        /// <param name="generationType">
        /// The generation type.
        /// </param>
        /// <param name="length">
        /// The length.
        /// </param>
        /// <returns>
        /// The concatenate values with separator.
        /// </returns>
        public static string ConcatenateValuesWithSeparator(object values, object nullPercentage, string generationType, object length)
        {
            List<string> list;
            string groupedValues = null;

            if (values != null)
            {
                list = GetListOfStrings(values);

                for (int i = 0; i < list.Count(); i++)
                {
                    if (i == (list.Count() - 1))
                    {
                        groupedValues += list[i];
                    }
                    else
                    {
                        groupedValues += list[i] + "|";
                    }
                }
            }

            if (values != null)
            {
                groupedValues += "|";
            }

            if (nullPercentage != null)
            {
                groupedValues += "Null:" + nullPercentage;
            }
            else
            {
                groupedValues += "Null:0";
            }

            groupedValues += "|GenerationType:" + generationType;
            if (length != null)
            {
                groupedValues += "|Length:" + length;
            }

            return groupedValues.Trim();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get list of strings.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        private static List<string> GetListOfStrings(object values)
        {
            if (values == null)
            {
                return null;
            }

            List<string> returnList;

            var stringList = values as List<string>;
            if (stringList != null)
            {
                return stringList;
            }

            var intList = values as List<int>;
            if (intList != null)
            {
                returnList = intList.ConvertAll(i => i.ToString());
                return returnList;
            }

            var uintList = values as List<uint>;
            if (uintList != null)
            {
                returnList = uintList.ConvertAll(i => i.ToString());
                return returnList;
            }

            var longList = values as List<long>;
            if (longList != null)
            {
                returnList = longList.ConvertAll(i => i.ToString());
                return returnList;
            }

            var ulongList = values as List<ulong>;
            if (ulongList != null)
            {
                returnList = ulongList.ConvertAll(i => i.ToString());
                return returnList;
            }

            var doubleList = values as List<double>;
            if (doubleList != null)
            {
                returnList = doubleList.ConvertAll(i => i.ToString());
                return returnList;
            }

            var floatList = values as List<float>;
            if (floatList != null)
            {
                returnList = floatList.ConvertAll(i => i.ToString());
                return returnList;
            }

            var byteList = values as List<byte>;
            if (byteList != null)
            {
                returnList = byteList.ConvertAll(i => i.ToString());
                return returnList;
            }

            var sbyteList = values as List<sbyte>;
            if (sbyteList != null)
            {
                returnList = sbyteList.ConvertAll(i => i.ToString());
                return returnList;
            }

            var shortList = values as List<short>;
            if (shortList != null)
            {
                returnList = shortList.ConvertAll(i => i.ToString());
                return returnList;
            }

            var ushortList = values as List<ushort>;
            if (ushortList != null)
            {
                returnList = ushortList.ConvertAll(i => i.ToString());
                return returnList;
            }

            var decimalList = values as List<decimal>;
            if (decimalList != null)
            {
                returnList = decimalList.ConvertAll(i => i.ToString());
                return returnList;
            }

            var datelList = values as List<DateTime>;
            if (datelList != null)
            {
                returnList = datelList.ConvertAll(i => i.ToString());
                return returnList;
            }

            var objectList = values as List<object>;
            if (objectList != null)
            {
                returnList = objectList.ConvertAll(i => i.ToString());
                return returnList;
            }

            var chartList = values as List<char>;
            if (chartList != null)
            {
                returnList = chartList.ConvertAll(i => i.ToString());
                return returnList;
            }

            throw new ArgumentException("DevMagicFake didn't support list of this type");


        }

        #endregion
    }
}
