using church.backend.Models.catalogue.states_towns;
using church.backend.services.JsonWebToken;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace church.backend.services.Controllers
{
    public class StateTownController : Controller
    {
        private readonly StateTownServices _stateTownServices;
        public StateTownController(StateTownServices stateTownServices)
        {
            _stateTownServices = stateTownServices;
        }

        /// <summary>
        /// Consulta el catalogo de dias de la semana, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         "state":{
        ///             "id": 1,
        ///             "state_name": "baja california",
        ///         },
        ///         "towns_list":[
        ///             {
        ///                 "id": 1,
        ///                 "town_name": "tijuana",
        ///             }
        ///         ]
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("state_towns")]
        public IActionResult consultDays()
        {
            state_town_response response = _stateTownServices.consultStateTowns();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

    }
}
