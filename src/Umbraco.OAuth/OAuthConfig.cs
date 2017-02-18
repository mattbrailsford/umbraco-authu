using System;
using System.Web.Http;
using Umbraco.OAuth.Services;

namespace Umbraco.OAuth
{
    public class OAuthConfig
    {
        public static OAuthConfig _instance;
        private static object _mutex = new object();

        public IUmbracoOAuthConfigOptions Options { get; private set; }
        public OAuthServicesContext Services { get; private set; }

        private OAuthConfig(OAuthConfigOptions options)
        {
            // Store options
            Options = options;

            // Create services
            Services = new OAuthServicesContext(
                options.UserService,
                options.RefreshTokenStore,
                new JwtTokenService(options.SymmetricKey));

            // Configure the controller route
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                "OAuth",
                options.TokenEndpoint.TrimStart('~', '/'),
                new
                {
                    controller = "OAuth",
                    action = "Token"
                });
        }

        public static OAuthConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception("UmbracoOAuth has not yet been configured. You must call Configure beforce accessing the config instance.");
                }

                return _instance;
            }
        }

        public static void Configure(OAuthConfigOptions options)
        {
            if (_instance == null)
            {
                lock (_mutex) // now I can claim some form of thread safety...
                {
                    if (_instance == null)
                    {
                        _instance = new OAuthConfig(options);

                        return;
                    }
                }
            }

            throw new Exception("UmbracoOAuth has already been configured");
        }
    }
}
