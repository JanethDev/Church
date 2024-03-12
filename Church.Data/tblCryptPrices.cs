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
    public class tblCryptPrices
    {
        [Key]
        [Display(Name = "ID")]
        public int CryptPriceID { get; set; }

        public int CryptClassificationID { get; set; }

        public decimal Price { get; set; }

        public decimal UnitPrice { get; set; }

        public int Level { get; set; }
    }
}
