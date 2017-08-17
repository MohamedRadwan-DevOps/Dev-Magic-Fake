// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The customer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;

#endregion

namespace M.Radwan.Domain.Entities
{
    /// <summary>
    /// The customer.
    /// </summary>
    [Serializable]
    public class Customer
    {
        #region Properties

        /// <summary>
        /// Gets or sets Code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets CountryLocation.
        /// </summary>
        public Country CountryLocation { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        // public List<Order> Orders { get; set; }

        /// <summary>
        /// Gets or sets Orders.
        /// </summary>
        public List<Order> Orders { get; set; }

        /// <summary>
        /// Gets or sets Percentage.
        /// </summary>
        public short Percentage { get; set; }

        /// <summary>
        /// Gets or sets date.
        /// </summary>
        public DateTime Date { get; set; }

        #endregion
    }
}