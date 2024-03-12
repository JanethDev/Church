using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    
    public class tblUsers
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int UserID { get; set; }

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

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Rol")]      
        public int RolID { get; set; }

        [StringLength(80, ErrorMessage = "Correo eletrónico no debe exceder de 80 caracteres")]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Usuario")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Correo eletrónico no valido")]
        [EmailAddress]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Contraseña")]
        [MinLength(5, ErrorMessage = "La longitud mínima es de 5 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "La contraseña y su confirmacion no coinciden.")]
        [NotMapped]
        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string NotificationKeyName { get; set; }
        public string NotificationKey { get; set; }

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
        public string Rol { get; set; }
        [NotMapped]
        public string CreatedByName { get; set; }
        [NotMapped]
        public string UpdatedByName { get; set; }


        public tblRoles tblRoles { get; set; }


        [NotMapped]
        public string ID = "UserID";

        [NotMapped]
        public string UserFullName
        {
            get
            {
                return Name + " " + PSurname + " " + MSurname;
            }
        }
    }
}
