using church.backend.Models.enums;
using church.backend.Models.misa_intents;
using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace church.backend.services.Controllers
{
    public class MisaIntentsController : Controller
    {
        private readonly MisaIntentsServices _misaIntentsServices;
        public MisaIntentsController(MisaIntentsServices misaIntentsServices)
        {
            _misaIntentsServices = misaIntentsServices;
        }

        /// <summary>
        /// Consulta todas las solicitudes de intenciones de misa, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "intent_id": 1,
        ///             "intent": "velorio",
        ///             "misa_id" : 1,
        ///             "misa_day" : "lunes",
        ///             "misa_hour" : "07:00 am",
        ///             "date: "2023-01-01",
        ///             "mention_person":"juan",
        ///             "applicant:"la famila de juan",
        ///             "phone":"6641234567",
        ///             "donation":30.5,
        ///             "exchange_rate":15.6,
        ///             "decription":"descripción para la misa", 
        ///             "status_id": 1005,
        ///             "status": "pendiente"
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("misa_intents/all")]
        public IActionResult consultAll()
        {
            misa_intents_response response = _misaIntentsServices.consultAll();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Consulta las solicitudes pendientes de intenciones de misa, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "intent_id": 1,
        ///             "intent": "velorio",
        ///             "misa_id" : 1,
        ///             "misa_day" : "lunes",
        ///             "misa_hour" : "07:00 am",
        ///             "date: "2023-01-01",
        ///             "mention_person":"juan",
        ///             "applicant:"la famila de juan",
        ///             "phone":"6641234567",
        ///             "donation":30.5,
        ///             "exchange_rate":15.6,
        ///             "decription":"descripción para la misa", 
        ///             "status_id": 1005,
        ///             "status": "pendiente"
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("misa_intents/pending")]
        public IActionResult consultActive()
        {
            misa_intents_response response = _misaIntentsServices.consultByStatus(misa_intents_status.Pendiente);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Consulta las solicitudes atendidas de intenciones de misa, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "intent_id": 1,
        ///             "intent": "velorio",
        ///             "misa_id" : 1,
        ///             "misa_day" : "lunes",
        ///             "misa_hour" : "07:00 am",
        ///             "date: "2023-01-01",
        ///             "mention_person":"juan",
        ///             "applicant:"la famila de juan",
        ///             "phone":"6641234567",
        ///             "donation":30.5,
        ///             "exchange_rate":15.6,
        ///             "decription":"descripción para la misa", 
        ///             "status_id": 1006,
        ///             "status": "atendido"
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("misa_intents/served")]
        public IActionResult consultExpired()
        {
            misa_intents_response response = _misaIntentsServices.consultByStatus(misa_intents_status.Atendido);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Consulta las solicitudes canceladas de intenciones de misa, requiere token
        /// </summary>
        /// <response code="200">
        /// Ejemplo de respuesta:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "intent_id": 1,
        ///             "intent": "velorio",
        ///             "misa_id" : 1,
        ///             "misa_day" : "lunes",
        ///             "misa_hour" : "07:00 am",
        ///             "date: "2023-01-01",
        ///             "mention_person":"juan",
        ///             "applicant:"la famila de juan",
        ///             "phone":"6641234567",
        ///             "donation":30.5,
        ///             "exchange_rate":15.6,
        ///             "decription":"descripción para la misa", 
        ///             "status_id": 1007,
        ///             "status": "canceladas"
        ///         }
        ///     ]
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("misa_intents/cancel")]
        public IActionResult consultDeactive()
        {
            misa_intents_response response = _misaIntentsServices.consultByStatus(misa_intents_status.Cancelado);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Crea una nueva intención de misa, requiere token
        /// </summary>
        /// <param name="data"></param>
        /// <returns>crea un nuevo descuento</returns>
        /// <remarks>
        /// Ejemplo de llenado:
        ///
        ///     {
        ///        "intent_id": 1,
        ///        "misa_id" : 1,
        ///        "date" : "2024-02-15",
        ///        "mention_person"   : "juanito",
        ///        "applicant"   : "familia juanito",
        ///        "phone"   : "6641234567",
        ///        "donation"   : 300.50,
        ///        "exchange_rate"   : 12.5,
        ///        "decription"   : "descripcion de la misa"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Significa que agregó la intención de misa correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpPost]
        [JwtAuthentication]
        [Route("create/misa_intents")]
        public IActionResult createDiscount([FromBody] create_misa_intent_request data)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _misaIntentsServices.createMisaIntent(data, user_id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }

        /// <summary>
        /// Crea una nueva intención de misa, requiere token
        /// </summary>
        /// <param name="data"></param>
        /// <returns>crea un nuevo descuento</returns>
        /// <remarks>
        /// Ejemplo de llenado:
        ///
        ///     {
        ///        "id":1 
        ///        "intent_id": 1,
        ///        "misa_id" : 1,
        ///        "date" : "2024-02-15",
        ///        "mention_person"   : "juanito",
        ///        "applicant"   : "familia juanito",
        ///        "phone"   : "6641234567",
        ///        "donation"   : 300.50,
        ///        "exchange_rate"   : 12.5,
        ///        "decription"   : "descripcion de la misa",
        ///        "status_id" : 1007
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Significa que agregó la intención de misa correctamente</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpPost]
        [JwtAuthentication]
        [Route("update/misa_intents")]
        public IActionResult updateDiscount([FromBody] update_misa_intent_request data)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _misaIntentsServices.updateMisaIntent(data, user_id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }
    }
}
