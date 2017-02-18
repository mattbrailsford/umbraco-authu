using System.Net;
using System.Web.Mvc;

namespace Umbraco.OAuth.Web.Mvc
{
    internal class AddOAuthChallengeResult : ActionResult
    {
        private ActionResult _innerResult;
        private string _realm;

        public AddOAuthChallengeResult(ActionResult innerResult, string realm)
        {
            this._innerResult = innerResult;
            this._realm = realm;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            this._innerResult.ExecuteResult(context);

            if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                var header = context.HttpContext.Response.Headers["WWW-Authenticate"];
                if (string.IsNullOrWhiteSpace(header) || !header.StartsWith("Bearer"))
                    context.HttpContext.Response.Headers["WWW-Authenticate"] = $"Bearer realm=\"{this._realm}\"";
            } 
        }
    }
}