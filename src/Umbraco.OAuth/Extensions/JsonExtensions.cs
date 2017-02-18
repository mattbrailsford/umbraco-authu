using Newtonsoft.Json;

namespace Umbraco.OAuth.Extensions
{
    internal static class JsonExtensions
    {
        public static string SerializeToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T DeserializeJsonTo<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
