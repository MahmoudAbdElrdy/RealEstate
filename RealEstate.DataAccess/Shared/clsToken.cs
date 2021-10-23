using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RealEstate.DataAccess.Shared
{
    [Serializable]
    public static class clsToken
    {
        private const string _SECRET_KEY = "$m@rt@Ma7m0ud#Key!Gm2512&Aug";
        public static readonly SymmetricSecurityKey _SIGNING_KEY = new
            SymmetricSecurityKey(Encoding.UTF8.GetBytes(_SECRET_KEY));

        public static string GenerateToken(string Id, string Department, string Name)
        {
            Claim id = new Claim("Id", Id??"");
            Claim name = new Claim("Name", Name??"");
            Claim department = new Claim("Department", Department??"");
            var claims = new[] {
                id,
                name,department,
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddYears(2)).DateTime,
                signingCredentials: new SigningCredentials(_SIGNING_KEY,
                                                    SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        internal static string GetKey()
        {
            return "";
        }

        public static string GetUserIdFromToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_SECRET_KEY)),
                    ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");
                var claims = (principal.Claims).ToList();
                string userId = claims.Where(c => c.Type == "UserId").FirstOrDefault().Value;

                return userId;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenNoExpirationException(ex.Message);

            }
        }



    }
}
