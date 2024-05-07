using church.backend.Models.catalogue.discounts;
using church.backend.Models.catalogue.misa;
using church.backend.Models.enums;
using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace church.backend.services.Controllers
{
    public class MisaController : Controller
    {
        private readonly MisaServices _misaServices;
        public MisaController(MisaServices misaServices)
        {
            _misaServices = misaServices;
        }

        /// <summary>
        /// Consulta el catalogo de misas, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "dayId": 1,
        ///             "day" : "lunes",
        ///             "hour: "08:00 am"
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("misas")]
        public IActionResult consultAll()
        {
            misa_response response = _misaServices.consultAll();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Crea una nueva misa, requiere token
        /// </summary>
        /// <param name="data"></param>
        /// <returns>crea una misa</returns>
        /// <remarks>
        /// Ejemplo de llenado:
        ///
        ///     {
        ///        "day_id": 1,
        ///        "hour" : "10:00 pm"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Significa que agregó la misa correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpPost]
        [JwtAuthentication]
        [Route("create/misa")]
        public IActionResult createDiscount([FromBody] create_misa_request data)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _misaServices.createMisa(data, user_id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }

        /// <summary>
        /// Elimina una misa por ID, requiere token
        /// </summary>
        /// <response code="200">Significa que eliminó la misa correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpPost]
        [JwtAuthentication]
        [Route("delete/misa")]
        public IActionResult updateDiscount([FromQuery] int id)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _misaServices.deleteMisa(id, user_id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }
    }
}
