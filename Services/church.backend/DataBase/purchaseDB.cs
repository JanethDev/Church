using church.backend.Models.enums;
using church.backend.Models.purchase;
using church.backend.services.Models;
using church.backend.services.Models.enums;
using church.backend.Utilities;
using System.Data.SqlClient;

namespace church.backend.DataBase
{
    public class purchaseDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        private readonly NullValues _nv;
        public purchaseDB(IConfiguration configuration, NullValues nv)
        {
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
            _nv = nv;
        }
        public CurrencyResponse consultCurrencies()
        {
            try
            {
                CurrencyResponse response = new CurrencyResponse() { code = 1 };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = _configuration["queries:purchase:paymentCurrency"]!;
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new Currency()
                                {
                                    id = _nv.nullInt(reader["id"].ToString()!),
                                    currency = reader["currency"].ToString()!
                                });
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new CurrencyResponse()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public TypePaymentsResponse consultTypePayments()
        {
            try
            {
                TypePaymentsResponse response = new TypePaymentsResponse() { code = 1 };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = _configuration["queries:purchase:paymentType"]!;
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new TypePayment()
                                {
                                    id = _nv.nullInt(reader["id"].ToString()!),
                                    typePayment = reader["type_payment"].ToString()!
                                });
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new TypePaymentsResponse()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public GeneralResponse CreatePurchase(purchase_request data)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:purchase:createPurchase"]!
                                                , data.tuition
                                                , data.datePurchase.ToString("yyyy-MM-dd")
                                                , data.statusId
                                                , data.cryptId
                                                , data.cryptPrice
                                                , data.cryptSpaces
                                                , data.maintenanceFee
                                                , data.federalTax
                                                , data.discountId
                                                , data.discountAmount
                                                , data.ashDeposit
                                                , data.customerId
                                                , data.userId
                                                , data.monthlyPayments
                                                , data.referencePerson1
                                                , data.referencePersonPhone1
                                                , data.referencePerson2
                                                , data.referencePersonPhone2
                    );
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response = new GeneralResponse()
                                {
                                    code = int.Parse(reader["code"].ToString()!),
                                    message = reader["message"].ToString()!
                                };
                            }
                        }
                    }

                    if(response.code > 0)
                    {
                        double amountPayed = 0;
                        foreach(var item in data.payments)
                        {
                            item.number = 0;
                            item.date = DateTime.Now;
                            item.statusId = (int)payment_status.pagado;
                            amountPayed = amountPayed + item.paymentAmount;
                            CreatePayment(item, response.code);
                        }

                        double amount  = (data.cryptPrice+data.ashDeposit- amountPayed) /data.monthlyPayments;

                        for(int item = 1; item <= data.monthlyPayments; item++)
                        {
                            payment_request payment = new payment_request()
                            {
                                number = item,
                                date = DateTime.Now.AddMonths(item),
                                paymentAmount = amount,
                                concept = "Abono "+item,
                                statusId = (int)payment_status.pendiente
                            };
                            CreatePayment(payment, response.code);
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public void CreatePayment(payment_request data, int purchaseId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:purchase:createPayment"]!
                                                , purchaseId
                                                , data.paymentAmount
                                                , data.statusId
                                                , data.date.ToString("yyyy-MM-dd")
                                                , data.number
                                                , data.concept
                                                , data.typePaymentId
                                                , data.currencyId
                    );
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()){}
                        }
                    }
                }
            }
            catch {}
        }
    
        public PurchaseResponse ConsultPurchaceByClient(int customerId)
        {
            try
            {
                PurchaseResponse response = new PurchaseResponse() { code = 1};
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:purchase:purchaseByClient"]!
                                                , customerId
                    );
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Purchase temporalPurchar = new Purchase(){
                                    purchaseId = _nv.nullInt(reader["id"].ToString()!),
                                    tuition = reader["tuition"].ToString()!,
                                    datePurchase = _nv.nullDate(reader["date_purchase"].ToString()!),
                                    statusId = _nv.nullInt(reader["cat_status_id"].ToString()!),
                                    status = reader["status"].ToString()!,
                                    cryptId = _nv.nullInt(reader["cat_crypts_id"].ToString()!),
                                    cryptPrice = _nv.nullDouble(reader["crypt_price"].ToString()!),
                                    cryptSpaces = _nv.nullInt(reader["crypt_spaces"].ToString()!),
                                    maintenanceFee = _nv.nullDouble(reader["maintenance_fee"].ToString()!),
                                    federalTax = _nv.nullDouble(reader["federal_tax"].ToString()!),
                                    discountId = _nv.nullInt(reader["cat_discounts_id"].ToString()!),
                                    discountAmount = _nv.nullDouble(reader["discount"].ToString()!),
                                    ashDeposit = _nv.nullDouble(reader["ash_deposit"].ToString()!),
                                    customerId = _nv.nullInt(reader["cat_customers_id"].ToString()!),
                                    userId = _nv.nullInt(reader["cat_customers_id"].ToString()!),
                                    monthlyPayments = _nv.nullInt(reader["monthly_payments"].ToString()!),
                                    referencePerson1 = reader["reference_person_1"].ToString()!,
                                    referencePersonPhone1 = reader["reference_person_1_phone"].ToString()!,
                                    referencePerson2 = reader["reference_person_2"].ToString()!,
                                    referencePersonPhone2 = reader["reference_person_2_phone"].ToString()!,
                                };
                                temporalPurchar.payments = paymentsByPurchase(temporalPurchar.purchaseId);
                                response.data.Add(temporalPurchar);
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new PurchaseResponse()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public PurchaseResponse ConsultPurchaceByStatus(int statusId)
        {
            try
            {
                PurchaseResponse response = new PurchaseResponse() { code = 1};
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:purchase:purchaseByStatus"]!
                                                , statusId
                    );
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Purchase temporalPurchar = new Purchase(){
                                    purchaseId = _nv.nullInt(reader["id"].ToString()!),
                                    tuition = reader["tuition"].ToString()!,
                                    datePurchase = _nv.nullDate(reader["date_purchase"].ToString()!),
                                    statusId = _nv.nullInt(reader["cat_status_id"].ToString()!),
                                    status = reader["status"].ToString()!,
                                    cryptId = _nv.nullInt(reader["cat_crypts_id"].ToString()!),
                                    cryptPrice = _nv.nullDouble(reader["crypt_price"].ToString()!),
                                    cryptSpaces = _nv.nullInt(reader["crypt_spaces"].ToString()!),
                                    maintenanceFee = _nv.nullDouble(reader["maintenance_fee"].ToString()!),
                                    federalTax = _nv.nullDouble(reader["federal_tax"].ToString()!),
                                    discountId = _nv.nullInt(reader["cat_discounts_id"].ToString()!),
                                    discountAmount = _nv.nullDouble(reader["discount"].ToString()!),
                                    ashDeposit = _nv.nullDouble(reader["ash_deposit"].ToString()!),
                                    customerId = _nv.nullInt(reader["cat_customers_id"].ToString()!),
                                    userId = _nv.nullInt(reader["cat_customers_id"].ToString()!),
                                    monthlyPayments = _nv.nullInt(reader["monthly_payments"].ToString()!),
                                    referencePerson1 = reader["reference_person_1"].ToString()!,
                                    referencePersonPhone1 = reader["reference_person_1_phone"].ToString()!,
                                    referencePerson2 = reader["reference_person_2"].ToString()!,
                                    referencePersonPhone2 = reader["reference_person_2_phone"].ToString()!,
                                };
                                temporalPurchar.payments = paymentsByPurchase(temporalPurchar.purchaseId);
                                response.data.Add(temporalPurchar);
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new PurchaseResponse()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public List<Payment> paymentsByPurchase(int purchaseId)
        {
            try
            {
                List<Payment>  response = new List<Payment>();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:purchase:paymentsByPurchase"]!
                                                , purchaseId
                    );
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.Add(new Payment(){
                                    paymentAmount = _nv.nullDouble(reader["amount"].ToString()!),
                                    concept = reader["payment_concept"].ToString()!,
                                    typePaymentId = _nv.nullInt(reader["trans_purchase_payments_type_id"].ToString()!),
                                    typePayment = reader["type_payment"].ToString()!,
                                    currencyId = _nv.nullInt(reader["trans_purchase_payments_currency_id"].ToString()!),
                                    currency = reader["currency"].ToString()!,
                                    number = _nv.nullInt(reader["number_payment"].ToString()!),
                                    date = _nv.nullDate(reader["date_payment"].ToString()!),
                                    statusId = _nv.nullInt(reader["cat_status_id"].ToString()!),
                                    status = reader["status"].ToString()!,
                                });
                            }
                        }
                    }
                }
                return response;
            }
            catch
            {
                return new List<Payment>();
            }
        }
    }
}
