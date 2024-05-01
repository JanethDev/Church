using church.backend.Models.catalogue.cathedral;
using church.backend.Models.catalogue.discounts;
using church.backend.Models.enums;
using church.backend.services.DataBase;
using church.backend.services.Models;
using System.Data.SqlClient;

namespace church.backend.services.Services
{
    public class DiscountServices
    {
        private readonly discountDB _discountDB;

        public DiscountServices(discountDB discountDB)
        {
            _discountDB = discountDB;
        }

        public discount_response consultAll()
        {
            return _discountDB.consultAll();
        }

        public discount_response consultByStatus(discount_status status)
        {
            return _discountDB.consultByStatus(status);
        }

        public GeneralResponse createDiscount(create_discount_request data, int user_id)
        {
            if (string.IsNullOrWhiteSpace(data.description)) {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar una descripción del descuento"
                };
            }
            if (data.percentage<=0 || data.percentage>100)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "El porcentaje solo puede tomar valores de 1 a 100"
                };
            }
            if(data.end_date == new DateTime())
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar una fecha de inicio"
                };
            }
            if (data.end_date == new DateTime())
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar una fecha de finalizado"
                };
            }
            if (data.start_date > data.end_date)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "La fecha de finalizado debe ser mayor o igual a la fecha de inicio"
                };
            }
            return _discountDB.createDiscount(data, user_id);
        }

        public GeneralResponse updateDiscount(update_discount_request data, int user_id)
        {
            return _discountDB.updateDiscount(data, user_id);
        }
    }
}
