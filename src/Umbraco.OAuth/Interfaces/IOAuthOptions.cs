namespace Umbraco.OAuth.Interfaces
{
    public interface IOAuthOptions
    {
        string SymmetricKey { get; }

        string AllowedOrigin { get; }

        int AccessTokenLifeTime { get; }

        int RefreshTokenLifeTime { get; }

        bool AllowInsecureHttp { get; }
    }
}