using Our.Umbraco.AuthU.Models;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Our.Umbraco.AuthU.Data.Migrations.UmbracoDbOAuthClientStore
{
	[Migration("1.0.0", 2, "AuthU_UmbracoDbOAuthClientStore")]
	internal class CreateDemoClient : BaseMigration
	{
		public CreateDemoClient(ISqlSyntaxProvider sqlSyntax, ILogger logger) 
			: base(sqlSyntax, logger)
		{ }

		public override void Up()
		{
			var existing = Database.SingleOrDefault<OAuthClient>($"SELECT * FROM [OAuthClient] WHERE [ClientId] = @0", "DemoClient");
			if (existing == null)
			{
				Database.Save(new OAuthClient
				{
					ClientId = "DemoClient",
					Name = "Demo Client",
					Secret = "demo",
					SecurityLevel = SecurityLevel.Insecure,
					RefreshTokenLifeTime = 14400,
					AllowedOrigin = "*"
				});
			}
		}

		public override void Down()
		{
			Database.Execute("DELETE [OAuthClient] WHERE [ClientId] = @0", "DemoClient");
		}
	}
}
