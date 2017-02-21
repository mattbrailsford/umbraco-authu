using System.Security.Claims;

namespace Our.Umbraco.AuthU.Interfaces
{
    public interface IOAuthTokenService
    {
        string GenerateToken(ClaimsIdentity identity, int lifeTime);

        ClaimsPrincipal ReadToken(string token);
    }
}
