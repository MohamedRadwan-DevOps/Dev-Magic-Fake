// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductTypeForm.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The product type form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;


#endregion

namespace M.Radwan.Domain.Entities
{
    /// <summary>
    /// The product type form.
    /// </summary>
    [Serializable]
    public class ProductTypeForm
    {
        #region Properties

        /// <summary>
        /// Gets or sets Code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Gets or sets Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///   Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets VendorForm.
        /// </summary>
        public VendorForm VendorForm { get; set; }

        #endregion
    }
}