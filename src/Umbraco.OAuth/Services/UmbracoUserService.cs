namespace Umbraco.OAuth.Services
{
    public class UmbracoUsersOAuthUserService : MembershipProviderOAuthUserService
    {
        public override string UserType => "UmbracoUser";
        public override string MembershipProviderName => "UsersMembershipProvider";
    }
}
