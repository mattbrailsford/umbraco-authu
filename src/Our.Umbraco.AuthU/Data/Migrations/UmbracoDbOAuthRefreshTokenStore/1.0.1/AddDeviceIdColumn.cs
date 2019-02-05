using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Our.Umbraco.AuthU.Data.Migrations.UmbracoDbOAuthRefreshTokenStore
{
	[Migration("1.0.1", 1, Data.UmbracoDbOAuthRefreshTokenStore.SubProductName)]
	internal class AddDeviceIdColumn : BaseMigration
	{
		public AddDeviceIdColumn(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
			: base(sqlSyntax, logger)
		{ }

		public override void Up()
		{
			Alter.Table("OAuthRefreshToken").AddColumn("DeviceId").AsString().Nullable();
		}

		public override void Down()
		{
			Delete.Column("DeviceId").FromTable("OAuthRefreshToken");
		}
	}
}
