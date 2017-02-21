using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Our.Umbraco.AuthU.Web.WebApi
{
    internal class AddOAuthChallengeResult : IHttpActionResult
    {
        private IHttpActionResult _innerResult;
        private string _realm;

        public AddOAuthChallengeResult(IHttpActionResult innerResult, string realm)
        {
            this._innerResult = innerResult;
            this._realm = realm;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await this._innerResult.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (response.Headers.WwwAuthenticate.All(h => h.Scheme != "Bearer"))
                {
                    response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Bearer", $"realm=\"{this._realm}\""));
                }
            }
                
            return response;
        }
    }
}