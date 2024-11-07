using church.backend.DataBase;
using church.backend.Models.catalogue.crypts;
using church.backend.Models.catalogue.discounts;
using church.backend.Models.enums;
using church.backend.Models.purchase;
using church.backend.services.DataBase;
using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Models.access;
using church.backend.services.Models.enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            //valida si el vendedor tambien es el promotor
            if(data.userPromotorId<=0){
                data.userPromotorId = data.userId;
            }

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

            int tempCryptId = data.cryptTransferId<=0
                              ? data.cryptId
                              : data.cryptTransferId;
                              
            crypt_response crypt = _cryptDB.consultById(tempCryptId);
            if(crypt.data.Count == 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar el id de la cripta valida"
                };
            }
            crypt tempCrypt = crypt.data[0];

            if(tempCrypt.status_id != (int)crypt_status.disponible)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "La cripta seleccionada no se encuentra disponible"
                };
            }

            tempCrypt.status_id = data.statusId==(int)purchase_status.proceso
                                ? (int)crypt_status.vendido 
                                : (int)crypt_status.apartado;
                                
            update_crypt_request updateReques = new update_crypt_request(){
                id = tempCrypt.id,
                status_id = tempCrypt.status_id,
                is_shared = tempCrypt.is_shared,
                price = tempCrypt.price,
                price_shared = tempCrypt.price_shared,
                places_shared = tempCrypt.places_shared - data.cryptSpaces,
            };
            _cryptDB.updateCrypt(updateReques, userId);
            data.cryptPrice = tempCrypt.price;

            //actualiza cripta temporal como apartada
            if(data.cryptTransferId>0){
                crypt_response cryptTemp = _cryptDB.consultById(data.cryptId);
                if(cryptTemp.data.Count > 0){
                    update_crypt_request updateRequest = new update_crypt_request(){
                        id = cryptTemp.data[0].id,
                        status_id = (int)crypt_status.apartado,
                        is_shared = cryptTemp.data[0].is_shared,
                        price = cryptTemp.data[0].price,
                        price_shared = cryptTemp.data[0].price_shared,
                        places_shared = cryptTemp.data[0].places_shared - data.cryptSpaces,
                    };
                    _cryptDB.updateCrypt(updateRequest, userId);
                }
            }

            if (data.discountId > 0)
            {
                discount_response temp = _discountDB.consultAll();
                discount discount = temp.data.Find(item=>item.id == data.discountId)!;
                data.discountAmount = discount.percentage*data.cryptPrice;
            }
            data.userId = userId;
            data.datePurchase = DateTime.Now;

            GeneralResponse response = _purchaseDB.CreatePurchase(data);
            
            //Guarda id de contrato
            if(response.code!=-1 && data.statusId == (int)purchase_status.proceso)
            {
                _purchaseDB.updateTuitionPurchase(response.code);
            }

            return response;
        }
    
        public PurchaseResponse ConsultPurchaceByClient(int customerId)
        {
            if(customerId<=0){
                return new PurchaseResponse(){
                    code = -1,
                    message = "Es necesario enviar el id del cliente"
                };
            }
            return _purchaseDB.ConsultPurchaceByClient(customerId);
        }

        public PurchaseResponse ConsultPurchaceByStatus(int statusId)
        {
            if(statusId<=0){
                return new PurchaseResponse(){
                    code = -1,
                    message = "Es necesario enviar el id de estatus"
                };
            }
            return _purchaseDB.ConsultPurchaceByStatus(statusId);
        }

        public PurchaseResponse ConsultPurchaceById(int purchaseId)
        {
            if (purchaseId <= 0)
            {
                return new PurchaseResponse()
                {
                    code = -1,
                    message = "Es necesario enviar el id de la compra"
                };
            }
            return _purchaseDB.ConsultPurchaceById(purchaseId);
        }

        public GeneralResponse updateStatusPurchase(int purchaseId, int statusId, int userId)
        {
            if (purchaseId <= 0)
            {
                return new PurchaseResponse()
                {
                    code = -1,
                    message = "Es necesario enviar el id de la compra"
                };
            }
            if (statusId <= 0)
            {
                return new PurchaseResponse()
                {
                    code = -1,
                    message = "Es necesario enviar el id de estatus"
                };
            }

            if(statusId == (int)purchase_status.cancelado)
            {
                PurchaseResponse purchaseData = _purchaseDB.ConsultPurchaceById(purchaseId);
                
                if (purchaseData.code != 1)
                {
                    return new GeneralResponse()
                    {
                        code = purchaseData.code,
                        message = purchaseData.message
                    };
                }

                crypt_response crypt1 = _cryptDB.consultById(purchaseData.data[0].cryptId);
                update_crypt_request updateRequest1 = new update_crypt_request()
                {
                    id = crypt1.data[0].id,
                    status_id = (int)crypt_status.disponible,
                    is_shared = crypt1.data[0].is_shared,
                    price = crypt1.data[0].price,
                    price_shared = crypt1.data[0].price_shared,
                    places_shared = crypt1.data[0].places_shared,
                };
                _cryptDB.updateCrypt(updateRequest1, userId);

                if (purchaseData.data[0].cryptTransferId != 0)
                {
                    crypt_response crypt2 = _cryptDB.consultById(purchaseData.data[0].cryptTransferId);
                    update_crypt_request updateRequest2 = new update_crypt_request()
                    {
                        id = crypt2.data[0].id,
                        status_id = (int)crypt_status.disponible,
                        is_shared = crypt2.data[0].is_shared,
                        price = crypt2.data[0].price,
                        price_shared = crypt2.data[0].price_shared,
                        places_shared = crypt2.data[0].places_shared,
                    };
                    _cryptDB.updateCrypt(updateRequest2, userId);
                }
            }

            if (statusId == (int)purchase_status.proceso)
            {
                _purchaseDB.updateStatusPurchase(purchaseId, statusId);
                return _purchaseDB.updateTuitionPurchase(purchaseId);
            }
            else
            {

                return _purchaseDB.updateStatusPurchase(purchaseId, statusId);
            }
        }
    }
}
