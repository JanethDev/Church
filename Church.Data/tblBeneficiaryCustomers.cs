using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class tblBeneficiaryCustomers
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int BeneficiaryCustomerID { get; set; }
        public int PurchasesRequestID { get; set; }
        [Display(Name = "Cliente")]
        public int CustomerID { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Apellidos")]
        public string Surnames { get; set; }
        [Display(Name = "Teléfono")]
        public string CelPhone { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Parentesco")]
        public string Relationship { get; set; }
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
        [Display(Name = "Estado")]
        public int StateID { get; set; }
        [NotMapped]
        public string CreatedByName { get; set; }
        [NotMapped]
        public string UpdatedByName { get; set; }
    }
}
