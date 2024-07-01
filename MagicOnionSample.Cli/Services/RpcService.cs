using Grpc.Net.Client;
using MagicOnion;
using MagicOnion.Client;
using MagicOnionSample.Cli.ClientFilters;
using MagicOnionSample.Cli.Repositories;

namespace MagicOnionSample.Cli.Services;

public class RpcService(IAuthenticationTokenRepository tokenRepository) : IRpcService
{
    public GrpcChannel Channel { get; } = GrpcChannel.ForAddress("http://localhost:5000");

    public T Create<T>() where T : IService<T>
        => MagicOnionClient.Create<T>(Channel);

    // WithAuthenticationFilter를 통해 인증 토큰을 전달
    public T CreateWithAuthentication<T>() where T : IService<T>
        => MagicOnionClient.Create<T>(Channel, [new WithAuthenticationFilter(tokenRepository, Channel)]);
}