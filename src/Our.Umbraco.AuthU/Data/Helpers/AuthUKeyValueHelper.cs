using Our.Umbraco.AuthU.Models;
using Umbraco.Core.Composing;

namespace Our.Umbraco.AuthU.Data.Helpers
{
    /// <summary>
    /// This is a helper to retrieve the latest store value, created to allow us to run migrations
    /// without storing the key anywhere else.
    /// [umbracoKeyValue] - Umbraco.Core.Upgrader.State+AuthU
    /// </summary>
    public static class AuthUKeyValueHelper
    {
        public static string GetValue()
        {
            string authUState = string.Empty;

            using (var scope = Current.ScopeProvider.CreateScope())
            {
                var database = scope.Database;               
                var existing = database.SingleOrDefault<UmbracoKeyValue>("SELECT * FROM [umbracoKeyValue] WHERE [key] = 'Umbraco.Core.Upgrader.State+AuthU'");
                if (existing != null)
                {
                    authUState = existing.Value;
                }
                scope.Complete();
            }

            return authUState;
        }
    }
}
