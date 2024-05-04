using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace church.backend.services.Controllers
{
    public class FederalTaxController : Controller
    {
        private readonly FederalTaxServices _federalTaxServices;
        public FederalTaxController(FederalTaxServices federalTaxServices)
        {
            _federalTaxServices = federalTaxServices;
        }

        /// <summary>
        /// Consulta el impuesto federal configurado, requiere token
        /// </summary>
        /// <response code="200">Retorta el costo del impuesto federal</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("federal_tax")]
        public IActionResult consultFederalTax()
        {
            GeneralResponse response = _federalTaxServices.consultFederalTax();
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }

        /// <summary>
        /// Actualiza el impuesto federal, requiere token
        /// </summary>
        /// <response code="200">Retorta el costo del impuesto federal</response>
        /// <response code="400">Retorna algun error del usuario</response>
        [HttpGet]
        [JwtAuthentication]
        [Route("update/federal_tax")]
        public IActionResult updateFederalTax([FromQuery] string cost)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int user_id = int.Parse(claims?["user_id"] ?? "0");
            GeneralResponse response = _federalTaxServices.updateFederalTax(cost,user_id);
            if (response.code != 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.message);
            }
            return Ok(response.message);
        }

    }
}
