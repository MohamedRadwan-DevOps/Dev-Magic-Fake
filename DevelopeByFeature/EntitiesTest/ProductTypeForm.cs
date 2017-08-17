using System;

namespace M.Radwan.EntitiesTest
{
    [Serializable]
    public class ProductTypeForm
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public long Id { get; set; }
        
        public string Code { get; set; }
        
        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        public VendorForm VendorForm { get; set; }
    }
}