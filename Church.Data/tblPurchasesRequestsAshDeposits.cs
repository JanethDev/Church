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
    public class tblPurchasesRequestsAshDeposits
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int PurchasesRequestAshDepositID { get; set; }
        public int AshDepositID { get; set; }
        [Display(Name = "Cliente")]
        public int CustomerID { get; set; }
        [Display(Name = "Contrato")]
        public int ContractID { get; set; }
        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }
        [Display(Name = "Hora")]
        public string Hour { get; set; }
        [Display(Name = "Nombre completo")]
        public string Name { get; set; }
        [Display(Name = "Edad")]
        public int Age { get; set; }
        [Display(Name = "Religión")]
        public string Religion { get; set; }
        [Display(Name = "Relacón")]
        public string Relationship { get; set; }
        [Display(Name = "Sacramento")]
        public string Sacrament { get; set; }
        [Display(Name = "Estado civil")]
        public string CivilStatus { get; set; }
        [Display(Name = "Causa de muerte ")]
        public string DeathCause { get; set; }
        [Display(Name = "Solicitó ")]
        public string Applicant { get; set; }
        [Display(Name = "Teléfono")]
        public string Phone { get; set; }
        [Display(Name = "Esposa")]
        public string Wife { get; set; }
        public string Sons { get; set; }
        [Display(Name = "Nicho adquirido")]
        public string Nicho { get; set; }
        [Display(Name = "Nicho provicional")]
        public string ProvicionalNicho { get; set; }
        [Display(Name = "Level")]
        public int Level { get; set; }
        [Display(Name = "Fecha de la misa")]
        public DateTime MisaDate { get; set; }
        [Display(Name = "Hora de la misa")]
        public string MisaHour { get; set; }
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
        public string Customer { get; set; }
        [NotMapped]
        public string PSurname { get; set; }
        [NotMapped]
        public string MSurname { get; set; }

        [NotMapped]
        public tblCryptsSections tblCryptsSections { get; set; }

        [NotMapped]
        public string Ticket { get; set; }
        [NotMapped]
        public string DeathCertificate { get; set; }
        [NotMapped]
        public string CremationCertificate { get; set; }
    }
}
