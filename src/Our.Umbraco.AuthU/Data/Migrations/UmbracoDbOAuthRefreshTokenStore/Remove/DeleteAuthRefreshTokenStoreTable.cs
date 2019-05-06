using Our.Umbraco.AuthU.Models;
using Umbraco.Core.Migrations;

namespace Our.Umbraco.AuthU.Data.Migrations.UmbracoDbOAuthRefreshTokenStore
{
    /// <summary>
    /// Simple migration to delete the OAuthRefreshToken Table
    /// </summary>
    internal class DeleteAuthRefreshTokenStoreTable : MigrationBase
    {
        public DeleteAuthRefreshTokenStoreTable(IMigrationContext context) : base(context)
        { }

        public override void Migrate()
        {
            if (TableExists("OAuthRefreshToken"))
                Delete.Table("OAuthRefreshToken").Do();
        }
    }
}
