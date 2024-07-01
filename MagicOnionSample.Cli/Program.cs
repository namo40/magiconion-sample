using ConsoleAppFramework;
using MagicOnionSample.Cli.Commands;
using MagicOnionSample.Cli.Repositories;
using MagicOnionSample.Cli.Services;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

var services = new ServiceCollection();

services.AddSingleton<IAuthenticationTokenRepository, AuthenticationTokenRepository>();
services.AddSingleton<IRpcService, RpcService>();

ConsoleApp.ServiceProvider = services.BuildServiceProvider();

var app = ConsoleApp.Create();

app.Add<HelloWorldCommands>();
app.Add<AccountCommands>("account");

var isRunning = true;

app.Add("exit", () => isRunning = false);

while (isRunning)
{
    var readLine = AnsiConsole.Ask<string>("MagicOnionSample.Cli> ");
    if (string.IsNullOrEmpty(readLine))
        continue;

    await app.RunAsync(readLine.Split(" "));
}