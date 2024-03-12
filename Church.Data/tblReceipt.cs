using Church.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Church.Data
{
    public class tblReceipts
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ReceiptID { get; set; }
        public int PurchasesRequestID { get; set; }
        [Display(Name = "Activo")]
        public bool Active { get; set; }
        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Creado por")]
        public int CreatedBy { get; set; }
        [Display(Name = "Fecha de modificación")]
        public DateTime UpdatedDate { get; set; }
        [Display(Name = "Actualizado por")]
        public int UpdatedBy { get; set; }
        [Display(Name = "Fecha de eliminación")]
        public DateTime? DeletedDate { get; set; }
        [Display(Name = "Eliminado por")]
        public int? DeletedBy { get; set; }
        [NotMapped]
        public string CreatedByName { get; set; }
        [NotMapped]
        public string UpdatedByName { get; set; }
    }
}
