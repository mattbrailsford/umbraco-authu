namespace Umbraco.OAuth.Services
{
    public class UmbracoMembersOAuthUserService : MembershipProviderOAuthUserService
    {
        public override string UserType => "UmbracoMember";
        public override string MembershipProviderName => "UmbracoMembershipProvider";
    }
}
