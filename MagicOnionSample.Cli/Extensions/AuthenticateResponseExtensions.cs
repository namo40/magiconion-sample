using MagicOnionSample.ClientShared.Models;
using Spectre.Console;

namespace MagicOnionSample.Cli.Extensions;

public static class AuthenticateResponseExtensions
{
    public static void Print(this AuthenticateResponse response, string title)
    {
        var table = new Table();
        table.Title($"[green]{title}[/]");
        table.AddColumn(nameof(response.AccessToken));
        table.AddColumn(nameof(response.RefreshToken));
        table.AddColumn(nameof(response.ExpiresIn));

        table.AddRow(response.AccessToken, response.RefreshToken, response.ExpiresIn.ToString());

        AnsiConsole.Write(table);
    }
}