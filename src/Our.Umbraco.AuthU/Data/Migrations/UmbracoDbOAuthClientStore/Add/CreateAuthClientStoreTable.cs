using Our.Umbraco.AuthU.Models;
using Umbraco.Core.Migrations;

namespace Our.Umbraco.AuthU.Data.Migrations.UmbracoDbOAuthClientStore
{
    /// <summary>
    /// Simple migration to create the OAuthClient Table
    /// </summary>
    internal class CreateAuthClientStoreTable : MigrationBase
	{
		public CreateAuthClientStoreTable(IMigrationContext context) 
			: base(context)
		{ }

        public override void Migrate()
        {
            if (!TableExists("OAuthClient"))
                Create.Table<OAuthClient>().Do();
        }
    }
}
