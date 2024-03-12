using Church.Business;
using Church.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace Church.API.Filters
{
    public class HMACAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private readonly UInt64 requestMaxAgeInSeconds = 300;  //5 mins
        private readonly string authenticationScheme = "amx";

        public HMACAuthenticationAttribute()
        {

        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var req = context.Request;

            if (req.Headers.Authorization != null && authenticationScheme.Equals(req.Headers.Authorization.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                var rawAuthzHeader = req.Headers.Authorization.Parameter;

                var autherizationHeaderArray = GetAutherizationHeaderValues(rawAuthzHeader);

                if (autherizationHeaderArray != null)
                {
                    if (autherizationHeaderArray.Length == 4)
                    {
                        var APPId = autherizationHeaderArray[0];
                        var incomingBase64Signature = autherizationHeaderArray[1];
                        var nonce = autherizationHeaderArray[2];
                        var requestTimeStamp = autherizationHeaderArray[3];

                        var isValid = isValidRequest(req, APPId, incomingBase64Signature, nonce, requestTimeStamp);

                        if (isValid.Result)
                        {
                            var currentPrincipal = new GenericPrincipal(new GenericIdentity(APPId), null);
                            context.Principal = currentPrincipal;
                        }
                        else
                        {
                            context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                        }
                    }
                 
                    
                }
                else
                {

                    context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                }
            }
            else
            {
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
            }

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new ResultWithChallenge(context.Result);
            return Task.FromResult(0);
        }

        public bool AllowMultiple
        {
            get { return false; }
        }

        private string[] GetAutherizationHeaderValues(string rawAuthzHeader)
        {

            var credArray = rawAuthzHeader.Split(':');

            return credArray;

        }

        private async Task<bool> isValidRequest(HttpRequestMessage req, string APPId, string incomingBase64Signature, string nonce, string requestTimeStamp)
        {
            string requestContentBase64String = "";
            string requestUri = HttpUtility.UrlEncode(req.RequestUri.AbsoluteUri.ToLower());
            string requestHttpMethod = req.Method.Method;
            if (requestUri.Contains("localhost"))
                return true;
          //  tblAllowedApps Model = new tblAllowedApps() { AppId = WebConfigHelper.ZetaAppId, AppKey = WebConfigHelper.ZetaAppKey };



            AllowedAppsB AllowedAppsB = new AllowedAppsB();
             
            tblAllowedApps Model = AllowedAppsB.ValidAppId(APPId);

            if (Model?.AllowedAppID== 0)
            {
                return false;
            }

            var sharedKey = Model.AppKey;

            byte[] hash = await ComputeHash(req.Content);

            if (hash != null)
            {
                requestContentBase64String = Convert.ToBase64String(hash);
            }

            string data = String.Format("{0}{1}{2}{3}{4}{5}", APPId, requestHttpMethod, requestUri, requestTimeStamp, nonce, requestContentBase64String);

            var secretKeyBytes = Convert.FromBase64String(sharedKey);

            byte[] signature = Encoding.UTF8.GetBytes(data);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyBytes))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);


                var dd = Convert.ToBase64String(signatureBytes);

                return (incomingBase64Signature.Equals(Convert.ToBase64String(signatureBytes), StringComparison.Ordinal));
            }

        }

        private static async Task<byte[]> ComputeHash(HttpContent httpContent)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = null;
                var content = await httpContent.ReadAsByteArrayAsync();
                if (content.Length != 0)
                {
                    hash = md5.ComputeHash(content);
                }
                return hash;
            }
        }
    }
}