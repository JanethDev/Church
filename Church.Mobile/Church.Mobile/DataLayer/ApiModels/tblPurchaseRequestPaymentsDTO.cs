using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using Church.Mobile.DataLayer.AuxiliaryModels;
using Church.Mobile.Helpers;

namespace Church.Mobile.DataLayer.ApiModels
{
    public class tblPurchaseRequestPaymentsDTO
    {
        public int PurchaseRequestPaymentID { get; set; }
        public string EstatusName { get; set; }
        public PaymentEstatus Estatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string NumPayment { get; set; }
        public decimal Amount { get; set; }
        public bool OverduePayment { get; set; }
        public string AmountFormat
            => Amount.ToString("C2");

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

        public Color BGColor
        {
            get
            {
                var color = new Color();
                switch (Estatus)
                {
                    case PaymentEstatus.Pending:
                        color = Constants.ColorGray;
                        break;
                    case PaymentEstatus.ValidationProcess:
                        color = Constants.ColorBlue;
                        break;
                    case PaymentEstatus.Paid:
                        color = Constants.ColorGreen;
                        break;
                }

                if (Estatus == PaymentEstatus.Pending && OverduePayment)
                    color = Constants.ColorRed;

                return color;
            }
        }

        public bool ShowMessageLatePayment =>
            IsOverduePayment;

        public bool IsOverduePayment { get; set; }
        public bool HasPaymentDone { get; set; }
        public bool NextPayemntToDo { get; set; }

        public bool CanUploadFile
            => !HasPaymentDone && NextPayemntToDo;
        
        public bool ShowReceipt
            => HasPaymentDone && Estatus == PaymentEstatus.Paid;

        public string ReceiptUrl { get; set; }
    }
}
