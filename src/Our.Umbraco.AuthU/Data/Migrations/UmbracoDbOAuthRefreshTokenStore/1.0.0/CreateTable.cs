using Our.Umbraco.AuthU.Models;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Our.Umbraco.AuthU.Data.Migrations.UmbracoDbOAuthRefreshTokenStore
{
	[Migration("1.0.0", 1, Data.UmbracoDbOAuthRefreshTokenStore.SubProductName)]
	internal class CreateTable : BaseMigration
	{
		public CreateTable(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
			: base(sqlSyntax, logger)
		{ }

		public override void Up()
		{
			SchemaHelper.CreateTable<OAuthRefreshToken>(false);
		}

		public override void Down()
		{
			SchemaHelper.DropTable<OAuthRefreshToken>();
		}
	}
}
