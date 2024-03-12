using Church.Mobile.DataLayer.AuxiliaryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Church.Mobile.DataLayer.ApiModels
{
    public class tblPurchasesRequestsDTO
    {
        public int PurchasesRequestID { get; set; }
        public string Nicho { get; set; }
        public int Level { get; set; }
        public string ClassificationName { get; set; }
        public string PaymentMethodName { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public decimal CryptPrice { get; set; }
        public string CryptPriceFormat
            => CryptPrice.ToString("C2");

        public List<tblAshDepositsDTO> lstAshDeposits { get; set; }

        public List<tblPurchaseRequestPaymentsDTO> lstPayments { get; set; }

        public List<tblPurchaseRequestPaymentsDTO> lstOverduePayments { get; set; }

        public tblPurchaseRequestPaymentsDTO tblNextPayments
            => lstOverduePayments?.Where(r => r.Estatus == AuxiliaryModels.PaymentEstatus.Pending).FirstOrDefault();

        public decimal Rate { get; set; }
        public bool Currency { get; set; }
    }
}
