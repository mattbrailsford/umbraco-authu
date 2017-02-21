namespace Our.Umbraco.AuthU.Services
{
    public class UmbracoMembersOAuthUserService : MembershipProviderOAuthUserService
    {
        public override string UserType => "UmbracoMember";
        public override string MembershipProviderName => "UmbracoMembershipProvider";
    }
}
