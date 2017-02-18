using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Umbraco.OAuth.Web.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OAuthAttribute : BaseOAuthAttribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;

            // Parse the Authentication header
            var header = request.Headers.Authorization;
            if (header == null || header.Scheme != "Bearer" || string.IsNullOrEmpty(header.Parameter))
                return;

            // Extract the principal from the header
            var principal = Context.Services.TokenService.ReadToken(header.Parameter); //TODO: Check this validate
            if (principal == null)
                return;

            // Validate the principal
            if (!ValidatePrincipal(principal))
                return; 

            // Set the current principal
            context.Principal = principal;
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new AddOAuthChallengeResult(context.Result, Realm);
        }
    }
}
