using System;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Composing;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using Our.Umbraco.AuthU.Data.Migrations.Plans;

namespace Our.Umbraco.AuthU.Component
{
    /// <summary>
    /// This creates a component we can register as umbraco 8 starts up
    /// This is done in MigrationRunnerComposer.cs
    /// </summary>
    internal class MigrationsRunnerComponent : IComponent
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IMigrationBuilder _migrationBuilder;
        private readonly IKeyValueService _keyValueService;
        private readonly ILogger _logger;

        public MigrationsRunnerComponent(IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger)
        {
            _scopeProvider = scopeProvider;
            _migrationBuilder = migrationBuilder;
            _keyValueService = keyValueService;
            _logger = logger;
        }

        public void Initialize()
        {
            try
            {
                //Create the oAuth tables
                var upgrader = new Upgrader(new RegisterOAuthTables());

                upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);
            }
            catch (Exception e)
            {
                Current.Logger.Error<MigrationsRunnerComponent>($"Error running Migration Planner migration", e);
            }
        }

        public void Terminate()
        {

        }
    }
}
