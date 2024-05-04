using church.backend.Models.catalogue.days;
using church.backend.services.JsonWebToken;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace church.backend.services.Controllers
{
    public class DaysController : Controller
    {
        private readonly DaysServices _daysServices;
        public DaysController(DaysServices daysServices)
        {
            _daysServices = daysServices;
        }

        /// <summary>
        /// Consulta el catalogo de dias de la semana, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "day": "lunes",
        ///         },
        ///         {
        ///             "id": 2,
        ///             "day": "martes",
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("days")]
        public IActionResult consultDays()
        {
            days_response response = _daysServices.consultDays();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

    }
}
