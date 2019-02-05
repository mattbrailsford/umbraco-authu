using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Our.Umbraco.AuthU.Data.Migrations
{
	internal abstract class BaseMigration : MigrationBase
	{
		protected Database Database
		{
			get
			{
				return ApplicationContext.Current.DatabaseContext.Database;
			}
		}

		protected DatabaseSchemaHelper SchemaHelper
		{
			get
			{
				return new DatabaseSchemaHelper(Database, Logger, SqlSyntax);
			}
		}

		protected BaseMigration(ISqlSyntaxProvider sqlSyntax, ILogger logger)
			: base(sqlSyntax, logger)
		{ }
	}
}
