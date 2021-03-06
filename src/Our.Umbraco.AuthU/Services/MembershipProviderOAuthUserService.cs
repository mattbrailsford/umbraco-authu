using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Security;
using Our.Umbraco.AuthU.Interfaces;

namespace Our.Umbraco.AuthU.Services
{
    public abstract class MembershipProviderOAuthUserService : IOAuthUserService
    {
        public abstract string UserType { get; }

        public abstract string MembershipProviderName { get; }

        protected MembershipProvider MemberProvider => Membership.Providers[this.MembershipProviderName];

        public bool ValidateUser(string username)
        {
            try 
            {
                var user = this.MemberProvider.GetUser(username, false);
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
                return this.MemberProvider.ValidateUser(username, password);
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Claim> GetUserClaims(string username)
        {
            MembershipUser member = null;
            
            try
            {
                member = this.MemberProvider.GetUser(username, true);
            }
            catch {}
            
            if (member != null)
            {
                yield return new Claim(ClaimTypes.NameIdentifier, member.ProviderUserKey.ToString());

                var roles = Roles.GetRolesForUser(member.UserName);
                foreach (var role in roles)
                {
                    yield return new Claim(ClaimTypes.Role, role);
                }
            }
        }
    }
}
