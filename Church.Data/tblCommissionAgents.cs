using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class tblCommissionAgents
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int CommissionAgentID { get; set; }
        [Display(Name = "Parroquia")]
        public int CathedralID { get; set; }
        [StringLength(60)]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Apellido Paterno")]
        public string PSurname { get; set; }

        [StringLength(50)]
        [Display(Name = "Apellido Materno")]
        public string MSurname { get; set; }
        [Display(Name = "Teléfono")]
        public string Phone { get; set; }
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
        
        [NotMapped]
        public string Cathedral { get; set; }
        
        [NotMapped]
        [Display(Name = "Ciudad")]
        public int CityID { get; set; }

        [NotMapped]
        public string City { get; set; }
        [NotMapped]
        public string FullName { get; set; }
    }
}
