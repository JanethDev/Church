using Church.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class tblCryptsSections
    {
        [Key]
        [Display(Name = "ID")]
        public int CryptSectionID { get; set; }

        public int CryptClassificationID { get; set; }

        public string Code { get; set; }

        public int Levels { get; set; }

        public string StartLetter { get; set; }

        public string EndLetter { get; set; }

        public int Quantity { get; set; }

        [EnumDataType(typeof(CryptsNichos))]
        public CryptsNichos Classification { get; set; }

        public bool LocationInside { get; set; }

        public int Section { get; set; }

        public bool Active { get; set; }

        [NotMapped]
        public string ClassificationName
            => EnumHelper<CryptsNichos>.GetDisplayValue(Classification);

    }
}
