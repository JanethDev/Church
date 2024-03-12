using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class tblAllowedApps
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int AllowedAppID { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Nombre de la app")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "App ID")]
        public string AppId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "App Key")]
        public string AppKey { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Valido")]
        public bool Valid { get; set; }

    }
}
