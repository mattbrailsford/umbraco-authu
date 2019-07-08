using Our.Umbraco.AuthU.Interfaces;
using Our.Umbraco.AuthU.Models;
using Umbraco.Core.Composing;

namespace Our.Umbraco.AuthU.Data
{
    public class UmbracoDbOAuthClientStore : IOAuthClientStore
    {
        public OAuthClient FindClient(string clientId)
        {
            using (var scope = Current.ScopeProvider.CreateScope(autoComplete: true))
            {
                return scope.Database.SingleOrDefault<OAuthClient>("SELECT * FROM [OAuthClient] WHERE [ClientId] = @0", clientId);
            }
        }
    }
}
