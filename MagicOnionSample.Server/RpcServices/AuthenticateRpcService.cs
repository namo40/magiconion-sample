using Grpc.Core;
using MagicOnion;
using MagicOnion.Server;
using MagicOnionSample.ClientShared.Models;
using MagicOnionSample.ClientShared.RpcServices;
using MagicOnionSample.Server.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace MagicOnionSample.Server.RpcServices;

[Authorize]
public sealed class AuthenticateRpcService(IAuthenticateService authenticateService)
    : ServiceBase<IAuthenticateRpcService>, IAuthenticateRpcService
{
    [AllowAnonymous]
    public UnaryResult<AuthenticateResponse> AuthenticateAsync(long userId, string displayName)
    {
        var accessTokenResponse = authenticateService.Authenticate(userId, displayName);

        return UnaryResult.FromResult(new AuthenticateResponse
        {
            AccessToken = accessTokenResponse.AccessToken,
            RefreshToken = accessTokenResponse.RefreshToken,
            ExpiresIn = accessTokenResponse.ExpiresIn,
            Success = true
        });
    }

    [AllowAnonymous]
    public UnaryResult<AuthenticateResponse> RefreshTokenAsync(string refreshToken)
    {
        var accessTokenResponse = authenticateService.RefreshToken(refreshToken);

        return UnaryResult.FromResult(new AuthenticateResponse
        {
            AccessToken = accessTokenResponse.AccessToken,
            RefreshToken = accessTokenResponse.RefreshToken,
            ExpiresIn = accessTokenResponse.ExpiresIn,
            Success = true
        });
    }

    public UnaryResult<string> GetDisplayNameAsync()
        => UnaryResult.FromResult(Context.CallContext.GetHttpContext().User.Identity?.Name);
}