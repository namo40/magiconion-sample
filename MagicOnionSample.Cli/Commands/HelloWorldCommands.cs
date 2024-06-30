using ConsoleAppFramework;
using Grpc.Net.Client;
using MagicOnion.Client;
using MagicOnionSample.ClientShared.RpcServices;

namespace MagicOnionSample.Cli.Commands;

public class HelloWorldCommands
{
    [Command("hello")]
    public async Task HelloAsync()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:5000");

        var helloWorldRpcService = MagicOnionClient.Create<IHelloWorldRpcService>(channel);

        var response = await helloWorldRpcService.SayHelloAsync();

        Console.WriteLine($"Response: {response}");
    }
}