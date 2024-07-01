using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using MagicOnionSample.Cli.Extensions;
using MagicOnionSample.Cli.Repositories;
using MagicOnionSample.ClientShared.RpcServices;
using Spectre.Console;

namespace MagicOnionSample.Cli.ClientFilters;

/// <summary>
/// 인증 담당 필터
/// - 인증 토큰을 서버에 전달 할 수 있도록 합니다.
/// - 토큰이 만료되었을 경우 토큰을 갱신하는 역활을 수행합니다.
/// </summary>
/// <param name="tokenRepository"></param>
/// <param name="channel"></param>
public sealed class WithAuthenticationFilter(IAuthenticationTokenRepository tokenRepository, GrpcChannel channel)
    : IClientFilter
{
    private const string Authorization = nameof(Authorization);

    public async ValueTask<ResponseContext> SendAsync(RequestContext context,
        Func<RequestContext, ValueTask<ResponseContext>> next)
    {
        try
        {
            // 토큰 만료 시
            if (tokenRepository.IsExpired)
            {
                // 토큰 갱신
                await RefreshTokenAsync();

                context.CallOptions.Headers?.Remove(new Metadata.Entry(Authorization, string.Empty));
            }

            // 헤더에 토큰 추가
            var authorizationEntry = context.CallOptions.Headers?.Get(Authorization);
            if (authorizationEntry == null)
            {
                context.CallOptions.Headers?.Add(new Metadata.Entry(Authorization,
                    $"Bearer {tokenRepository.AccessToken}"));
            }

            return await next(context);
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            throw;
        }
    }

    private async Task RefreshTokenAsync()
    {
        var rpcService = MagicOnionClient.Create<IAuthenticateRpcService>(channel);

        // 인증 Rpc Service에서 토큰 갱신
        var response = await rpcService.RefreshTokenAsync(tokenRepository.RefreshToken);
        if (!response.Success)
        {
            throw new Exception("Failed to refresh token");
        }

        tokenRepository.Update(response.AccessToken, response.RefreshToken, response.ExpiresIn);

        response.Print("Refresh Token Response");
    }
}