using System;
using System.Collections.Generic;
using System.Text;

namespace Church.Mobile.DataLayer.Models
{
    public class tblPayments
    {
        public int PurchaseRequestPaymentID { get; set; }
        public byte[] FileBytes { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsInterest { get; set; }
        public decimal Amount { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
