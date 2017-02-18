using Umbraco.OAuth.Services;

namespace Umbraco.OAuth
{
    public class OAuthContext
    {
        public IOAuthOptions Options { get; private set; }

        public OAuthServicesContext Services { get; private set; }

        public OAuthContext(OAuthOptions options)
        {
            // Store options
            Options = options;

            // Create services
            Services = new OAuthServicesContext(
                options.UserService,
                options.RefreshTokenStore,
                new JwtTokenService(options.SymmetricKey));
        }
    }
}
