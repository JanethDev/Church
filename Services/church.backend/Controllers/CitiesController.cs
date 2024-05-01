using church.backend.Models.catalogue.cathedral;
using church.backend.Models.catalogue.cities;
using church.backend.Models.catalogue.roles;
using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Models.register;
using church.backend.services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace church.backend.services.Controllers
{
    public class CitiesController : Controller
    {
        private readonly CitiesServices _citiesServices;
        public CitiesController(CitiesServices citiesServices)
        {
            _citiesServices = citiesServices;
        }

        /// <summary>
        /// Consulta el catalogo de ciudades, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "city": "ciudad #1",
        ///         },
        ///         {
        ///             "id": 2,
        ///             "city": "ciudad #2",
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("cities")]
        public IActionResult consultCities()
        {
            cities_response response = _citiesServices.consultCities();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Crea una nueva ciudad, requiere token
        /// </summary>
        /// <response code="200">Significa que agregó la ciudad correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("create/city")]
        public IActionResult createCity([FromQuery] string name)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _citiesServices.createCity(name, user_id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }
    }
}
