using ConsoleAppFramework;
using MagicOnionSample.Cli.Extensions;
using MagicOnionSample.Cli.Repositories;
using MagicOnionSample.Cli.Services;
using MagicOnionSample.ClientShared.RpcServices;
using Spectre.Console;

namespace MagicOnionSample.Cli.Commands;

public class AccountCommands(
    IAuthenticationTokenRepository authenticationTokenRepository,
    IRpcService rpcService)
{
    /// <summary>
    /// 로그인
    /// </summary>
    [Command("login")]
    public async Task LoginAsync()
    {
        var authenticateRpcService = rpcService.Create<IAuthenticateRpcService>();

        var userId = AnsiConsole.Ask<long>("Enter your user ID ([green]number[/])?");
        var displayName = AnsiConsole.Ask<string>("Enter your display name ([green]string[/])?");

        await MethodCallAsync("Authenticating...", async () =>
        {
            var response = await authenticateRpcService.AuthenticateAsync(userId, displayName);
            if (!response.Success)
            {
                AnsiConsole.MarkupLine("[red]Failed to authenticate[/]");
                return;
            }

            authenticationTokenRepository.Update(response.AccessToken, response.RefreshToken, response.ExpiresIn);

            response.Print("Login Response");
        });
    }

    /// <summary>
    /// 인증을 사용하여, 현재 사용자의 Display Name을 출력합니다.
    /// </summary>
    [Command("display-name")]
    public async Task DisplayNameAsync()
    {
        var authenticateRpcService = rpcService.CreateWithAuthentication<IAuthenticateRpcService>();

        await MethodCallAsync("Getting display name...", async () =>
        {
            var displayName = await authenticateRpcService.GetDisplayNameAsync();
            AnsiConsole.MarkupLine($"[green]Display Name:[/] {displayName}");
        });
    }

    private static Task MethodCallAsync(string title, Func<Task> func)
        => AnsiConsole.Status().Spinner(Spinner.Known.Star).StartAsync(title, _ => func());
}