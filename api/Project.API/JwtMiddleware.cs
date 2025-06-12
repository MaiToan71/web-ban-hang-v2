using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project.Caching.Interfaces;
using Project.ViewModels;
using static Project.Utilities.Constants.SystemConstants;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace Project.API
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IConfiguration _configuration;
        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, ICachingExtension cachingExtension, IConfiguration configuration)
        {
            _next = next;
            _appSettings = appSettings.Value;
            _ICachingExtension = cachingExtension;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                attachPartnerToContext(context, token);
            }

            await _next(context);
        }


        private void attachPartnerToContext(HttpContext context, string token)
        {
            var postResult = new PostResult();
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWTs:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWTs:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWTs:Key"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // To immediately reject the access token
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                // attach user to context on successful jwt validation



                if (!string.IsNullOrEmpty(userId))
                {
                    context.Items["UserIdAuthorized"] = userId;

                }



            }
            catch (Exception e)
            {
             //   postResult.Status = false;
              //  postResult.Message = e.Message;
                // do nothing if jwt validation fails

            }
        }
    }
}
