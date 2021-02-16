using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Our.Umbraco.AuthU.Models
{
    [PrimaryKey("Id")]
    [TableName("OAuthClient")]
    public class OAuthClient
    {
        [PrimaryKeyColumn]
        public int Id { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        public string ClientId { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        public string Secret { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        public string Name { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        public SecurityLevel SecurityLevel { get; set; }

        public bool Active { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        public int RefreshTokenLifeTime { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        public string AllowedOrigin { get; set; }
    }

    public enum SecurityLevel
    {
        Insecure = 0,
        Secure = 1
    };
}
