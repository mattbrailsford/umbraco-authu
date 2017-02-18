using Umbraco.OAuth.Models;

namespace Umbraco.OAuth.Interfaces
{
    public interface IOAuthRefreshTokenStore
    {
        void AddRefreshToken(OAuthRefreshToken token);

        void RemoveRefreshToken(string refreshTokenId);

        OAuthRefreshToken FindRefreshToken(string refreshTokenId);
    }
}
