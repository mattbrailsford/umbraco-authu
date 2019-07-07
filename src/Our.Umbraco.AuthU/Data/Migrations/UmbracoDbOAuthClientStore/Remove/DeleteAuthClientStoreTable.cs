using Umbraco.Core.Migrations;

namespace Our.Umbraco.AuthU.Data.Migrations.UmbracoDbOAuthClientStore
{
    /// <summary>
    /// Simple migration to delete the OAuthClient Table
    /// </summary>
    internal class DeleteAuthClientStoreTable : MigrationBase
	{
		public DeleteAuthClientStoreTable(IMigrationContext context) 
			: base(context)
		{ }

        public override void Migrate()
        {
            if (TableExists("OAuthClient"))
                Delete.Table("OAuthClient").Do();
        }
    }
}
