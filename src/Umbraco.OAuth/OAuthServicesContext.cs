using Umbraco.OAuth.Interfaces;

namespace Umbraco.OAuth
{
    public class OAuthServicesContext
    {
        public IOAuthUserService UserService { get; private set; }

        public IOAuthRefreshTokenStore TokenStore { get; private set; }

        public IOAuthTokenService TokenService { get; private set; }

        internal OAuthServicesContext(IOAuthUserService userService,
            IOAuthRefreshTokenStore tokenStore,
            IOAuthTokenService tokenService)
        {
            UserService = userService;
            TokenStore = tokenStore;
            TokenService = tokenService;
        }
    }
}
