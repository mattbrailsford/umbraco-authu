using Umbraco.Core.Migrations;

namespace Our.Umbraco.AuthU.Data.Migrations
{
    /// <summary>
    /// Simple migration to create the DeviceId column on the OAuthRefreshToken Table
    /// </summary>
    internal class AddDeviceIdColumnToOAuthRefreshTokenTable : MigrationBase
    {
        public AddDeviceIdColumnToOAuthRefreshTokenTable(IMigrationContext context) : base(context)
        { }

        public override void Migrate()
        {
            if (!ColumnExists("OAuthRefreshToken", "DeviceId"))
                Alter.Table("OAuthRefreshToken").AddColumn("DeviceId").AsString().Nullable();
        }

    }
}
