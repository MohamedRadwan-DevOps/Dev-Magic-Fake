// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DotNetExtensions.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The dot net extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace M.Radwan.DevMagicFake.Extensions
{
    /// <summary>
    /// The dot net extensions.
    /// </summary>
    public static class DotNetExtensions
    {
        // if I change this method I have to revise the code because the expression tree translation depend on using this method by name
        #region Public Methods

        /// <summary>
        /// The null percentage.
        /// </summary>
        /// <param name="param">
        /// The param.
        /// </param>
        /// <param name="percentage">
        /// The percentage.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public static List<T> NullPercentage<T>(this IEnumerable<T> param, double percentage)
        {
            object box = percentage;
            List<T> list = param.ToList();
            list.Add((T)box);
            return list;
        }

        #endregion
    }
}
