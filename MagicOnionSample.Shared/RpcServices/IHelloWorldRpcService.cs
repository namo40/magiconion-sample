using MagicOnion;

namespace MagicOnionSample.ClientShared.RpcServices
{
    public interface IHelloWorldRpcService : IService<IHelloWorldRpcService>
    {
        UnaryResult<string> SayHelloAsync();
    }
}