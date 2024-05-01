using church.backend.Models.catalogue.cathedral;
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
    public class CathedralController : Controller
    {
        private readonly CathedralServices _cathedralServices;
        public CathedralController(CathedralServices cathedralServices)
        {
            _cathedralServices = cathedralServices;
        }

        /// <summary>
        /// Consulta el catalogo de catedrales, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "name": "catedral #1",
        ///             "city_id" : 1,
        ///             "city: "ciudad #1"
        ///         },
        ///         {
        ///             "id": 2,
        ///             "name": "catedral #2",
        ///             "city_id" : 1,
        ///             "city: "ciudad #1"
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("cathedrals")]
        public IActionResult consultCathedrals()
        {
            cathedral_response response = _cathedralServices.consultCathedrals();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Crea una nueva catedral, requiere token
        /// </summary>
        /// <param name="data"></param>
        /// <returns>crea una nueva catedral</returns>
        /// <remarks>
        /// Ejemplo de llenado:
        ///
        ///     {
        ///        "name": "Catedral nueva",
        ///        "city_id" : 2
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Significa que agregó la catedral correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpPost]
        [JwtAuthentication]
        [Route("create/cathedrals")]
        public IActionResult createCathedrals([FromBody] create_cathedral_request data)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _cathedralServices.createCathedrals(data, user_id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }
    }
}
