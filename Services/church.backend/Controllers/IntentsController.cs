
using church.backend.Models.catalogue.civil_status;
using church.backend.Models.catalogue.intents;
using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace church.backend.services.Controllers
{
    public class IntentsController : Controller
    {
        private readonly IntentsServices _intentsServices;
        public IntentsController(IntentsServices intentsServices)
        {
            _intentsServices = intentsServices;
        }

        /// <summary>
        /// Consulta el catalogo de intenciones para misa, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "intent": "aniversario de morido",
        ///         },
        ///         {
        ///             "id": 2,
        ///             "intent": "brujeria pa la suerte",
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("intents")]
        public IActionResult consultCivilStatus()
        {
            intents_response response = _intentsServices.consultIntents();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Crea una nueva intención para misa, requiere token
        /// </summary>
        /// <response code="200">Significa que agregó la intención correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("create/intent")]
        public IActionResult createCivilStatus([FromQuery] string name)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _intentsServices.createIntent(name, user_id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }
    }
}
