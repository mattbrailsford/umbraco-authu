using System;
using System.Linq;
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
        private OAuthContext _context;
        protected OAuthContext Context
        {
            get
            {
                if (_context == null)
                {
                    var realm = RequestContext.RouteData.Values["realm"].ToString();
                    _context = OAuth.GetContext(realm);
                }

                return _context;
            }
        }

        [HttpPost]
        public object Token(OAuthTokenRequest request)
        {
            HttpContext.Current.Response.Headers.Add("Access-Control-Allow-Origin", Context.Options.AllowedOrigin);

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
            if (Context.Services.UserService.ValidateUser(request.username, request.password))
            {
                return ProcessUsernameRequest(request.username);
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        protected object ProcessRefreshTokenRequest(OAuthTokenRequest request)
        {
            // Don't do anything if we don't have a token store registered
            if (Context.Services.TokenStore == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            // Lookup the refresh token
            var key = request.refresh_token.GenerateHash();
            var token = Context.Services.TokenStore.FindRefreshToken(key);
            if (token != null && token.ExpiresUtc > DateTime.UtcNow)
            {
                return ProcessUsernameRequest(token.Subject);
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        protected object ProcessUsernameRequest(string username)
        {
            // Construct an identity
            var claims = Context.Services.UserService.GetUserClaims(username);

            var identity = new ClaimsIdentity(claims);
            identity.AddClaim(new Claim(ClaimTypes.GroupSid, Context.Realm.GenerateHash()));

            var response = new OAuthTokenResponse
            {
                access_token = Context.Services.TokenService.GenerateToken(identity, Context.Options.AccessTokenLifeTime),
                token_type = "bearer",
                expires_in = Context.Options.AccessTokenLifeTime * 60
            };

            // If we have a token store, create a refresh token
            if (Context.Services.TokenStore != null)
            {
                var refreshTokenId = Guid.NewGuid().ToString("n");

                var token = new OAuthRefreshToken
                {
                    Key = refreshTokenId.GenerateHash(),
                    Subject = username,
                    UserType = Context.Services.UserService.UserType,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Context.Options.RefreshTokenLifeTime),
                    ProtectedTicket = response.SerializeToJson().Encrypt(Context.Options.SymmetricKey)
                };

                Context.Services.TokenStore.AddRefreshToken(token);

                response.refresh_token = refreshTokenId;
            }

            return response;
        }
    }
}
