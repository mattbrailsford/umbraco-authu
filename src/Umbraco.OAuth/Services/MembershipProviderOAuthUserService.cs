using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Security;
using Umbraco.OAuth.Interfaces;

namespace Umbraco.OAuth.Services
{
    public abstract class MembershipProviderOAuthUserService : IOAuthUserService
    {
        public abstract string UserType { get; }

        public abstract string MembershipProviderName { get; }

        protected MembershipProvider MemberProvider => Membership.Providers[this.MembershipProviderName];

        public bool ValidateUser(string username)
        {
            return this.MemberProvider.GetUser(username, false) != null;
        }

        public bool ValidateUser(string username, string password)
        {
            return this.MemberProvider.ValidateUser(username, password);
        }

        public IEnumerable<Claim> GetUserClaims(string username)
        {
            var member = this.MemberProvider.GetUser(username, true);
            if (member != null)
            {
                yield return new Claim(ClaimTypes.NameIdentifier, member.ProviderUserKey.ToString());
                yield return new Claim(ClaimTypes.Name, member.UserName);

                var roles = Roles.GetRolesForUser(member.UserName);
                foreach (var role in roles)
                {
                    yield return new Claim(ClaimTypes.Role, role);
                }
            }
        }
    }
}
