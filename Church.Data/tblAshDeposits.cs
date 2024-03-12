using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class tblAshDeposits
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int AshDepositID { get; set; }
        [Display(Name = "Solicitud")]
        public int PurchasesRequestID { get; set; }
        [Display(Name = "Importe pagado")]
        public int FederalTaxID { get; set; }
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
        [StringLength(50)]
        [Display(Name = "Relación")]
        public string Relationship { get; set; }
        [Display(Name = "Fecha de nacimiento")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Fecha de defunción")]
        public DateTime DeathDate { get; set; }
        [Display(Name = "Fecha de deposito")]
        public DateTime AshDepositDate { get; set; }
        [Display(Name = "Recibo de pago")]
        public string Ticket { get; set; }
        [Display(Name = "Acta de defunción")]
        public string DeathCertificate { get; set; }
        [Display(Name = "Permiso de cremación")]
        public string CremationCertificate { get; set; }
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
        public decimal FederalTax { get; set; }
        [NotMapped]
        public string CreatedByName { get; set; }
        [NotMapped]
        public string UpdatedByName { get; set; }
        [NotMapped]
        public string Religion { get; set; }
        [NotMapped]
        public string Sacrament { get; set; }
        [NotMapped]
        public string CivilStatus { get; set; }
        [NotMapped]
        public string DeathCause { get; set; }
        [NotMapped]
        public string Applicant { get; set; }
        [NotMapped]
        public string Phone { get; set; }
        [NotMapped]
        public int ContractID { get; set; }
        [NotMapped]
        public string Nicho { get; set; }
        [NotMapped]
        public string ProvicionalNicho { get; set; }
        [NotMapped]
        public int Level { get; set; }
        [NotMapped]
        public DateTime MisaDate { get; set; }
        [NotMapped]
        public DateTime Date { get; set; }
        [NotMapped]
        public string MisaHour { get; set; }
        [NotMapped]
        public string Hour { get; set; }
        [NotMapped]
        public int PurchasesRequestAshDepositID { get; set; }
        [NotMapped]
        public int CustomerID { get; set; }
        [NotMapped]
        public int Age { get; set; }
        [NotMapped]
        public string Customer { get; set; }
        [NotMapped]
        public string Wife { get; set; }
        [NotMapped]
        public string Sons { get; set; }
        [NotMapped]
        public string FullName { get; set; }
        [NotMapped]
        public int Month { get; set; }
        [NotMapped]
        public int Day { get; set; }
        [NotMapped]
        public int Year { get; set; }

        [NotMapped]
        public string DeathDateFormat
        {
            get
            {
                DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;

                string sMonth = dtinfo.GetMonthName(DeathDate.Month);
                
                sMonth = sMonth[0].ToString().ToUpper() + sMonth.Substring(1, sMonth.Length - 1);
                
                string value = $"{DeathDate.Day} de {sMonth}";

                return value;
            }
        }
    }
}
