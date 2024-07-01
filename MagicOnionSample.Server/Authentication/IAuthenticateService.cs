using Microsoft.AspNetCore.Authentication.BearerToken;

namespace MagicOnionSample.Server.Authentication;

public interface IAuthenticateService
{
    AccessTokenResponse Authenticate(long userId, string displayName);
    AccessTokenResponse RefreshToken(string refreshToken);
}