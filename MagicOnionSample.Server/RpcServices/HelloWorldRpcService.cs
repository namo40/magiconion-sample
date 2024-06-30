using MagicOnion;
using MagicOnion.Server;
using MagicOnionSample.ClientShared.RpcServices;

namespace MagicOnionSample.Server.RpcServices;

public sealed class HelloWorldRpcService : ServiceBase<IHelloWorldRpcService>, IHelloWorldRpcService
{
    public UnaryResult<string> SayHelloAsync()
    {
        return UnaryResult.FromResult("Hello World!");
    }
}