namespace Our.Umbraco.AuthU
{
    public partial class OAuth
    {
        public static readonly string DefaultRealm = "default";

        public static readonly string DefaultClientId = "default";

        public static readonly string DefaultEndpointPath = "/oauth/token";

        public static class ClaimTypes
        {
            public static readonly string Realm = "realm";
            public static readonly string DeviceId = "deviceid";
        }
    }
}