using Our.Umbraco.AuthU.Interfaces;
using Our.Umbraco.AuthU.Models;
using Umbraco.Core.Scoping;

namespace Our.Umbraco.AuthU.Data
{
    public class UmbracoDbOAuthRefreshTokenStore : IOAuthRefreshTokenStore
    {
        internal const string CurrentVersion = "1.0.1";
        internal const string SubProductName = "AuthU_UmbracoDbOAuthRefreshTokenStore";

        private readonly IScopeProvider _scopeProvider;

        public UmbracoDbOAuthRefreshTokenStore(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        public void AddRefreshToken(OAuthRefreshToken token)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                scope.Database.Execute("DELETE FROM [OAuthRefreshToken] WHERE [Subject] = @0 AND [UserType] = @1 AND [Realm] = @2 AND [ClientId] = @3 AND [DeviceId] = @4",
                token.Subject,
                token.UserType,
                token.Realm,
                token.ClientId,
                token.DeviceId);

                scope.Database.Save(token);
            }
        }

        public void RemoveRefreshToken(string refreshTokenId)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                scope.Database.Execute("DELETE FROM [OAuthRefreshToken] WHERE [Key] = @0", refreshTokenId);
            }
        }

        public OAuthRefreshToken FindRefreshToken(string refreshTokenId)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                return scope.Database.SingleOrDefault<OAuthRefreshToken>("SELECT * FROM [OAuthRefreshToken] WHERE [Key] = @0", refreshTokenId);
            }
        }
    }
}
