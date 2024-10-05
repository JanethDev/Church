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
                    string query = _configuration["queries:purchase:paymentCurrency"]!;
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
    }
}
