using Our.Umbraco.AuthU.Models;
using Umbraco.Core.Migrations;

namespace Our.Umbraco.AuthU.Data.Migrations
{
    /// <summary>
    /// Simple migration to add a demo client to the OAuthClient Table
    /// </summary>
    internal class CreateDemoOAuthClient : MigrationBase
    {
        public CreateDemoOAuthClient(IMigrationContext context) : base(context)
        { }

        public override void Migrate()
        {
            var existing = Database.SingleOrDefault<OAuthClient>($"SELECT 1 FROM [OAuthClient] WHERE [ClientId] = @0", "DemoClient");
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
    }
}
