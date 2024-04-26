using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Models.register;
using church.backend.services.Models.roles;
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
