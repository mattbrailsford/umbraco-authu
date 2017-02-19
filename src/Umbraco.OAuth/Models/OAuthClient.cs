using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Umbraco.OAuth.Models
{
    [PrimaryKey("Id")]
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

        [Column("SecurityLevel")]
        [NullSetting(NullSetting = NullSettings.NotNull)]
        public int __SecurityLevel { get; set; }

        [Ignore]
        public SecurityLevel SecurityLevel
        {
            get { return (SecurityLevel)this.__SecurityLevel; }
            set { this.__SecurityLevel = (int)value; }
        }

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
