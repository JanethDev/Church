using church.backend.services.Models.register;
using church.backend.services.Models;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;
using church.backend.Models.register;

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
    }
}
