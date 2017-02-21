using Our.Umbraco.AuthU.Models;

namespace Our.Umbraco.AuthU.Interfaces
{
    public interface IOAuthRefreshTokenStore
    {
        void AddRefreshToken(OAuthRefreshToken token);

        void RemoveRefreshToken(string refreshTokenId);

        OAuthRefreshToken FindRefreshToken(string refreshTokenId);
    }
}
