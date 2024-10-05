using church.backend.DataBase;
using church.backend.Models.catalogue.crypts;
using church.backend.Models.catalogue.discounts;
using church.backend.Models.purchase;
using church.backend.services.DataBase;
using church.backend.services.JsonWebToken;
using church.backend.services.Models;

namespace church.backend.Services
{
    public class PurchaseServices
    {
        private readonly cryptDB _cryptDB;
        private readonly purchaseDB _purchaseDB;
        private readonly discountDB _discountDB;

        public PurchaseServices(purchaseDB purchaseDB, cryptDB cryptDB, discountDB discountDB)
        {
            _purchaseDB = purchaseDB;
            _cryptDB = cryptDB;
            _discountDB = discountDB;
        }

        public CurrencyResponse consultCurrencies()
        {
            return _purchaseDB.consultCurrencies();
        }

        public TypePaymentsResponse consultTypePayments()
        {
            return _purchaseDB.consultTypePayments();
        }

        public GeneralResponse CreatePurchase(purchase_request data, int userId)
        {
            if (data.customerId <= 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar el id del cliente"
                };
            }

            if (data.cryptId <= 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar el id de la cripta"
                };
            }

            crypt_response crypt = _cryptDB.consultById(data.cryptId);
            if(crypt.data.Count == 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar el id de la cripta valida"
                };
            }

            data.cryptPrice = crypt.data[0].price;

            if (data.discountId > 0)
            {
                discount_response temp = _discountDB.consultAll();
                discount discount = temp.data.Find(item=>item.id == data.discountId)!;
                data.discountAmount = discount.percentage*data.cryptPrice;
            }
            data.userId = userId;
            data.datePurchase = DateTime.Now;
            return _purchaseDB.CreatePurchase(data);
        }
    }
}
