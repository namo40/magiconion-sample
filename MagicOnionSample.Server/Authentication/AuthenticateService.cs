using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Options;

namespace MagicOnionSample.Server.Authentication;

public class AuthenticateService(IOptions<JwtServiceOptions> jwtServiceOptions) : IAuthenticateService
{
    private readonly JwtService _accessTokenService = new(jwtServiceOptions.Value.AccessTokenSecretBytes,
        jwtServiceOptions.Value.AccessTokenExpirationSeconds);

    private readonly JwtService _refreshTokenService = new(jwtServiceOptions.Value.RefreshTokenSecretBytes,
        jwtServiceOptions.Value.RefreshTokenExpirationSeconds);

    public AccessTokenResponse Authenticate(long userId, string displayName)
    {
        var claims = CreateClaims(userId, displayName);

        return new AccessTokenResponse
        {
            AccessToken = _accessTokenService.Generate(claims),
            ExpiresIn = jwtServiceOptions.Value.AccessTokenExpirationSeconds,
            RefreshToken = _refreshTokenService.Generate(claims)
        };
    }

    public AccessTokenResponse RefreshToken(string refreshToken)
    {
        var (isAuthenticated, claims) = _refreshTokenService.ValidateToken(refreshToken);
        if (!isAuthenticated)
        {
            throw new UnauthorizedAccessException();
        }

        var accessToken = _accessTokenService.Generate(claims);
        refreshToken = _refreshTokenService.Generate(claims);

        return new AccessTokenResponse
        {
            AccessToken = accessToken,
            ExpiresIn = jwtServiceOptions.Value.AccessTokenExpirationSeconds,
            RefreshToken = refreshToken
        };
    }

    private static Claim[] CreateClaims(long userId, string displayName)
    {
        return
        [
            new Claim(ClaimTypes.Name, displayName),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ];
    }
}