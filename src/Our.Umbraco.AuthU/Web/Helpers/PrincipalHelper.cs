using Our.Umbraco.AuthU.Interfaces;
using System.Linq;
using System.Security.Claims;

namespace Our.Umbraco.AuthU.Web.Helpers
{
    internal static class PrincipalHelper
    {
        public static bool ValidatePrincipal(ClaimsPrincipal principal, string targetRealm, IOAuthUserService userService)
        {
            // Make sure principal isn't null
            if (principal == null)
                return false;

            // Make sure identity is authenticated
            if (!principal.Identity.IsAuthenticated)
                return false;

            // Make sure principal belongs to realm
            var realm = principal.Claims.FirstOrDefault(x => x.Type == OAuth.ClaimTypes.Realm)?.Value;
            if (realm == null || realm != targetRealm)
                return false;

            // Make sure username is present
            var username = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return false;

            // Make sure username is valid
            if (!userService.ValidateUser(username))
                return false;

            return true;
        }
    }
}
