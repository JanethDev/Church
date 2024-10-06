using church.backend.services.Models.register;
using church.backend.services.Models;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;
using church.backend.Models.register;
using church.backend.services.JsonWebToken;

namespace church.backend.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AccessServices _AccessServices;
        public CustomerController(AccessServices AccessServices)
        {
            _AccessServices = AccessServices;
        }

        /// <summary>
        /// Creación de clientes, requiere token
        /// </summary>
        /// <param name="data"></param>
        /// <returns>crea un nuevo cliente</returns>
        /// <remarks>
        /// Ejemplo de llenado:
        ///
        ///     {
        ///        "email": "ejemplo@correo.com",
        ///        "name": "Juanito",
        ///        "father_last_name": "Perez",
        ///        "mother_last_name": "Lopez",
        ///        "phone" : "6631234567",
        ///        "rfc" : "GODA08492DFHD",
        ///        "zip_code" : 24523,
        ///        "address" : "calle bonita #35",
        ///        "catStatesId" : 1,
        ///        "catTownsId" : 1
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Significa que agregó al cliente correctamente</response>
        /// <response code="400">Retorna algun error del cliente</response>
        [HttpPost]
        [JwtAuthentication]
        [Route("customer/create")]
        public IActionResult createCustomer([FromBody] create_customer_request data)
        {
            GeneralResponse customer = _AccessServices.createCustomer(data);
            if (customer.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, customer.message);
            }
            return Ok(customer.message);
        }

        /// <summary>
        /// Busqueda de clientes por nombre,correo o teléfono, requiere token
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Consulta todos los clientes con que se encuentre coincidencia</returns>
        /// <response code="200">Devuelve Listado de clientes</response>
        /// <response code="400">Retorna algun error de consulta</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("customer/search")]
        public IActionResult makeLogin([FromQuery] string value)
        {
            list_customer client = _AccessServices.SearchCustomer(value);
            if (client.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, client.message);
            }
            return Ok(client.customers);
        }

        /// <summary>
        /// Creación de beneficiarios de clientes, requiere token
        /// </summary>
        /// <param name="data"></param>
        /// <returns>crea un nuevo beneficiario</returns>
        /// <remarks>
        /// Ejemplo de llenado:
        ///
        ///     {
        ///        "customerId": 1,
        ///        "name": "Juanito",
        ///        "lastname": "Perez",
        ///        "phone" : "6631234567",
        ///        "birthdate" : "1990-10-25",
        ///        "relationship" : "hermano"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Significa que agregó al beneficiario correctamente</response>
        /// <response code="400">Retorna algun error del beneficiario</response>
        [HttpPost]
        [JwtAuthentication]
        [Route("customer/create/beneficiarie")]
        public IActionResult createBeneficiarie([FromBody] BeneficiarieRequest data)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse customer = _AccessServices.createBeneficiaries(data,user_id);
            if (customer.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, customer.message);
            }
            return Ok(customer.message);
        }

        // <summary>
        /// elmina un beneficiario de un cliente, requiere token
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Elmina un beneficiario de un cliente</returns>
        /// <response code="200">Devuelve mensaje de exito</response>
        /// <response code="400">Retorna algun error</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("customer/delete/beneficiarie")]
        public IActionResult deleteBeneficiaries([FromQuery] int beneficiarieId)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse client = _AccessServices.deleteBeneficiaries(beneficiarieId,user_id);
            if (client.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, client.message);
            }
            return Ok(client.message);
        }

        // <summary>
        /// elmina un beneficiario de un cliente, requiere token
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Consulta todos los clientes con que se encuentre coincidencia</returns>
        /// <response code="200">Devuelve Listado de clientes</response>
        /// <response code="400">Retorna algun error de consulta</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("customer/consult/beneficiaries")]
        public IActionResult consultBeneficiaries([FromQuery] int customerId)
        {
            BeneficiarieResponse client = _AccessServices.consultBeneficiaries(customerId);
            if (client.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, client.message);
            }
            return Ok(client.data);
        }
    }
}
