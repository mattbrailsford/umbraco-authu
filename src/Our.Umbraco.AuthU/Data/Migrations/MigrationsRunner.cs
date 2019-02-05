using Semver;
using System;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;

namespace Our.Umbraco.AuthU.Data.Migrations
{
	internal class MigrationsRunner
	{
		public static void RunMigrations(string version, string productName)
		{
			var currentVersion = new SemVersion(0, 0, 0);

			var migrations = ApplicationContext.Current.Services.MigrationEntryService.GetAll(productName);
			var latestMigration = migrations.OrderByDescending(x => x.Version).FirstOrDefault();
			if (latestMigration != null)
				currentVersion = latestMigration.Version;

			var targetVersion = SemVersion.Parse(version);
			if (targetVersion == currentVersion)
				return;

			var migrationsRunner = new MigrationRunner(
			  ApplicationContext.Current.Services.MigrationEntryService,
			  ApplicationContext.Current.ProfilingLogger.Logger,
			  currentVersion,
			  targetVersion,
			  productName);

			try
			{
				migrationsRunner.Execute(ApplicationContext.Current.DatabaseContext.Database);
			}
			catch (Exception e)
			{
				LogHelper.Error<MigrationsRunner>($"Error running {productName} migration", e);
			}
		}
	}
}
