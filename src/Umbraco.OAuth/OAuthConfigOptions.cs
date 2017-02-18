using Umbraco.OAuth.Interfaces;
using Umbraco.OAuth.Services;

namespace Umbraco.OAuth
{
    public interface IUmbracoOAuthConfigOptions
    {
        string SymmetricKey { get; }

        string AllowedOrigin { get; }

        int AccessTokenLifeTime { get; }

        int RefreshTokenLifeTime { get; }
    }

    public class OAuthConfigOptions : IUmbracoOAuthConfigOptions
    {
        public OAuthConfigOptions()
        {
            UserService = new UmbracoMembersOAuthUserService();
            TokenEndpoint = "/oauth/token";
            AllowedOrigin = "*";
            AccessTokenLifeTime = 20; // 20 minutes
            RefreshTokenLifeTime = 1440; // 1 day
        }

        public IOAuthUserService UserService { get; set; }

        public string TokenEndpoint { get; set; }

        public string SymmetricKey { get; set; }

        public string AllowedOrigin { get; set; }

        public int AccessTokenLifeTime { get; set; }

        public IOAuthRefreshTokenStore RefreshTokenStore { get; set; }

        public int RefreshTokenLifeTime { get; set; }
    }
}
