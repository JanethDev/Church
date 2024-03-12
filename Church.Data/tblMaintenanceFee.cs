using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class tblMaintenanceFee
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int MaintenanceFeeID { get; set; }
        [Display(Name = "Costo")]
        public int Cost { get; set; }
        [Display(Name = "Activo")]
        public bool IsShared { get; set; }
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
        [NotMapped]
        [Display(Name = "Costo familiar")]
        public int SharedCost { get; set; }
        [NotMapped]
        [Display(Name = "Costo compartido")]

        public int FamiliarCost { get; set; }
    }
}
