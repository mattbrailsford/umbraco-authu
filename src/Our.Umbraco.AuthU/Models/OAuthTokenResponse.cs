using Newtonsoft.Json;

namespace Our.Umbraco.AuthU.Models
{
    public class OAuthTokenResponse
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string refresh_token { get; set; }
    }
}
