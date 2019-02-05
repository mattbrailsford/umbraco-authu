using Umbraco.Core;
using Umbraco.Core.Persistence;
using Our.Umbraco.AuthU.Interfaces;
using Our.Umbraco.AuthU.Models;
using Our.Umbraco.AuthU.Data.Migrations;

namespace Our.Umbraco.AuthU.Data
{
    public class UmbracoDbOAuthRefreshTokenStore : IOAuthRefreshTokenStore
	{
		internal const string CurrentVersion = "1.0.1";
		internal const string SubProductName = "AuthU_UmbracoDbOAuthRefreshTokenStore";

		protected Database Db => ApplicationContext.Current.DatabaseContext.Database;

        public UmbracoDbOAuthRefreshTokenStore()
        {
			MigrationsRunner.RunMigrations(CurrentVersion, SubProductName);
		}

        public void AddRefreshToken(OAuthRefreshToken token)
        {
            Db.Execute("DELETE FROM [OAuthRefreshToken] WHERE [Subject] = @0 AND [UserType] = @1 AND [Realm] = @2 AND [ClientId] = @3 AND [DeviceId] = @4",
                token.Subject,
                token.UserType,
                token.Realm,
                token.ClientId,
				token.DeviceId);

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
    }
}
