using Our.Umbraco.AuthU.Component;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Our.Umbraco.AuthU.Composer
{
    /// <summary>
    /// This class registers components at umbraco start up.
    /// </summary>
    internal class MigrationRunnerComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            // Registers the Migrations Runner component, this creates the tables
            // required for the oAuth stores.
            composition.Components().Append<MigrationsRunnerComponent>();
        }
    }
}
