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
    public class RolesController : Controller
    {
        private readonly RolesServices _RolesServices;
        public RolesController(RolesServices rolesServices)
        {
            _RolesServices = rolesServices;
        }

        /// <summary>
        /// Consulta el catalogo de roles, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "role": "role #1",
        ///         },
        ///         {
        ///             "id": 2,
        ///             "role": "role #2",
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("roles")]
        public IActionResult consultRoles()
        {
            roles_response response = _RolesServices.consultRoles();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

    }
}
