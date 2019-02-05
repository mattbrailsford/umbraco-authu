using Our.Umbraco.AuthU.Web.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Our.Umbraco.AuthU.Web.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OAuthAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public string Realm { get; set; }

        protected OAuthContext Context { get; }

        public OAuthAttribute()
            : this(OAuth.DefaultRealm)
        { }

        public OAuthAttribute(string realm)
        {
            Realm = realm;
            Context = OAuth.GetContext(Realm);
        }

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
            if (!PrincipalHelper.ValidatePrincipal(principal, Realm, Context.Services.UserService))
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
