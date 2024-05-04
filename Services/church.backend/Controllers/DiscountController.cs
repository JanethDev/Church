using church.backend.Models.catalogue.discounts;
using church.backend.Models.enums;
using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace church.backend.services.Controllers
{
    public class DiscountController : Controller
    {
        private readonly DiscountServices _discountServices;
        public DiscountController(DiscountServices discountServices)
        {
            _discountServices = discountServices;
        }

        /// <summary>
        /// Consulta el catalogo de descuentos, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "description": "descuento #1",
        ///             "percentage" : 15.50,
        ///             "start_date: "2023-01-01",
        ///             "end_date: "2025-01-01",
        ///             "status_id": 5,
        ///             "status": "expirado"
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("discounts/all")]
        public IActionResult consultAll()
        {
            discount_response response = _discountServices.consultAll();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Consulta el catalogo de descuentos activos, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "description": "descuento #1",
        ///             "percentage" : 15.50,
        ///             "start_date: "2023-01-01",
        ///             "end_date: "2025-01-01",
        ///             "status_id": 4,
        ///             "status": "activo"
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("discounts/active")]
        public IActionResult consultActive()
        {
            discount_response response = _discountServices.consultByStatus(discount_status.Activo);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Consulta el catalogo de descuentos pendientes (el parametro de fechas aun no es vigente), requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "description": "descuento #1",
        ///             "percentage" : 15.50,
        ///             "start_date: "2023-01-01",
        ///             "end_date: "2025-01-01",
        ///             "status_id": 3,
        ///             "status": "pendiente"
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("discounts/pending")]
        public IActionResult consultPending()
        {
            discount_response response = _discountServices.consultByStatus(discount_status.Pendiente);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Consulta el catalogo de descuentos expirados, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "description": "descuento #1",
        ///             "percentage" : 15.50,
        ///             "start_date: "2023-01-01",
        ///             "end_date: "2025-01-01",
        ///             "status_id": 5,
        ///             "status": "expirado"
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("discounts/expired")]
        public IActionResult consultExpired()
        {
            discount_response response = _discountServices.consultByStatus(discount_status.Expirado);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Consulta el catalogo de descuentos desactivados (cancelados), requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "description": "descuento #1",
        ///             "percentage" : 15.50,
        ///             "start_date: "2023-01-01",
        ///             "end_date: "2025-01-01",
        ///             "status_id": 6,
        ///             "status": "desactivado"
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("discounts/deactive")]
        public IActionResult consultDeactive()
        {
            discount_response response = _discountServices.consultByStatus(discount_status.Desactivado);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Crea un nuevo descuento, requiere token
        /// </summary>
        /// <param name="data"></param>
        /// <returns>crea un nuevo descuento</returns>
        /// <remarks>
        /// Ejemplo de llenado:
        ///
        ///     {
        ///        "description": "promoción perrona de 90% de descuento",
        ///        "percentage" : 90.0,
        ///        "start_date" : "2024-02-15",
        ///        "end_date"   : "2025-08-30"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Significa que agregó la catedral correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpPost]
        [JwtAuthentication]
        [Route("create/discount")]
        public IActionResult createDiscount([FromBody] create_discount_request data)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _discountServices.createDiscount(data, user_id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }

        /// <summary>
        /// Actualiza un descuento, requiere token
        /// </summary>
        /// <param name="data"></param>
        /// <returns>actualiza un descuento</returns>
        /// <remarks>
        /// Ejemplo de llenado:
        ///
        ///     {
        ///        "id"         : 1 ,
        ///        "description": "promoción perrona de 90% de descuento",
        ///        "percentage" : 90.0,
        ///        "start_date" : "2024-02-15",
        ///        "end_date"   : "2025-08-30",
        ///        "status_id"  : 6
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Significa que agregó la catedral correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpPost]
        [JwtAuthentication]
        [Route("update/discount")]
        public IActionResult updateDiscount([FromBody] update_discount_request data)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _discountServices.updateDiscount(data, user_id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }
    }
}
