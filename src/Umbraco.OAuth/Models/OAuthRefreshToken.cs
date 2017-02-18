using System;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Umbraco.OAuth.Models
{
    [PrimaryKey("Id")]
    public class OAuthRefreshToken
    {
        [PrimaryKeyColumn]
        public int Id { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        public string Key { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        public string Subject { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        public string UserType { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        public DateTime IssuedUtc { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        public DateTime ExpiresUtc { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string ProtectedTicket { get; set; }
    }
}
