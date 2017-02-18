using System;
using System.Linq;
using System.Security.Claims;

namespace Umbraco.OAuth.Web
{
    public class BaseOAuthAttribute : Attribute
    {
        protected OAuthServicesContext Services => OAuthConfig.Instance.Services;

        protected bool ValidatePrincipal(ClaimsPrincipal principal)
        {
            // Make sure principal isn't null
            if (principal == null)
                return false;

            // Make sure identity is authenticated
            if (!principal.Identity.IsAuthenticated)
                return false;

            // Make sure username is present
            var username = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return false;

            // Make sure username is valid
            if (!Services.UserService.ValidateUser(username))
                return false;

            return true;
        }
    }
}
