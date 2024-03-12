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
    public class tblPurchasesRequests
    {

        [Display(Name = "ID")]
        public int PurchasesRequestID { get; set; }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PurchasesRequestID2 { get; set; }

        public int DiscountID { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }

        [Display(Name = "Contrato")]
        public int ContractID { get; set; }

        [Display(Name = "Comisionista")]
        public int? CommissionAgentID { get; set; }

        [Display(Name = "Cliente")]
        public int CustomerID { get; set; }

        [Display(Name = "CLAVE DE LA CRIPTA")]
        public string CryptKey { get; set; }

        public CryptTypes CryptType { get; set; }

        public int? CryptPosition { get; set; }

        public string Nicho { get; set; }

        public int Quantity { get; set; }

        public bool Currency { get; set; }


        [Display(Name = "PLAN DE VENTA")]
        public PaymentMethods PaymentMethod { get; set; }

        [Display(Name = "CryptSectionID")]
        public int CryptSectionID { get; set; }
        [Display(Name = "Numero de factura")]
        public int BillingNumber { get; set; }
        [Display(Name = "Factura")]
        public string Billing { get; set; }

        [Display(Name = "Precio cripta")]
        public decimal CryptPrice { get; set; }
        [Display(Name = "Precio original cripta")]
        public decimal OriginalPrice { get; set; }

        [Display(Name = "Enganche")]
        public decimal Enganche { get; set; }

        [Display(Name = "Mensualidades")]
        public decimal Mensualidades { get; set; }

        [Display(Name = "Fecha de primer pago")]
        public DateTime? FirstPaymentDate { get; set; }

        [Display(Name = "Fecha de ultimo pago")]
        public DateTime? LastPaymentDate { get; set; }

        [Display(Name = "Tipo de pago")]
        public int TypePay { get; set; }
        [Display(Name = "Recibo")]
        public string Ticket { get; set; }
        [Display(Name = "Recibo ")]
        public string TicketCashDeposit { get; set; }
        [Display(Name = "Pago en efectivo")]
        public decimal? CashPaymentAmount { get; set; }
        [Display(Name = "transferencia")]
        public decimal? CashPaymentAmountOrTransfer { get; set; }
        [Display(Name = "Deposito en efectivo")]
        public decimal? CashDeposit { get; set; }

        [Display(Name = "Pago en cheque")]
        public decimal? CheckPaymentAmount { get; set; }

        [Display(Name = "Número de cheque")]
        public string CheckPaymentNo { get; set; }

        [Display(Name = "Número de cuenta de cheque")]
        public string CheckPaymentAccount { get; set; }

        [Display(Name = "Banco del cheque")]
        public string CheckPaymentBank { get; set; }

        [Display(Name = "Pago con tarjeta de credito")]
        public decimal? CreditCardPaymentAmount { get; set; }

        public string CreditCardPaymentNo { get; set; }

        [Display(Name = "Número de cuenta tarjeta de credito")]
        public string CreditCardPaymentAccount { get; set; }

        [Display(Name = "Banco de tarjeta de credito")]
        public string CreditCardPaymentBank { get; set; }
        [Display(Name = "Level")]
        public int Level { get; set; }
        public bool Cancel { get; set; }
        [Display(Name = "Activo")]
        public bool Active { get; set; }
        public bool CheckMaintenanceFee { get; set; }
        public bool CheckFederalTax { get; set; }
        public int MaintenanceFeeID { get; set; }
        public int FederalTaxID { get; set; }
        public bool BillingChecked { get; set; }
        public string ReferenceCustomer1 { get; set; }
        public string ReferenceCustomerPhone1 { get; set; }
        public string ReferenceCustomer2 { get; set; }
        public string ReferenceCustomerPhone2 { get; set; }
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
        public string Name { get; set; }
        [NotMapped]
        public string PSurname { get; set; }
        [NotMapped]
        public string MSurname { get; set; }
        [NotMapped]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Correo eletrónico no valido")]
        public string Email { get; set; }
        [NotMapped]
        public bool Prospective { get; set; }
        [NotMapped]
        public int? TownID { get; set; }
        [NotMapped]
        public string Town { get; set; }
        [NotMapped]
        public string Delegation { get; set; }
        [NotMapped]
        public int? StateID { get; set; }
        [NotMapped]
        public string State { get; set; }
        [NotMapped]
        public string CelPhone { get; set; }

        [NotMapped]
        public string Address { get; set; }
        [NotMapped]
        public string AddressNumber { get; set; }
        [NotMapped]
        public int? AdressInteriorNumber { get; set; }
        [NotMapped]
        public string Neighborhood { get; set; }
        [NotMapped]
        public int? ZipCode { get; set; }
        [NotMapped]
        public string RFCCURP { get; set; }
        [NotMapped]
        public string BusinessName { get; set; }
        [NotMapped]
        public DateTime? DateOfBirth { get; set; }
        [NotMapped]
        public string sDateOfBirth { get; set; }
        [NotMapped]
        public int? CivilStatusID { get; set; }
        [NotMapped]
        public string CivilStatus { get; set; }
        [NotMapped]
        public string Occupation { get; set; }
        [NotMapped]
        public string Company { get; set; }
        [NotMapped]
        public string PhoneCompany { get; set; }
        [NotMapped]
        public string AddressCompany { get; set; }
        [NotMapped]
        public int? ExtPhoneCompany { get; set; }
        [NotMapped]
        public int? TownAddressCompanyID { get; set; }
        [NotMapped]
        public string TownAddressCompany { get; set; }
        [NotMapped]
        public string CityOfBirth { get; set; }
        [NotMapped]
        public string DelegationAddressCompany { get; set; }
        [NotMapped]
        public string StateAddressCompany { get; set; }
        [NotMapped]
        public int? StateAddressCompanyID { get; set; }
        [NotMapped]
        public int? Income { get; set; }
        [NotMapped]
        public int UserID { get; set; }

        [NotMapped]
        public string PaymentMethodName
            => EnumHelper<PaymentMethods>.GetDisplayValue(PaymentMethod);

        [NotMapped]
        public CryptsNichos Classification { get; set; }

        [NotMapped]
        public string ClassificationName
            => EnumHelper<CryptsNichos>.GetDisplayValue(Classification);
        [NotMapped]
        public tblCryptsSections tblCryptsSections { get; set; }

        [NotMapped]
        public decimal MaintenanceFee { get; set; }
        [NotMapped]
        public decimal FederalTax { get; set; }
        [NotMapped]
        public string Discount { get; set; }
        [NotMapped]
        public string DiscountDescription { get; set; }
        [NotMapped]
        public bool Quotation { get; set; }
        [NotMapped]
        public int PurchasesRequestQuotationID { get; set; }

        [NotMapped]
        public List<tblPurchaseRequestPayments> lstPayments { get; set; }

        [NotMapped]
        public List<tblPurchaseRequestPayments> lstOverduePayments
            => lstPayments?.Where(r => r.OverduePayment)?.ToList();

        [NotMapped]
        public tblPurchaseRequestPayments tblNextPayments
            => lstPayments?.Where(r => !r.OverduePayment)?.FirstOrDefault();

        [NotMapped]
        public List<tblAshDeposits> lstAshDeposits { get; set; }
        [NotMapped]
        public string CommissionAgent { get; set; }

        [NotMapped]
        public decimal Rate { get; set; }
    }
}
