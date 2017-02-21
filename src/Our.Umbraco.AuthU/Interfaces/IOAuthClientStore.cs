using Our.Umbraco.AuthU.Models;

namespace Our.Umbraco.AuthU.Interfaces
{
    public interface IOAuthClientStore
    {
        OAuthClient FindClient(string clientId);
    }
}
