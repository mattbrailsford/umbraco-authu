using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Umbraco.OAuth.Interfaces;

namespace Umbraco.OAuth.Services
{
    public class JwtTokenService : IOAuthTokenService
    {
        private string _secret;

        public JwtTokenService(string secret)
        {
            _secret = secret;
        }

        public string GenerateToken(ClaimsIdentity identity, int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(this._secret); 
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Lifetime = new Lifetime(now, now.AddMinutes(expireMinutes)),
                SigningCredentials = new SigningCredentials(new InMemorySymmetricSecurityKey(symmetricKey), 
                    SecurityAlgorithms.HmacSha256Signature,
                    SecurityAlgorithms.Sha256Digest)
            };
            
            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        public ClaimsPrincipal ReadToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(this._secret);
                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new InMemorySymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;

                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
