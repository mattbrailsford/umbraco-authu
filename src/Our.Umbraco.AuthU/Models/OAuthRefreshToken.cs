using System;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Our.Umbraco.AuthU.Models
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
        public string Realm { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        public string ClientId { get; set; }

		[NullSetting(NullSetting = NullSettings.Null)]
		public string DeviceId { get; set; }

		[NullSetting(NullSetting = NullSettings.NotNull)]
        public DateTime IssuedUtc { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        public DateTime ExpiresUtc { get; set; }

        [NullSetting(NullSetting = NullSettings.NotNull)]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string ProtectedTicket { get; set; }
    }
}
