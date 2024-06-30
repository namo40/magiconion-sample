using ConsoleAppFramework;

var app = ConsoleApp.Create();

var isRunning = true;

app.Add("exit", () => isRunning = false);

while (isRunning)
{
    Console.Write("MagicOnionSample.Cli> ");
    var readLine = Console.ReadLine();
    if (string.IsNullOrEmpty(readLine))
        continue;

    await app.RunAsync(readLine.Split(" "));
}