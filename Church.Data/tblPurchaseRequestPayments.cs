using Church.Data.Enums;
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
    public class tblPurchaseRequestPayments
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int PurchaseRequestPaymentID { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Id de solicitud")]
        public int PurchasesRequestID { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Importe")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Esatus")]
        public PaymentEstatus Estatus { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Fecha de pago")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(10, ErrorMessage = "Este campo no debe exceder de 15 caracteres")]
        [Display(Name = "Número de pago")]
        public string NumPayment { get; set; }

        [Display(Name = "Ticket")]
        public string TicketUrl { get; set; }

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
        public bool OverduePayment { get; set; }

        [NotMapped]
        public bool IsOverduePayment { get; set; }

        [NotMapped]
        public bool HasInterest { get; set; }

        [NotMapped]
        public bool HasPaymentDone { get; set; }

        [NotMapped]
        public bool NextPayemntToDo { get; set; }

        [NotMapped]
        public string ReceiptUrl { get; set; }

        public string EstatusName
        {
            get
            {
                var value = EnumHelper<PaymentEstatus>.GetDisplayValue(Estatus);
                if (Estatus == PaymentEstatus.Pending && OverduePayment)
                    value = "Atrasado";
                return value;
            }
        }

        [NotMapped]
        public string PaymentDateFormat
        {
            get
            {
                DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;

                string sMonth = dtinfo.GetAbbreviatedMonthName(PaymentDate.Month);
                sMonth = sMonth[0].ToString().ToUpper() + sMonth.Substring(1, sMonth.Length - 2);
                string value = $"{PaymentDate.Day} {sMonth} {PaymentDate.Year}";

                return value;
            }
        }

        [NotMapped]
        public string EstatusColor
        {
            get
            {
                string color = "#A9A9A9";

                switch (Estatus)
                {
                    case PaymentEstatus.Pending:
                        color = "#A9A9A9";

                        break;
                    case PaymentEstatus.ValidationProcess:
                        color = "#8080FF";

                        break;
                    case PaymentEstatus.Paid:
                        color = "#008000";

                        break;
                }

                if (Estatus == PaymentEstatus.Pending && OverduePayment)
                    color = "#990000";

                return color;
            }
        }

        [NotMapped]
        public bool CanUploadFile
            => !HasPaymentDone && NextPayemntToDo;

        [NotMapped]
        public bool ShowReceipt
            => HasPaymentDone && Estatus == PaymentEstatus.Paid;
    }
}