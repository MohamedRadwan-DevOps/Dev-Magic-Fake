// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGenerationOption.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The data generation option.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace M.Radwan.DevMagicFake.DataGeneration
{
    /// <summary>
    /// The data generation option.
    /// </summary>
    public class DataGenerationOption
    {
        // if I change this method I have to revise the code because the expression tree translation depend on using this method by name
        #region Public Methods

        /// <summary>
        /// The generat from list.
        /// </summary>
        /// <param name="range">
        /// The range.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public List<T> GeneratFromList<T>(List<T> range)
        {
            return range;
        }

        /// <summary>
        /// The generat from random.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public List<TM> GenerateFromRandom<TM>()
        {
            return new List<TM>();
        }

        public List<TM> GenerateFromRandom<TM>(Expression<Func<DataGenerationType, TM>> type)
        {
            return new List<TM>();
        }

        /// <summary>
        /// The generate from range.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public List<T> GenerateFromRange<T>(T from, T to)
        {
            if (typeof(T) == typeof(string) || typeof(T) == typeof(bool) || typeof(T) == typeof(object))
            {
                throw new ArgumentException(typeof(T).FullName + " not supported in GenerateFromRange use GeneratFromList instead");
            }

            return new List<T> { from, to };
        }

        /// <summary>
        /// The generate from value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="length">
        /// The length.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public List<T> GenerateFromValue<T>(T value, int length)
        {
            /* this exception will never been thrown because the method never called until now because I just use it in expression tree so if I want to throw the exception I must
             * throw the exception in the expression tree reading method not here, of course if I compile the method it will be thrown 
            */
            if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(object) || typeof(T) == typeof(double) || typeof(T) == typeof(decimal))
            {
                throw new ArgumentException(typeof(T).FullName + " not supported in GenerateFromValue  use GeneratFromList instead");
            }

            return null;
        }

        #endregion
    }
}
