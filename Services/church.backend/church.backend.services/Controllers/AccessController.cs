using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Models.Client;
using church.backend.services.Services;
using Microsoft.AspNetCore.Mvc;
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
        [Route("client/get")]
        public GeneralResponse GetClient([FromQuery] string email, string password)
        {
            return _AccessServices.GetClient(email, password);
        }

        [HttpPost]
        [Route("client/save")]
        public async Task<GetClientResponse> SaveClient([FromBody] SaveClientRequest request)
        {
            return await _AccessServices.SaveClient(request);
        }

        [HttpPut]
        [Route("client/validate/")]
        public GeneralResponse ValidateAccount([FromBody] ValidateAccountRequest request)
        {
            return _AccessServices.ValidateAccount(request);
        }

        [HttpGet]
        [Route("client/sendCode/{email}/{brand}")]//XIGA services
        public async Task<GeneralResponse> ReSendEmail([FromRoute] string email, [FromRoute] int brand)
        {
            return await _AccessServices.ReSendEmail(email, brand);
        }

        [HttpGet]
        [JwtAuthentication]
        [Route("client/getPhoto")]
        public GetClientResponse GetClientById()
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int clientId = int.Parse(claims?["clientId"] ?? "0");
            return _AccessServices.GetClientById(clientId);
        }

        [HttpPost]
        [JwtAuthentication]
        [Route("client/updatePhoto")]
        public GeneralResponse UpdateClientPhoto([FromBody] UpdatePhotoRequest request)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int clientId = int.Parse(claims?["clientId"] ?? "0");
            return _AccessServices.UpdateClientPhoto(clientId, request.photo);
        }

        [HttpPost]
        [JwtAuthentication]
        [Route("client/changePassword")]
        public GeneralResponse ChangePassword([FromQuery] string password)
        {
            var claims = HttpContext.Items["Claims"] as IDictionary<string, string>;
            int clientId = int.Parse(claims?["clientId"] ?? "0");
            return _AccessServices.ChangePassword(clientId,password);
        }

        [HttpGet]
        [Route("client/temporalPassword")]
        public async Task<GeneralResponse> SetTempPassword([FromQuery] string email) //Change, must send html view via email
        {
            return await _AccessServices.SetTempPassword(email);
        }

        [HttpPost]
        [JwtAuthentication]
        [Route("client/updateProfile")]
        public GetClientResponse UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            return _AccessServices.UpdateProfile(request);
        }
    }
}
