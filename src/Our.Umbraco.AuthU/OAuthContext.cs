using Our.Umbraco.AuthU.Interfaces;
using Our.Umbraco.AuthU.Services;

namespace Our.Umbraco.AuthU
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
                options.ClientStore,
                options.RefreshTokenStore,
                new JwtTokenService(options.SymmetricKey));
        }
    }
}
