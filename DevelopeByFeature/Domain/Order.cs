// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Order.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The order.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;

#endregion

namespace M.Radwan.Domain.Entities
{
    /// <summary>
    /// The order.
    /// </summary>
    [Serializable]
    public class Order
    {
        #region Properties

        /// <summary>
        /// Gets or sets Code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets OrderDate.
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets OrderName.
        /// </summary>
        public string OrderName { get; set; }

        #endregion
    }
}
