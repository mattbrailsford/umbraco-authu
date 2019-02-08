using Our.Umbraco.AuthU.Web.Helpers;
using System;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Our.Umbraco.AuthU.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class OAuthAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public string Realm { get; set; } = OAuth.DefaultRealm;

        protected OAuthContext Context {
            get { return OAuth.GetContext(Realm); }
        }

        public OAuthAttribute(string realm)
        {
            Realm = realm;
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            try
            {
                // Parse the Authentication header
                var header = AuthenticationHeaderValue.Parse(request.Headers["Authorization"]);
                if (header == null || header.Scheme != "Bearer" || string.IsNullOrWhiteSpace(header.Parameter))
                    return;

                // Extract the principal from the header
                var principal = Context.Services.TokenService.ReadToken(header.Parameter); //TODO: Check this validate
                if (principal == null)
                    return;

                // Validate the principal
                if (!PrincipalHelper.ValidatePrincipal(principal, Realm, Context.Services.UserService))
                    return;

                // Set the current principal
                filterContext.Principal = principal;
            }
            catch(FormatException ex)
            {
                return;
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            context.Result = new AddOAuthChallengeResult(context.Result, Realm);
        }
    }
}
