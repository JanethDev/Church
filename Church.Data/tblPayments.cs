using Church.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Church.Data
{
    public class tblPayments
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int PaymentID { get; set; }
        public int PurchaseRequestPaymentID { get; set; }
        public decimal Amount { get; set; }
        public decimal ExchangeRate { get; set; }
        public string TicketUrl { get; set; }
        public string ReceiptUrl { get; set; }
        public bool IsInterest { get; set; }
        public ClientPaymentEnum Estatus { get; set; }
        public DateTime? ApprovedRejectedDate { get; set; }
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
        public bool Validate { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }

        [NotMapped]
        public string Nicho { get; set; }
        [NotMapped]
        public DateTime PaymentDate { get; set; }
        [NotMapped]
        public string sPaymentDate => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(PaymentDate.ToString("MMMM yy", CultureInfo.CreateSpecificCulture("es-MX")));
        [NotMapped]
        public string NumPayment { get; set; }

        [NotMapped]
        public byte[] FileBytes { get; set; }

        [NotMapped]
        public string UserNotificationKey { get; set; }
        [NotMapped]
        public bool Currency { get; set; }
    }
}
