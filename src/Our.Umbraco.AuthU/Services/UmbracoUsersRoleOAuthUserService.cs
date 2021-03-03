using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Security;
using Our.Umbraco.AuthU.Interfaces;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;
using Umbraco.Core.Composing;

namespace Our.Umbraco.AuthU.Services
{
    public abstract class UmbracoUsersRoleOAuthUserService : IOAuthUserService
    {
        public string UserType => "UmbracoUser";
        private MembershipProvider MemberProvider => Membership.Providers["UsersMembershipProvider"];
        private readonly IUserService _userService = Current.Services.UserService;

        public bool ValidateUser(string username)
        {
            try
            {
                var user = _userService.GetByUsername(username);
                return user != null && user.IsApproved && !user.IsLockedOut;
            }
            catch
            {
                return false;
            }
        }

        public bool ValidateUser(string username, string password)
        {
            try
            {
                return MemberProvider.ValidateUser(username, password);
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Claim> GetUserClaims(string username)
        {
            IUser user = null;

            try
            {
                user = _userService.GetByUsername(username);
            }
            catch { }

            if (user != null)
            {
                yield return new Claim(ClaimTypes.NameIdentifier, user.ProviderUserKey.ToString());

                var roles = user.Groups.Select(g => g.Alias);
                foreach (var role in roles)
                {
                    yield return new Claim(ClaimTypes.Role, role);
                }
            }
        }
    }
}
