using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Models.register;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace church.backend.services.Controllers
{
    public class AccessController : Controller
    {
        private readonly AccessServices _AccessServices;
        public AccessController(AccessServices AccessServices)
        {
            _AccessServices = AccessServices;
        }

        /// <summary>
        /// Inicio de sesión para usuario
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>garantiza acceso al usuario</returns>
        /// <response code="200">Devuelve Token de acceso</response>
        /// <response code="400">Retorna algun error del usuario</response>
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

        /// <summary>
        /// Cambia la contraseña del usuario, requiere token
        /// </summary>
        /// <param name="password"></param>
        /// <returns>cambia la contraseña del usuario</returns>
        /// <response code="200">Actualiza la contraseña</response>
        /// <response code="400">Retorna algun error del usuario</response>
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

        /// <summary>
        /// Para recuperación la contraseña olvidada
        /// </summary>
        /// <param name="email"></param>
        /// <returns>cambia la contraseña del usuario</returns>
        /// <response code="200">Actualiza la contraseña</response>
        /// <response code="400">Retorna algun error del usuario</response>
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

        /// <summary>
        /// Creación de usuarios empleados, requiere token
        /// </summary>
        /// <param name="data"></param>
        /// <returns>crea un nuevo usuario empleado</returns>
        /// <remarks>
        /// Ejemplo de llenado:
        ///
        ///     {
        ///        "email": "ejemplo@correo.com",
        ///        "name": "Juanito",
        ///        "father_last_name": "Perez",
        ///        "mother_last_name": "Lopez",
        ///        "phone" : "6631234567",
        ///        "role_id" : 2
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Significa que agregó al usuario correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
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
