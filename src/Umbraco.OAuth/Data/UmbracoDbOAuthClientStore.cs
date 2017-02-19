using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.OAuth.Interfaces;
using Umbraco.OAuth.Models;

namespace Umbraco.OAuth.Data
{
    public class UmbracoDbOAuthClientStore : IOAuthClientStore
    {
        protected Database Db => ApplicationContext.Current.DatabaseContext.Database;

        public UmbracoDbOAuthClientStore()
        {
            this.EnsureTablesExist();
        }

        public OAuthClient FindClient(string clientId)
        {
            return this.Db.SingleOrDefault<OAuthClient>("SELECT * FROM [OAuthClient] WHERE [ClientId] = @0",
                clientId);
        }

        protected void EnsureTablesExist()
        {
            var dbCtx = ApplicationContext.Current.DatabaseContext;
            var dbSchemaHelper = new DatabaseSchemaHelper(dbCtx.Database, LoggerResolver.Current.Logger, dbCtx.SqlSyntax);

            if (!dbSchemaHelper.TableExist(typeof(OAuthClient).Name))
            {
                // Create table
                dbSchemaHelper.CreateTable(false, typeof(OAuthClient));

                // Seed the table
                dbCtx.Database.Save(new OAuthClient
                {
                    ClientId = "DemoClient",
                    Name = "Demo Client",
                    Secret = "demo",
                    SecurityLevel = SecurityLevel.Insecure,
                    RefreshTokenLifeTime = 14400,
                    AllowedOrigin = "*"
                });
            }
        }
    }
}
