using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Our.Umbraco.AuthU.Web
{
    public class OAuthResponseException : HttpResponseException
    {
        public OAuthResponseException(HttpStatusCode statusCode)
            : base(statusCode)
        { }

        public OAuthResponseException(HttpStatusCode statusCode, object data)
            : base(new HttpResponseMessage(statusCode) {
                Content = new ObjectContent(
                    data.GetType(), 
                    data,
                    new JsonMediaTypeFormatter(), 
                    "application/json"
                )
            })
        { }

        public OAuthResponseException(HttpResponseMessage response)
            : base(response)
        { }
    }
}
