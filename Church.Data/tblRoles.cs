using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class tblRoles
    {
        [Key]
        public int RolID { get; set; }


        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(50, ErrorMessage = "Este campo no debe exceder de 50 caracteres")]
        [Display(Name = "Rol")]
        [DTOAtribute(Name = "Rol")]       
        public string Name { get; set; }

    }
}
