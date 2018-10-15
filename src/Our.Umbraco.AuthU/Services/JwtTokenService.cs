using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Our.Umbraco.AuthU.Interfaces;

namespace Our.Umbraco.AuthU.Services
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
				IssuedAt = now,
				NotBefore = now,
				Expires = now.AddMinutes(expireMinutes),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey),
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
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
					ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
