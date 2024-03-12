using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class tblPushNotificationsMessages
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Folio")]
        public int PushNotificationMessageID { get; set; }

        [Display(Name = "Título")]
        [StringLength(50, ErrorMessage = "Este campo no debe exceder de 50 caracteres")]
        public string Title { get; set; }

        [Display(Name = "Mensaje")]
        [StringLength(140, ErrorMessage = "Este campo no debe exceder de 140 caracteres")]
        public string Message { get; set; }

        [Display(Name = "Estatus")]
        public bool Draft { get; set; }

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
        [NotMapped] public bool nmDraft { get; set; }
        [NotMapped] public string CreatedByName { get; set; }
        [NotMapped] public string UpdatedByName { get; set; }
    }
}
