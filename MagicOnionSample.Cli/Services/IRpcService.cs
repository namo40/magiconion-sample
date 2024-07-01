using MagicOnion;

namespace MagicOnionSample.Cli.Services;

public interface IRpcService
{
    T Create<T>() where T : IService<T>;
    T CreateWithAuthentication<T>() where T : IService<T>;
}