using System;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Umbraco.OAuth.Extensions;
using Umbraco.OAuth.Models;

namespace Umbraco.OAuth.Web.Controllers
{
    public class OAuthController : ApiController
    {
        protected IUmbracoOAuthConfigOptions Config => OAuthConfig.Instance.Options;
        protected OAuthServicesContext Services => OAuthConfig.Instance.Services;

        [HttpPost]
        public object Token(OAuthTokenRequest request)
        {
            HttpContext.Current.Response.Headers.Add("Access-Control-Allow-Origin", Config.AllowedOrigin);

            switch (request.grant_type)
            {
                case "password":
                    return ProcessPasswordTokenRequest(request);

                case "refresh_token":
                    return ProcessRefreshTokenRequest(request);

                default:
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        protected object ProcessPasswordTokenRequest(OAuthTokenRequest request)
        {
            // Validate the user
            if (Services.UserService.ValidateUser(request.username, request.password))
            {
                return ProcessUsernameRequest(request.username);
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        protected object ProcessRefreshTokenRequest(OAuthTokenRequest request)
        {
            // Don't do anything if we don't have a token store registered
            if (Services.TokenStore == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            // Lookup the refresh token
            var key = request.refresh_token.GenerateHash();
            var token = Services.TokenStore.FindRefreshToken(key);
            if (token != null && token.ExpiresUtc > DateTime.UtcNow)
            {
                return ProcessUsernameRequest(token.Subject);
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        protected object ProcessUsernameRequest(string username)
        {
            // Construct an identity
            var claims = Services.UserService.GetUserClaims(username);
            var identity = new ClaimsIdentity(claims);
            var response = new OAuthTokenResponse
            {
                access_token = Services.TokenService.GenerateToken(identity, Config.AccessTokenLifeTime),
                token_type = "bearer",
                expires_in = Config.AccessTokenLifeTime * 60
            };

            // If we have a token store, create a refresh token
            if (Services.TokenStore != null)
            {
                var refreshTokenId = Guid.NewGuid().ToString("n");

                var token = new OAuthRefreshToken
                {
                    Key = refreshTokenId.GenerateHash(),
                    Subject = username,
                    UserType = Services.UserService.UserType,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Config.RefreshTokenLifeTime),
                    ProtectedTicket = response.SerializeToJson().Encrypt(Config.SymmetricKey)
                };

                Services.TokenStore.AddRefreshToken(token);

                response.refresh_token = refreshTokenId;
            }

            return response;
        }
    }
}
