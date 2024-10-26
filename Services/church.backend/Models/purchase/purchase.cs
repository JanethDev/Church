using church.backend.Models.register;
using church.backend.services.Models;

namespace church.backend.Models.purchase
{
    public class Purchase
    {
        public int userId {  get; set; } = new int();
        public int purchaseId {  get; set; } = new int();
        public string tuition { get; set; } = string.Empty;
        public DateTime datePurchase { get; set; } = new DateTime();
        public int statusId {  get; set; } = new int();
        public string status { get; set; } = string.Empty;
        public int cryptId {  get; set; } = new int(); 
        public double cryptPrice {  get; set; } = new double();
        public int cryptSpaces {  get; set; } = new int();
        public double maintenanceFee {  get; set; } = new double();
        public double federalTax {  get; set; } = new double();
        public int discountId {  get; set; } = new int();
        public double discountAmount {  get; set; } = new double();
        public double ashDeposit {  get; set; } = new double();
        public int customerId {  get; set; } = new int();
        public int monthlyPayments {  get; set; } = new int();
        public string referencePerson1 { get; set; } = string.Empty;
        public string referencePersonPhone1 { get; set; } = string.Empty;
        public string referencePerson2 { get; set; } = string.Empty;
        public string referencePersonPhone2 { get; set; } = string.Empty;
        public double otherFee { get; set; } = new double();
        public string signature { get; set; } = string.Empty;

        public int customerNumber {  get; set; } = new int();
        public string customerName { get; set; } = string.Empty;
        public string customerPsurname { get; set; } = string.Empty;
        public string customerMsurname { get; set; } = string.Empty;
        public string customerPhone { get; set; } = string.Empty;
        public string customerEmail { get; set; } = string.Empty;
        public string customerRFC { get; set; } = string.Empty;
        public string customerZipCode { get; set; } = string.Empty;
        public string customerAddress { get; set; } = string.Empty;
        public int customerStateId {  get; set; } = new int();
        public int customerTownId {  get; set; } = new int();
        public string customerSocialReason { get; set; } = string.Empty;
        public DateTime customerBirthdate { get; set; } = new DateTime();
        public string customerBirthplace { get; set; } = string.Empty;
        public string customerCivilStatus { get; set; } = string.Empty;
        public string customerOccupation { get; set; } = string.Empty;
        public string businessName { get; set; } = string.Empty;
        public string businessAddress { get; set; } = string.Empty;
        public string businessPhone { get; set; } = string.Empty;
        public string businessExt { get; set; } = string.Empty;
        public string deputation { get; set; } = string.Empty;
        public double averageIncome { get; set; } = new double();
        public string businessCity { get; set; } = string.Empty;
        public string businessMunicipality { get; set; } = string.Empty;
        public string businessState { get; set; } = string.Empty;
        public string houseNumber { get; set; } = string.Empty;
        public string aptNumber { get; set; } = string.Empty;
        public string customerMunicipality { get; set; } = string.Empty;
        public string neighborhood { get; set; } = string.Empty;
        public List<Beneficiarie> beneficiaries { get; set; } = new List<Beneficiarie>();
        public List<Payment> payments { get; set; } = new List<Payment>();
    }
}
