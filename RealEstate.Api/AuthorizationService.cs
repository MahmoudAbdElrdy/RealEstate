
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Api;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace RealEstate.Service.Classes
{
    public class AuthorizationService : IAuthorizationFilter
    {
        private const string _SECRET_KEY = "$m@rt@Ma7m0ud#Key!Gm2512&Aug";
        System.Security.Claims.ClaimsPrincipal _principal;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }
            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.Contains("Bearer"))
            {
                authHeader = authHeader.Replace("Bearer ", "").TrimStart();
   
            }
            try
            {
                var result = GetUserIdFromToken(authHeader);
                    if(result != null)
                {
                context.Result = result; }
                    if(result is ObjectResult )
                {
                    return;
                }
                var claims = (_principal.Claims).ToList();
                Settings.Id  = Convert.ToInt64(claims.Where(c => c.Type == "Id").FirstOrDefault().Value);
                Settings.Department = claims.Where(c => c.Type == "Department").FirstOrDefault().Value;
                Settings.Name = claims.Where(c => c.Type == "Department").FirstOrDefault().Value;
                context.HttpContext.User = _principal;
            }
            catch (Exception ex)
            {
                context.Result = new ForbidResult(ex.Message);
            }



        }
 
        public IActionResult GetUserIdFromToken(string token)
        {
            try
            {


                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_SECRET_KEY)),
                    ValidateLifetime = true //here we are saying that we don't care about the token's expiration date
                };
          
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");
                _principal = principal ;
                return null; //new StatusCodeResult(200);

            }
            catch (SecurityTokenExpiredException ex)
            {
                ObjectResult obj = new ObjectResult(new SecurityTokenExpiredException(ex.Message));
                obj.StatusCode = 498;
                return obj;
            
            }
            catch(SecurityTokenValidationException exv)
            {
                throw new SecurityTokenExpiredException(exv.Message);
            }
            catch (Exception gex)
            {
                if (token == null)
                {
                    ObjectResult obj = new ObjectResult("Token Required");
                    obj.StatusCode = 499;
                    return obj;
                }
                throw new NotSupportedException(gex.Message);
            }
        }
    }

    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute()
            : base(typeof(AuthorizationService))
        {
    
        }

    }
}
