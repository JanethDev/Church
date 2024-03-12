using Church.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class dtoPurchasesRequest
    {
        public int TotalCount { get; set; }
        public Int64 ROWNUM { get; set; }
        public int PurchasesRequestID { get; set; }
        public int ContractID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CryptKey { get; set; }
        public CryptsNichos Classification { get; set; }
        public string ClassificationName
            => EnumHelper<CryptsNichos>.GetDisplayValue(Classification);
        public string Nicho { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string AddressNumber { get; set; }
        public string AdressInteriorNumber { get; set; }
        public string Neighborhood { get; set; }
        public int ZipCode { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal CryptPrice { get; set; }
        public decimal Mensualidades { get; set; }
        public string Phone { get; set; }
        public string CelPhone { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string CityOfBirth { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CivilStatus { get; set; }
        public string Occupation { get; set; }
        public bool CheckMaintenanceFee { get; set; }
        public bool CheckFederalTax { get; set; }
    }
}
