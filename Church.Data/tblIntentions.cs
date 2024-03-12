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
    public class tblIntentions
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int IntentionID { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Tipo de intención")]
        public int IntentionTypeID { get; set; }

        [StringLength(100)]
        //[Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Fecha de solicitud")]
        public DateTime ApplicationDate { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Fecha de intención")]
        public DateTime IntentionDate { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Hora")]
        public int ScheduleID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Persona mención")]
        public string MentionPerson { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Solicitante")]
        public string Applicant { get; set; }

        [StringLength(16)]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Donativo")]
        public decimal Donation { get; set; }

        [Display(Name = "Moneda")]
        public ExchangeTypes ExchangeType { get; set; }

        [StringLength(40)]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        #region Audit fields
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
        #endregion

        #region NotMapped
        [NotMapped]
        public string IntentionTypeName { get; set; }

        [NotMapped]
        public string ExchangeTypeName => EnumHelper<ExchangeTypes>.GetDisplayValue(ExchangeType);

        [NotMapped]
        public string CreatedByName { get; set; }

        [NotMapped]
        public string UpdatedByName { get; set; }

        [NotMapped]
        public tblIntentionsType tblIntentionsType { get; set; }
        [NotMapped]
        public int Month { get; set; }
        [NotMapped]
        public int Day { get; set; }
        [NotMapped]
        public int Year { get; set; }
        [NotMapped]
        public string Schedule { get; set; }
        #endregion
    }
}
