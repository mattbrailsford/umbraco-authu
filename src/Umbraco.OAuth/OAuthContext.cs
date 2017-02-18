using Umbraco.OAuth.Services;

namespace Umbraco.OAuth
{
    public class OAuthContext
    {
        public string Realm { get; private set; }

        public IOAuthOptions Options { get; private set; }

        public OAuthServicesContext Services { get; private set; }

        public OAuthContext(string realm, OAuthOptions options)
        {
            Realm = realm;

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
