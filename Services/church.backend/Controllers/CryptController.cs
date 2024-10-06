using church.backend.Models.catalogue.crypts;
using church.backend.Models.catalogue.discounts;
using church.backend.Models.enums;
using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace church.backend.services.Controllers
{
    public class CryptController : Controller
    {
        private readonly CryptServices _cryptServices;
        public CryptController(CryptServices cryptServices)
        {
            _cryptServices = cryptServices;
        }

        /// <summary>
        /// Consulta las criptas de una zona, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "full_position": "ejemplo posicion",
        ///             "zone" : "A2",
        ///             "position: "C4",
        ///             "is_shared: false,
        ///             "places_shared": 0,
        ///             "price" : 4000,
        ///             "price_shared": 0
        ///             "status_id": 5,
        ///             "status": "expirado"
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("crypts/byzone")]
        public IActionResult consultByZone([FromQuery] string zone)
        {
            crypt_responseV2 response = _cryptServices.consultByZone(zone);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Consulta una cripta por id, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "full_position": "ejemplo posicion",
        ///             "zone" : "A2",
        ///             "position: "C4",
        ///             "is_shared: false,
        ///             "places_shared": 0,
        ///             "price" : 4000,
        ///             "price_shared": 0
        ///             "status_id": 5,
        ///             "status": "expirado"
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("crypts/byid")]
        public IActionResult consultById([FromQuery] string id)
        {
            crypt_response response = _cryptServices.consultById(id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Consulta el listado de zonas disponibles, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         "zona 1",
        ///         "zona 2",
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("crypts/zones")]
        public IActionResult consultZones()
        {
            crypt_zones response = _cryptServices.consultZones();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Actualiza una cripta, requiere token
        /// </summary>
        /// <param name="data"></param>
        /// <returns>actualiza un descuento</returns>
        /// <remarks>
        /// Ejemplo de llenado:
        ///
        ///     {
        ///        "id"            : 1 ,
        ///        "status_id"     : 2,
        ///        "is_shared"     : true,
        ///        "places_shared" : 3,
        ///        "price"         : 0,
        ///        "price_shared"  : 50600
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Significa que agregó la catedral correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpPost]
        [JwtAuthentication]
        [Route("update/crypt")]
        public IActionResult updateCrypt([FromBody] update_crypt_request data)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _cryptServices.updateCrypt(data, user_id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }

        /*[HttpGet]
        [Route("add")]
        public string add()
        {
            _cryptServices.addc();
            return "listo";
        }*/
    }
}
