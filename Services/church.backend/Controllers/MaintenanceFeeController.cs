using church.backend.Models.catalogue.maintenance_fee;
using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace church.backend.services.Controllers
{
    public class MaintenanceFeeController : Controller
    {
        private readonly MaintenanceFeeServices _maintenanceFeeServices;
        public MaintenanceFeeController(MaintenanceFeeServices maintenanceFeeServices)
        {
            _maintenanceFeeServices = maintenanceFeeServices;
        }

        /// <summary>
        /// Consulta los costos de los mantenimientos, requiere token
        /// </summary>
        /// <response code="200">
        ///  Ejemplo de respuesta:
        ///  
        ///     {
        ///         "cost": 1000,
        ///         "shared_cost": 500
        ///     }
        /// </response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("maintenance_fee")]
        public IActionResult consultFederalTax()
        {
            maintenance_fee_response response = _maintenanceFeeServices.consultMaintenanceFee();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.data);
        }

        /// <summary>
        /// Actualiza el costo de los mantenimientos, requiere token
        /// </summary>
        /// <response code="200">Retorta el costo del impuesto federal</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("update/maintenance_fee")]
        public IActionResult updateFederalTax([FromQuery] string cost, string shared_cost)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _maintenanceFeeServices.updateMaintenanceFee(shared_cost,cost,user_id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }

    }
}
