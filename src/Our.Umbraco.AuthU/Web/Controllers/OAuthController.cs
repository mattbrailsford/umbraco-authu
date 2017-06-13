using System;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Our.Umbraco.AuthU.Extensions;
using Our.Umbraco.AuthU.Models;
using System.Linq;

namespace Our.Umbraco.AuthU.Web.Controllers
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

        protected OAuthClient Client { get; private set; }

        [HttpPost]
        public object Token(OAuthTokenRequest request)
        {
            if (!Context.Options.AllowInsecureHttp && Request.RequestUri.Scheme != Uri.UriSchemeHttps)
                throw new OAuthResponseException(HttpStatusCode.UpgradeRequired, new { invalid_scheme = "Requests must be made over HTTPS" });

            if (request == null)
                throw new OAuthResponseException(HttpStatusCode.BadRequest, new { invalid_request = "Invalid request, please make sure to post request data as application/x-www-form-urlencoded" });

            ProcessClient(request);

	    	SetAllowedOriginHeader();
            
            switch (request.grant_type)
            {
                case "password":
                    return ProcessPasswordTokenRequest(request);

                case "refresh_token":
                    return ProcessRefreshTokenRequest(request);

                default:
                    throw new OAuthResponseException(HttpStatusCode.BadRequest, new { invalid_grant = "Invalid grant_type" });
            }
        }

        protected void ProcessClient(OAuthTokenRequest request)
        {
            if (Context.Services.ClientStore != null)
            {
                if (string.IsNullOrWhiteSpace(request.client_id))
                    throw new OAuthResponseException(HttpStatusCode.Unauthorized, new { invalid_clientId = "A client_id is required" });

                var client = Context.Services.ClientStore.FindClient(request.client_id);
                if (client == null)
                    throw new OAuthResponseException(HttpStatusCode.Unauthorized, new { invalid_clientId = $"Client {request.client_id} is not registered in the system" });

                if (client.SecurityLevel == SecurityLevel.Secure)
                {
                    if (string.IsNullOrWhiteSpace(request.client_secret))
                        throw new OAuthResponseException(HttpStatusCode.Unauthorized, new { invalid_clientSecret = "A client_secret is required" });

                    if (client.Secret != request.client_secret.GenerateHash())
                        throw new OAuthResponseException(HttpStatusCode.Unauthorized, new { invalid_clientSecret = "Invalid client_secret" });
                }

                if (!client.Active)
                    throw new OAuthResponseException(HttpStatusCode.Unauthorized, new { invalid_clientId = "Client is inactive" });

                Client = client;
            }
        }
	
		protected void SetAllowedOriginHeader()
		{
			string AccessControlAllowOriginHeaderKey = "Access-Control-Allow-Origin";
			string allowedOrigin = Client != null ? Client.AllowedOrigin : Context.Options.AllowedOrigin;

			if (HttpContext.Current.Response.Headers.AllKeys.Contains(AccessControlAllowOriginHeaderKey))
			{
				var accessControlHeader = HttpContext.Current.Response.Headers.GetValues(AccessControlAllowOriginHeaderKey).FirstOrDefault();

				if (!accessControlHeader.Equals(allowedOrigin, StringComparison.OrdinalIgnoreCase))
				{
					string errorMessage = $"There is currently a header set with the key {AccessControlAllowOriginHeaderKey}, but the value: {accessControlHeader} differs from the OAuth configured value: {allowedOrigin}";
					throw new OAuthResponseException(HttpStatusCode.InternalServerError, new { invalid_allowed_origin = errorMessage });
				}
			}
			else
			{
				HttpContext.Current.Response.Headers.Add(AccessControlAllowOriginHeaderKey, allowedOrigin);
			}
		}

        protected object ProcessPasswordTokenRequest(OAuthTokenRequest request)
        {
            // Validate the user
            if (Context.Services.UserService.ValidateUser(request.username, request.password))
            {
                return ProcessUsernameRequest(request.username);
            }

            throw new OAuthResponseException(HttpStatusCode.Unauthorized, new { invalid_grant = "The username and/or password is incorrect" });
        }

        protected object ProcessRefreshTokenRequest(OAuthTokenRequest request)
        {
            // Don't do anything if we don't have a token store registered
            if (Context.Services.RefreshTokenStore == null)
                throw new OAuthResponseException(HttpStatusCode.BadRequest, new { invalid_refreshToken = "A refresh token store is not registered in the system" });

            // Lookup the refresh token
            var key = request.refresh_token.GenerateHash();
            var token = Context.Services.RefreshTokenStore.FindRefreshToken(key);
            if (token != null)
            {
                if (token.ExpiresUtc <= DateTime.UtcNow)
                    throw new OAuthResponseException(HttpStatusCode.Unauthorized, new { invalid_refreshToken = "The refresh token has expired" });
                
                if (Client != null && token.ClientId != Client.ClientId)
                    throw new OAuthResponseException(HttpStatusCode.Unauthorized, new { invalid_clientId = "Refresh token is issued to a different clientId" });

                if (token.Realm != Context.Realm)
                    throw new OAuthResponseException(HttpStatusCode.Unauthorized, new { invalid_realm = "Refresh token is issued to a different realm" });

                return ProcessUsernameRequest(token.Subject);
            }

            throw new OAuthResponseException(HttpStatusCode.Unauthorized, new { invalid_refreshToken = $"Refresh token {request.refresh_token} not found" });
        }

        protected object ProcessUsernameRequest(string username)
        {
            // Construct an identity
            var claims = Context.Services.UserService.GetUserClaims(username);

            var identity = new ClaimsIdentity("OAuth");
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            identity.AddClaim(new Claim(OAuth.ClaimTypes.Realm, Context.Realm));
            identity.AddClaims(claims);

            var response = new OAuthTokenResponse
            {
                access_token = Context.Services.TokenService.GenerateToken(identity, Context.Options.AccessTokenLifeTime),
                token_type = "bearer",
                expires_in = Context.Options.AccessTokenLifeTime * 60
            };

            // If we have a token store, create a refresh token
            if (Context.Services.RefreshTokenStore != null)
            {
                var refreshTokenId = Guid.NewGuid().ToString("n");
                var refreshTokenLifeTime = Client?.RefreshTokenLifeTime ?? Context.Options.RefreshTokenLifeTime;

                var token = new OAuthRefreshToken
                {
                    Key = refreshTokenId.GenerateHash(),
                    Subject = username,
                    UserType = Context.Services.UserService.UserType,
                    Realm = Context.Realm,
                    ClientId = Client != null ? Client.ClientId : OAuth.DefaultClientId,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(refreshTokenLifeTime),
                    ProtectedTicket = response.SerializeToJson().Encrypt(Context.Options.SymmetricKey)
                };

                Context.Services.RefreshTokenStore.AddRefreshToken(token);

                response.refresh_token = refreshTokenId;
            }

            return response;
        }
    }
}
