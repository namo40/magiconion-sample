namespace MagicOnionSample.Server.Authentication;

public record JwtServiceOptions
{
    public string AccessTokenSecret { get; init; }
    public long AccessTokenExpirationSeconds { get; init; }
    public string RefreshTokenSecret { get; init; }
    public long RefreshTokenExpirationSeconds { get; init; }

    public byte[] AccessTokenSecretBytes => Convert.FromBase64String(AccessTokenSecret);
    public byte[] RefreshTokenSecretBytes => Convert.FromBase64String(RefreshTokenSecret);
}