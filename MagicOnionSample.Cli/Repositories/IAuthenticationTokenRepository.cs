namespace MagicOnionSample.Cli.Repositories;

public interface IAuthenticationTokenRepository
{
    string AccessToken { get; }
    string RefreshToken { get; }
    bool IsExpired { get; }

    void Update(string accessToken, string refreshToken, long expiresIn);
}