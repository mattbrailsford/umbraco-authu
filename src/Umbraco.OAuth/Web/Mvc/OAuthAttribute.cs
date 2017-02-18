using System;
using System.Net.Http.Headers;
using System.Web.Mvc.Filters;
using Umbraco.Core;

namespace Umbraco.OAuth.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OAuthAttribute : BaseOAuthAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var request = filterContext.HttpContext.Request; 
            
            // Parse the Authentication header
            var header = AuthenticationHeaderValue.Parse(request.Headers["Authorization"]);
            if (header == null || header.Scheme != "Bearer" || header.Parameter.IsNullOrWhiteSpace())
                return;
            
            // Extract the principal from the header
            var principal = Context.Services.TokenService.ReadToken(header.Parameter); //TODO: Check this validate
            if (principal == null)
                return;

            // Set the current principal
            filterContext.Principal = principal;
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            context.Result = new AddOAuthChallengeResult(context.Result, Realm);
        }
    }
}
