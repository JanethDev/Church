using church.backend.Models.purchase;
using church.backend.services.Models;
using church.backend.services.Models.enums;
using church.backend.services.Models.register;
using church.backend.services.Services;
using church.backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace church.backend.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly PurchaseServices _PurchaseServices;
        public PurchaseController(PurchaseServices PurchaseServices)
        {
            _PurchaseServices = PurchaseServices;
        }

        /// <summary>
        /// Devuelve la lista de tipo de moneda
        /// </summary>
        /// <returns>Devuelve la lista de tipo de moneda</returns>
        /// <response code="200">Listado de tipo de moneda</response>
        /// <response code="400">Retorna algun error</response>
        [HttpGet]
        [Route("purchase/currency")]
        public IActionResult consultCurrencies()
        {
            CurrencyResponse response = _PurchaseServices.consultCurrencies();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Devuelve la lista de tipo de pago
        /// </summary>
        /// <returns>Devuelve la lista de tipo de pago</returns>
        /// <response code="200">Listado de tipo de pago</response>
        /// <response code="400">Retorna algun error</response>
        [HttpGet]
        [Route("purchase/TypePayments")]
        public IActionResult consultTypePayments()
        {
            TypePaymentsResponse response = _PurchaseServices.consultTypePayments();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Creación de compras, requiere token
        /// </summary>
        /// <param name="data"></param>
        /// <returns>crea un nueva compra</returns>
        /// <remarks>
        /// Ejemplo de llenado:
        ///
        ///     {
        ///        "tuition": "ejemplo",
        ///        "cryptId": 1,
        ///        "cryptSpaces": 4,
        ///        "maintenanceFee": 10.5,
        ///        "federalTax" : 10.5,
        ///        "discountId" : 2,
        ///        "ashDeposit" : 10.5,
        ///        "customerId" : 1,
        ///        "monthlyPayments" : 30,
        ///        "referencePerson1" : "test",
        ///        "referencePersonPhone1" : "test",
        ///        "referencePerson2" : "test",
        ///        "referencePersonPhone2" : "test",
        ///        "payments":  [
        ///                     {
        ///                         "paymentAmount" : 10000.30,
        ///                         "concept" : "pago inicial",
        ///                         "typePaymentId" : 1,
        ///                         "currencyId" : 1
        ///                     }
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Significa que agregó la compra correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpPost]
        [Route("purchase/create")]
        public IActionResult CreatePurchase([FromBody] purchase_request data)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            data.statusId = (int)purchase_status.proceso;
            GeneralResponse client = _PurchaseServices.CreatePurchase(data, user_id);
            if (client.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, client.message);
            }
            return Ok(client.message);
        }

        /// <summary>
        /// Creación de compras, requiere token
        /// </summary>
        /// <param name="data"></param>
        /// <returns>crea un nueva compra</returns>
        /// <remarks>
        /// Ejemplo de llenado:
        ///
        ///     {
        ///        "tuition": "ejemplo",
        ///        "cryptId": 1,
        ///        "cryptSpaces": 4,
        ///        "maintenanceFee": 10.5,
        ///        "federalTax" : 10.5,
        ///        "discountId" : 2,
        ///        "ashDeposit" : 10.5,
        ///        "customerId" : 1,
        ///        "monthlyPayments" : 30,
        ///        "referencePerson1" : "test",
        ///        "referencePersonPhone1" : "test",
        ///        "referencePerson2" : "test",
        ///        "referencePersonPhone2" : "test",
        ///        "payments":  [
        ///                     {
        ///                         "paymentAmount" : 10000.30,
        ///                         "concept" : "pago inicial",
        ///                         "typePaymentId" : 1,
        ///                         "currencyId" : 1
        ///                     }
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Significa que agregó la compra correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpPost]
        [Route("purchase/reserve")]
        public IActionResult ReservePurchase([FromBody] purchase_request data)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            data.statusId = (int)purchase_status.presolicitud;
            GeneralResponse client = _PurchaseServices.CreatePurchase(data, user_id);
            if (client.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, client.message);
            }
            return Ok(client.message);
        }

        /// <summary>
        /// Devuelve la lista las compras por cliente
        /// </summary>
        /// <returns>Devuelve la lista las compras por cliente</returns>
        /// <response code="200">lista las compras por cliente</response>
        /// <response code="400">Retorna algun error</response>
        [HttpGet]
        [Route("purchase/by/customer")]
        public IActionResult ConsultPurchaceByClient([FromQuery] int customerId)
        {
            PurchaseResponse response = _PurchaseServices.ConsultPurchaceByClient(customerId);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Devuelve la lista las compras reservadas/apartadas
        /// </summary>
        /// <returns>Devuelve la lista las compras reservadas/apartadas</returns>
        /// <response code="200">lista las compras reservadas/apartadas</response>
        /// <response code="400">Retorna algun error</response>
        [HttpGet]
        [Route("purchase/reserved")]
        public IActionResult PurchaseReserved()
        {
            PurchaseResponse response = _PurchaseServices.ConsultPurchaceByClient((int)purchase_status.presolicitud);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }
    }
}
