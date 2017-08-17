using System;

namespace M.Radwan.EntitiesTest
{
    /// <summary>
    /// The service form.
    /// </summary>
    [Serializable]
    public class ServiceForm
    {
        #region Properties
        
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///   Gets or sets The Service phone.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Gets or sets The Service Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///   Gets or sets The Service FixedCommission.
        /// </summary>
        public int FixedCommission { get; set; }

        /// <summary>
        ///   Gets or sets The Service name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets Service contact person mobile.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets ProductTypeDropList.
        /// </summary>
        public ProductTypeForm ProductTypeForm { get; set; }

        /// <summary>
        /// Gets or sets VendorDropList.
        /// </summary>
        public VendorForm VendorForm { get; set; }

        #endregion
    }
    
}