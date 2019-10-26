using Our.Umbraco.AuthU.Models;
using Umbraco.Core.Migrations;

namespace Our.Umbraco.AuthU.Data.Migrations
{
    /// <summary>
    /// Simple migration to create the OAuthClient Table
    /// </summary>
    internal class CreateOAuthClientTable : MigrationBase
	{
		public CreateOAuthClientTable(IMigrationContext context) 
			: base(context)
		{ }

        public override void Migrate()
        {
            if (!TableExists("OAuthClient"))
                Create.Table<OAuthClient>().Do();
        }
    }
}
