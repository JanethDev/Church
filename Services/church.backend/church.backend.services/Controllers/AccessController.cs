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
    public class AccessController : Controller
    {
        private readonly AccessServices _AccessServices;
        public AccessController(AccessServices AccessServices)
        {
            _AccessServices = AccessServices;
        }

        [HttpGet]
        [Route("access/login")]
        public IActionResult makeLogin([FromQuery] string email, string password)
        {     
            GeneralResponse client = _AccessServices.login(email, password);
            if (client.code != 1) {
                return StatusCode(StatusCodes.Status400BadRequest, client.message);
            }
            return Ok(client.message);
        }

        [HttpGet]
        [JwtAuthentication]
        [Route("access/update/password")]
        public IActionResult ChangePassword([FromQuery] string password)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _AccessServices.ChangePassword(user_id, password);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }

        [HttpGet]
        [Route("access/temporal/password")]
        public async Task<IActionResult> SetTempPassword([FromQuery] string email)
        {
            GeneralResponse response = await _AccessServices.SetTempPassword(email);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }

        [HttpPost]
        [Route("access/create/employee")]
        public IActionResult createEmployee([FromBody] create_employee_request data)
        {
            GeneralResponse client = _AccessServices.createEmployee(data);
            if (client.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, client.message);
            }
            return Ok(client.message);
        }
    }
}
