using Our.Umbraco.AuthU.Models;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Our.Umbraco.AuthU.Data.Migrations.UmbracoDbOAuthClientStore
{
	[Migration("1.0.0", 1, Data.UmbracoDbOAuthClientStore.SubProductName)]
	internal class CreateTable : BaseMigration
	{
		public CreateTable(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
			: base(sqlSyntax, logger)
		{ }

		public override void Up()
		{
			SchemaHelper.CreateTable<OAuthClient>(false);
		}

		public override void Down()
		{
			SchemaHelper.DropTable<OAuthClient>();
		}
	}
}
