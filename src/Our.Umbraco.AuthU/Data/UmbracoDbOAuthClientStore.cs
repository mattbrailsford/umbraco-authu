using Our.Umbraco.AuthU.Interfaces;
using Our.Umbraco.AuthU.Models;
using NPoco;

namespace Our.Umbraco.AuthU.Data
{
    public class UmbracoDbOAuthClientStore : IOAuthClientStore
    {
		protected Database Db => new Database("umbracoDbDsn");

        public OAuthClient FindClient(string clientId)
        {
            return Db.SingleOrDefault<OAuthClient>("SELECT * FROM [OAuthClient] WHERE [ClientId] = @0", clientId);
        }
    }
}
