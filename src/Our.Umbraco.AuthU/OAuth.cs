using System;
using System.Collections.Concurrent;
using System.Web;
using System.Web.Http;

namespace Our.Umbraco.AuthU
{
    public partial class OAuth
    {
        private static ConcurrentDictionary<string, OAuthContext> _contexts = new ConcurrentDictionary<string, OAuthContext>();

        public static OAuthContext GetContext()
        {
            return GetContext(DefaultRealm);
        }

        public static OAuthContext GetContext(string realm)
        {
            OAuthContext config;
            if (_contexts.TryGetValue(realm, out config))
            {
                return config;
            }
            
            string currentAbsolutePath = HttpContext.Current.Request.Url.AbsolutePath;

			if(!string.IsNullOrWhiteSpace(currentAbsolutePath))
			{
				// the umbraco upgrade process calls two methods in the /install/api/ when upgrading GetSetup and PostPerformInstall

				if(currentAbsolutePath.StartsWith("/install/api/"))
				{
					// user is installing a new version of umbraco.
					return config;
				}
			}

            throw new Exception($"And endpoint for the realm \"{realm}\" has not yet been configured. Please call ConfigureEndpoint first.");
        }

        public static void ConfigureEndpoint(OAuthOptions config)
        {
            ConfigureEndpoint(DefaultRealm, DefaultEndpointPath, config);
        }

        public static void ConfigureEndpoint(string path, OAuthOptions config)
        {
            ConfigureEndpoint(DefaultRealm, path, config);
        }

        public static void ConfigureEndpoint(string realm, string path, OAuthOptions options)
        {
            if (!_contexts.ContainsKey(realm))
            {
                if (_contexts.TryAdd(realm, new OAuthContext(realm, options)))
                {
                    GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                        "OAuth_" + realm,
                        path.TrimStart('~', '/'),
                        new
                        {
                            controller = "OAuth",
                            action = "Token",
                            realm
                        });

                    return;
                }
            }

            throw new Exception($"An endpoint for the realm \"{realm}\" has already been configured.");
        }
    }
}
