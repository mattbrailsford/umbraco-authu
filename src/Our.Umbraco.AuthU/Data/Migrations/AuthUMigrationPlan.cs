using Umbraco.Core.Migrations;

namespace Our.Umbraco.AuthU.Data.Migrations
{
    /// <summary>
    /// This is the plan that runs the AuthU Migrations
    /// </summary>
    internal class AuthUMigrationPlan : MigrationPlan
    {
        /// <summary>
        /// Add the demo id if preferred
        /// </summary>
        /// <param name="CreateDemoClient"></param>
        public AuthUMigrationPlan() : base("AuthU")
        {
            From(InitialState)
                .To<CreateOAuthClientTable>("CreateOAuthClientTable")
                .To<CreateOAuthRefreshTokenTable>("CreateOAuthRefreshTokenTable")
                .To<AddDeviceIdColumnToOAuthRefreshTokenTable>("AddDeviceIdColumnToOAuthRefreshTokenTable")
                .To<CreateDemoOAuthClient>("CreateDemoOAuthClient");
        }

        /// <summary>
        /// Using the helper method to get the AuthU Key 'Umbraco.Core.Upgrader.State+AuthU' Value
        /// </summary>
        public override string InitialState => string.Empty;
    }
}
