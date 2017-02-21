using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Our.Umbraco.AuthU.Interfaces;
using Our.Umbraco.AuthU.Models;

namespace Our.Umbraco.AuthU.Data
{
    public class UmbracoDbOAuthRefreshTokenStore : IOAuthRefreshTokenStore
    {
        protected Database Db => ApplicationContext.Current.DatabaseContext.Database;

        public UmbracoDbOAuthRefreshTokenStore()
        {
            EnsureTablesExist();
        }

        public void AddRefreshToken(OAuthRefreshToken token)
        {
            Db.Execute("DELETE FROM [OAuthRefreshToken] WHERE [Subject] = @0 AND [UserType] = @1 AND [Realm] = @2 AND [ClientId] = @3",
                token.Subject,
                token.UserType,
                token.Realm,
                token.ClientId);

            Db.Save(token);
        }

        public void RemoveRefreshToken(string refreshTokenId)
        {
            Db.Execute("DELETE FROM [OAuthRefreshToken] WHERE [Key] = @0",
                refreshTokenId);
        }

        public OAuthRefreshToken FindRefreshToken(string refreshTokenId)
        {
            return Db.SingleOrDefault<OAuthRefreshToken>("SELECT * FROM [OAuthRefreshToken] WHERE [Key] = @0",
                refreshTokenId);
        }

        protected void EnsureTablesExist()
        {
            var dbCtx = ApplicationContext.Current.DatabaseContext;
            var dbSchemaHelper = new DatabaseSchemaHelper(dbCtx.Database, LoggerResolver.Current.Logger, dbCtx.SqlSyntax);

            if (!dbSchemaHelper.TableExist(typeof(OAuthRefreshToken).Name))
            {
                dbSchemaHelper.CreateTable(false, typeof(OAuthRefreshToken));
            }
        }
    }
}
