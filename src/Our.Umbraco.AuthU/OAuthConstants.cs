namespace Our.Umbraco.AuthU
{
    public partial class OAuth
    {
        internal static readonly string DefaultRealm = "default";

        internal static readonly string DefaultClientId = "default";

        internal static readonly string DefaultEndpointPath = "/oauth/token";

        internal static class ClaimTypes
        {
            internal static readonly string Realm = "realm";
			internal static readonly string DeviceId = "deviceid";
		}
    }
}