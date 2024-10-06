namespace church.backend.Models.purchase
{
    public class Payment
    {
        public double paymentAmount { get; set; } = new double();
        public string concept { get; set; } = string.Empty;
        public int typePaymentId { get; set; } = new int();
        public string typePayment { get; set; } = string.Empty;
        public int currencyId { get; set; } = new int();
        public string currency { get; set; } = string.Empty;
        public int number { get; set; } = new int();
        public DateTime date { get; set; } = new DateTime();
        public int statusId { get; set; } = new int();
        public string status { get; set; } = string.Empty;
    }
}
