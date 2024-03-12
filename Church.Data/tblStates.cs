using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class tblStates
    {
        [Key]
        public int StateID { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(150, ErrorMessage = "Este campo no debe exceder de 150 caracteres")]
        [Display(Name = "Estado")]
        public string Name { get; set; }
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
