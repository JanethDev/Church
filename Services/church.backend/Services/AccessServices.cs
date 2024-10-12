using church.backend.Models.register;
using church.backend.services.DataBase;
using church.backend.services.JsonWebToken;
using church.backend.services.Models;
using church.backend.services.Models.access;
using church.backend.services.Models.register;
using System.Text.RegularExpressions;

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

        public GeneralResponse login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email)) {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un correo"
                };
            }
            if (!ValidateEmail(email))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "El correo enviado no es válido"
                };
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar una contraseña"
                };
            }

            login_response response = _accessDB.login(email, password);
            
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

        public GeneralResponse ChangePassword(int user_id, string password)
        {
            if (string.IsNullOrWhiteSpace(password)) {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es encesario enviar una contraseña"
                };
            }
            if (password.Length<8)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es encesario enviar una contraseña por lo menos 8 caracteres"
                };
            }
            return _accessDB.ChangePassword(user_id, password);
        }

        public async Task<GeneralResponse> SetTempPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un correo"
                };
            }
            if (!ValidateEmail(email))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "El correo enviado no es válido"
                };
            }
            string temp = GetRandomAlphanumericPassword();
            GeneralResponse response = _accessDB.SetTempPassword(email, temp);
            if(response.code != 1)
            {
                return response;
            }

            //await SendTemporalPassword(email, temp);
            return response;
        }

        public GeneralResponse createEmployee(create_employee_request data)
        {
            if (data.role_id < 0) {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un rol para el usaurio"
                };
            }
            if (string.IsNullOrWhiteSpace(data.email))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un correo"
                };
            }
            if (!ValidateEmail(data.email))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "El correo enviado no es válido"
                };
            }
            if (string.IsNullOrWhiteSpace(data.name))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un nombre"
                };
            }
            if (string.IsNullOrWhiteSpace(data.father_last_name))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un apellido"
                };
            }
            if (string.IsNullOrWhiteSpace(data.father_last_name))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un apellido"
                };
            }
            if (string.IsNullOrWhiteSpace(data.phone))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar su número de teléfono"
                };
            }
            data.password = GetRandomAlphanumericPassword();
            return _accessDB.createEmployee(data);
        }

        public GeneralResponse createCustomer(create_customer_request data)
        {
            if (data.catStatesId < 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un estado"
                };
            }
            if (data.catTownsId < 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar una ciudad"
                };
            }
            if (string.IsNullOrWhiteSpace(data.email))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un correo"
                };
            }
            if (!ValidateEmail(data.email))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "El correo enviado no es válido"
                };
            }
            if (string.IsNullOrWhiteSpace(data.name))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un nombre"
                };
            }
            if (string.IsNullOrWhiteSpace(data.father_last_name))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un apellido"
                };
            }
            if (string.IsNullOrWhiteSpace(data.father_last_name))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un apellido"
                };
            }
            if (string.IsNullOrWhiteSpace(data.phone))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar su número de teléfono"
                };
            }
            return _accessDB.createCustomer(data);
        }

        public GeneralResponse updateCustomer(customer_response data, int user_id)
        {
            if (user_id <= 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un id de usaurio"
                };
            }

            return _accessDB.updateCustomer(data, user_id);
        }
        public GeneralResponse createBeneficiaries(BeneficiarieRequest data, int user_id)
        {
            if (data.customerId < 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un id de cliente para los beneficiarios"
                };
            }
            if (user_id < 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un id de usuario"
                };
            }
            data.user_id = user_id;
            return _accessDB.createBeneficiaries(data);
        }

        public GeneralResponse deleteBeneficiaries(int beneficiarieId, int user_id)
        {
            if (beneficiarieId < 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un id de beneficiario"
                };
            }
            if (user_id < 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un id de usuario"
                };
            }
            return _accessDB.deleteBeneficiaries(beneficiarieId,user_id);
        }

        public GeneralResponse updateBeneficiaries(Beneficiarie data, int user_id)
        {
            if (user_id < 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un id de usuario"
                };
            }
            return _accessDB.updateBeneficiaries(data,user_id);
        }

        public BeneficiarieResponse consultBeneficiaries(int customerId)
        {
            if (customerId < 0)
            {
                return new BeneficiarieResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un id de cliente de los beneficiarios"
                };
            }
            return _accessDB.consultBeneficiaries(customerId);
        }

        public list_customer SearchCustomer(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new list_customer()
                {
                    code = -1,
                    message = "Es necesario enviar un valor de busqueda"
                };
            }

            return _accessDB.SearchCustomer(value);
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

        public static bool ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (Regex.IsMatch(email, pattern))
            {
                if (!email.Contains(".."))
                {
                    if (!email.StartsWith(".") && !email.EndsWith("."))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
