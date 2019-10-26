using Our.Umbraco.AuthU.Models;
using Umbraco.Core.Migrations;

namespace Our.Umbraco.AuthU.Data.Migrations
{
    /// <summary>
    /// Simple migration to create the OAuthRefreshToken Table
    /// </summary>
    internal class CreateOAuthRefreshTokenTable : MigrationBase
    {
        public CreateOAuthRefreshTokenTable(IMigrationContext context) : base(context)
        { }

        public override void Migrate()
        {
            if (!TableExists("OAuthRefreshToken"))
                Create.Table<OAuthRefreshToken>().Do();
        }
    }
}
