using Our.Umbraco.AuthU.Interfaces;
using Our.Umbraco.AuthU.Models;
using Umbraco.Core.Scoping;

namespace Our.Umbraco.AuthU.Data
{
    public class UmbracoDbOAuthClientStore : IOAuthClientStore
    {
        private readonly IScopeProvider _scopeProvider;

        public UmbracoDbOAuthClientStore(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        public OAuthClient FindClient(string clientId)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                return scope.Database.SingleOrDefault<OAuthClient>("SELECT * FROM [OAuthClient] WHERE [ClientId] = @0", clientId);
            }
        }
    }
}
