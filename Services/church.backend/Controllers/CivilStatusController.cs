using church.backend.Models.catalogue.civil_status;
using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace church.backend.services.Controllers
{
    public class CivilStatusController : Controller
    {
        private readonly CivilStatusServices _civilStatusServices;
        public CivilStatusController(CivilStatusServices civilStatusServices)
        {
            _civilStatusServices = civilStatusServices;
        }

        /// <summary>
        /// Consulta el catalogo de estados civiles, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "civilStatus": "casado",
        ///         },
        ///         {
        ///             "id": 2,
        ///             "civilStatus": "soltero",
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("civil_status")]
        public IActionResult consultCivilStatus()
        {
            civil_status_response response = _civilStatusServices.consultCivilStatus();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Crea un nuevo estado civil, requiere token
        /// </summary>
        /// <response code="200">Significa que agregó el estado civil correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("create/civil_status")]
        public IActionResult createCivilStatus([FromQuery] string name)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _civilStatusServices.createCivilStatus(name, user_id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }
    }
}
