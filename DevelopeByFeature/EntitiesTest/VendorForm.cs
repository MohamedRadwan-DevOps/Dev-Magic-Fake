using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using M.Radwan.DevMagicFake.Attributes;

namespace M.Radwan.EntitiesTest
{
    
    [Serializable()]
    public class VendorForm
    {
        
        //[ScaffoldColumn(false)]
        public long Id { get; set; }

        [RegularExpression(@"[A-Za-z]([A-Za-z]+ ?){2,50}", ErrorMessage = "Invalid character")]
        [Required(ErrorMessage="it's required")]
        [DisplayName("koko")]
        [StringLength(6, ErrorMessage = "Length error")]
        public string Code { get; set; }

        public DateTime Date { get; set; }
        [StringLength(4,ErrorMessage = "Length error")]
        public string Name { get; set; }

        public int Phone { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
    }
}