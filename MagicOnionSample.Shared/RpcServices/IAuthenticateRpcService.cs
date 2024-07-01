using MagicOnion;
using MagicOnionSample.ClientShared.Models;

namespace MagicOnionSample.ClientShared.RpcServices
{
    public interface IAuthenticateRpcService : IService<IAuthenticateRpcService>
    {
        UnaryResult<AuthenticateResponse> AuthenticateAsync(long userId, string displayName);
        UnaryResult<AuthenticateResponse> RefreshTokenAsync(string refreshToken);
        UnaryResult<string> GetDisplayNameAsync();
    }
}