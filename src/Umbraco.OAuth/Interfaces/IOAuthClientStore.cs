using Umbraco.OAuth.Models;

namespace Umbraco.OAuth.Interfaces
{
    public interface IOAuthClientStore
    {
        OAuthClient FindClient(string clientId);
    }
}
