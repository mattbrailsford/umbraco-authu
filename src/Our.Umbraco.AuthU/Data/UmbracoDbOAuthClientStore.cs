using Umbraco.Core;
using Umbraco.Core.Persistence;
using Our.Umbraco.AuthU.Interfaces;
using Our.Umbraco.AuthU.Models;
using Our.Umbraco.AuthU.Data.Migrations;

namespace Our.Umbraco.AuthU.Data
{
    public class UmbracoDbOAuthClientStore : IOAuthClientStore
    {
		internal const string CurrentVersion = "1.0.0";
		internal const string SubProductName = "AuthU_UmbracoDbOAuthClientStore";

		protected Database Db => ApplicationContext.Current.DatabaseContext.Database;

        public UmbracoDbOAuthClientStore()
        {
			MigrationsRunner.RunMigrations(CurrentVersion, SubProductName);
		}

        public OAuthClient FindClient(string clientId)
        {
            return Db.SingleOrDefault<OAuthClient>("SELECT * FROM [OAuthClient] WHERE [ClientId] = @0",
                clientId);
        }
    }
}
