using System.ComponentModel.DataAnnotations;

namespace church.backend.Models.register
{
    public class customer_response
    {
        public int id { get; set; } = new int();
        public int customerNumber { get; set; } = new int();
        public string email { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string father_last_name { get; set; } = string.Empty;
        public string mother_last_name { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string rfc { get; set; } = string.Empty;
        public int zip_code { get; set; } = new int();
        public string address { get; set; } = string.Empty;
        public int catStatesId { get; set; } = new int();
        public string state { get; set; } = string.Empty;
        public int catTownsId { get; set; } = new int();
        public string town { get; set; } = string.Empty;
        public string social_reason { get; set; } = string.Empty;
        public DateTime birthdate { get; set; } = new DateTime();
        public string birth_place { get; set; } = string.Empty;
        public string civil_status { get; set; } = string.Empty;
        public string occupation { get; set; } = string.Empty;
        public string business_name { get; set; } = string.Empty;
        public string business_address { get; set; } = string.Empty;
        public string business_city { get; set; } = string.Empty;
        public string business_municipality { get; set; } = string.Empty;
        public string business_state { get; set; } = string.Empty;
        public string business_phone { get; set; } = string.Empty;
        public string business_ext { get; set; } = string.Empty;
        public string deputation { get; set; } = string.Empty;
        public string house_number { get; set; } = string.Empty;
        public string apt_number { get; set; } = string.Empty;
        public string customer_municipality { get; set; } = string.Empty;
        public string neighborhood { get; set; } = string.Empty;
        public double average_income { get; set; } = new double();
    }
}
