using System.Security.Claims;

namespace Umbraco.OAuth.Interfaces
{
    public interface IOAuthTokenService
    {
        string GenerateToken(ClaimsIdentity identity, int lifeTime);

        ClaimsPrincipal ReadToken(string token);
    }
}
