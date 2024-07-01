using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace MagicOnionSample.Server.Authentication;

public sealed class JwtService(byte[] secretBytes, double expirationSeconds)
{
    private readonly SymmetricSecurityKey _issuerSigningKey = new(secretBytes);

    public string Generate(IEnumerable<Claim> claims)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        var token = jwtSecurityTokenHandler.CreateEncodedJwt(new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(expirationSeconds)
        });

        return token;
    }

    public (bool isAuthenticated, Claim[] claims) ValidateToken(string token)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var validationResult =
            jwtSecurityTokenHandler.ValidateToken(token, GetTokenValidationParameters(), out _);

        return validationResult.Identity?.IsAuthenticated == true
            ? (true, validationResult.Claims.ToArray())
            : (false, null);
    }

    private TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            IssuerSigningKey = _issuerSigningKey,
            ClockSkew = TimeSpan.Zero,

            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true
        };
    }
}