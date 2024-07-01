namespace MagicOnionSample.Cli.Repositories;

public class AuthenticationTokenRepository : IAuthenticationTokenRepository
{
    private readonly object _syncObject = new();

    public string AccessToken { get; private set; }
    public string RefreshToken { get; private set; }
    public DateTimeOffset Expiration { get; private set; }

    public bool IsExpired => string.IsNullOrEmpty(AccessToken) || Expiration < DateTimeOffset.UtcNow;

    public void Update(string accessToken, string refreshToken, long expiresIn)
    {
        lock (_syncObject)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Expiration = DateTimeOffset.UtcNow.AddSeconds(expiresIn);
        }
    }
}