using church.backend.services.DataBase;
using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Models.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace church.backend.services.Services
{
    public class AccessServices
    {
        private readonly accessDB _accessDB;
        private readonly IConfiguration _configuration;
        private readonly JwtService _jwtService;
        private readonly IHttpClientFactory _httpClientFactory;
        public AccessServices(IConfiguration configuration, accessDB accessDB, IHttpClientFactory httpClientFactory, JwtService jwtService)
        {
            _configuration = configuration;
            _accessDB = accessDB;
            _httpClientFactory = httpClientFactory;
            _jwtService = jwtService;
        }

        public GeneralResponse ValidateAccount(ValidateAccountRequest request)
        {
            return _accessDB.ValidateAccount(request);
        }

        public async Task<GeneralResponse> ReSendEmail(string email, int brand)
        {
            GeneralResponse getCode = _accessDB.GetEmailValidationCode(email, brand);

            if(getCode.code != 1)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = getCode.message
                };
            }

            await SendValidateEmail(email, getCode.message);
            return new GeneralResponse()
            {
                code =1,
                message = "success"
            };
        }

        public GeneralResponse GetClient(string email, string password)
        {
            //GetClientResponse response = _accessDB.GetClient(email, new CryptoService().Encrypt(password), Encrypt(password, true), password);
            GetClientResponse response = new GetClientResponse();
            if (response.code != 1)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = response.message
                };
            }

            return new GeneralResponse() {
                code = 1,
                message = _jwtService.GenerateToken(response.data)
            };
        }

        public async Task<GetClientResponse> SaveClient(SaveClientRequest request)
        {
            string code = new Random().Next(1000, 10000).ToString();
            //request.password = new CryptoService().Encrypt(request.password);
            GeneralResponse response = _accessDB.SaveClient(request, code);
            if (response.code != 1) {
                return new GetClientResponse()
                {
                    code = -1,
                    message = response.message
                };
            }
            await SendValidateEmail(request.email, code);
            return new GetClientResponse() { 
                code = 1,
                message = "success",
                data = new AppClient()
                {
                    clientId = int.Parse(response.message),
                    name = request.name,
                    email = request.email,
                    phoneNumber = request.phoneNumber,
                    rfc = request.rfc,
                    isMoral = request.isMoral,
                    city = request.city,
                    state = request.state,
                    postal_code = request.postal_code,
                }
            };
        }

        public GetClientResponse GetClientById(int clientId)
        {
            return _accessDB.GetClientById(clientId);
        }

        public GeneralResponse UpdateClientPhoto(int clientId, string photo)
        {
            byte[] bytesPhoto = Convert.FromBase64String(photo);
            return _accessDB.UpdateClientPhoto(clientId, bytesPhoto);
        }

        public GeneralResponse ChangePassword(int clientId, string password)
        {
            return _accessDB.ChangePassword(clientId,password);
        }

        public async Task<GeneralResponse> SetTempPassword(string email)
        {
            string temp = GetRandomAlphanumericPassword();
            GeneralResponse response = _accessDB.SetTempPassword(email, temp);
            if(response.code != 1)
            {
                return response;
            }

            await SendTemporalPassword(email, temp);
            return response;
        }

        public GetClientResponse UpdateProfile(UpdateProfileRequest request)
        {
            return _accessDB.UpdateProfile(request);
        }

        private async Task SendValidateEmail(string email, string code)
        {
            var httpClient = _httpClientFactory.CreateClient();
            string htmlBody;
            using (var reader = new StreamReader("Templates/validateTemplate.html"))
            {
                htmlBody = reader.ReadToEnd();
            }

            htmlBody = htmlBody.Replace("{Code}", code);
            /*var content = new SendgridRequest
            {
                personalizations = new List<Personalization>
                {
                    new Personalization
                    {
                        to = new List<To> { new To { email = email } },
                        subject = "Validación de cuenta"
                    }
                },
                content = new List<Content> { new Content { type = "text/html", value = htmlBody } },
                from = new From { email = _configuration["SendGrid:mail"], name = "EFT Móvil" }
            };*/
            var request = new HttpRequestMessage(HttpMethod.Post, _configuration["SendGrid:url"]);
            //request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", $"Bearer {_configuration["SendGrid:key"]}");
            await httpClient.SendAsync(request);
        }
       
        private async Task SendTemporalPassword(string email, string password)
        {
            var httpClient = _httpClientFactory.CreateClient();
            string htmlBody;
            using (var reader = new System.IO.StreamReader("Templates/password.html"))
            {
                htmlBody = reader.ReadToEnd();
            }

            htmlBody = htmlBody.Replace("{Code}", password);
            /*var content = new SendgridRequest
            {
                personalizations = new List<Personalization>
                {
                    new Personalization
                    {
                        to = new List<To> { new To { email = email } },
                        subject = "Recuperación de contraseña"
                    }
                },
                content = new List<Content> { new Content { type = "text/html", value = htmlBody } },
                from = new From { email = _configuration["SendGrid:mail"], name = "EFT Móvil" }
            };*/
            var request = new HttpRequestMessage(HttpMethod.Post, _configuration["SendGrid:url"]);
            //request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", $"Bearer {_configuration["SendGrid:key"]}");
            await httpClient.SendAsync(request);
        }

        private string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            string key = "mx.com.gasmart.gasmartmovil";
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        private string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            string key = "mx.com.gasmart.gasmartmovil";
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        private string GetRandomAlphanumericPassword()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHJKMNPQRSTUVWXYZ23456789"; // NO [i, l, 1, 0, o] por confusión en los clientes
            var stringChars = new char[8];
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new String(stringChars);
        }
    }
}
