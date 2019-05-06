using Our.Umbraco.AuthU.Data.Helpers;
using Our.Umbraco.AuthU.Data.Migrations.UmbracoDbOAuthClientStore;
using Our.Umbraco.AuthU.Data.Migrations.UmbracoDbOAuthRefreshTokenStore;
using Umbraco.Core.Migrations;

namespace Our.Umbraco.AuthU.Data.Migrations.Plans
{
    /// <summary>
    /// This is the plan that runs the AuthU Migrations to remove it from umbraco db
    /// </summary>
    internal class RemoveOAuthTables : MigrationPlan
    {
        public RemoveOAuthTables() : base("AuthU")
        {
            From(InitialState)
            .To<DeleteAuthClientStoreTable>("DeleteAuthClientStoreTable")
            .To<DeleteAuthRefreshTokenStoreTable>("DeleteAuthRefreshTokenStoreTable");
        }

        /// <summary>
        /// Using the helper methid to get the AuthU Key 'Umbraco.Core.Upgrader.State+AuthU' Value
        /// </summary>
        public override string InitialState => AuthUKeyValueHelper.GetValue();
    }


}
