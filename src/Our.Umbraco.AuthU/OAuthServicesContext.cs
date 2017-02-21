using Our.Umbraco.AuthU.Interfaces;

namespace Our.Umbraco.AuthU
{
    public class OAuthServicesContext
    {
        public IOAuthUserService UserService { get; private set; }

        public IOAuthClientStore ClientStore { get; private set; }

        public IOAuthRefreshTokenStore RefreshTokenStore { get; private set; }

        public IOAuthTokenService TokenService { get; private set; }

        internal OAuthServicesContext(IOAuthUserService userService,
            IOAuthClientStore clientStore,
            IOAuthRefreshTokenStore refreshTokenStore,
            IOAuthTokenService tokenService)
        {
            UserService = userService;
            ClientStore = clientStore;
            RefreshTokenStore = refreshTokenStore;
            TokenService = tokenService;
        }
    }
}
