using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace church.backend.services.JsonWebToken
{
    public class JwtAuthenticationAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _secretKey;

        public JwtAuthenticationAttribute()
        {
            _secretKey = "CHURCHmnjhbvgfcrdexcfrvgbhnjmhgvfrcvgbhnyv234dfg";
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var token = authorizationHeader.First().Split(" ").Last();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
                context.HttpContext.Items["Claims"] = claims;

                await next();
            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
