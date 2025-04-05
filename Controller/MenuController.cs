using Spectre.Console;
abstract class MenuController
{
    public async Task StartAsync()
    {
        bool exit = false;

        await OnReady(); // Optional work that occurs once before loop
        while (!exit)
        {
            Console.Clear();
            await MainAsync(); // Work that occurs before each loop

            exit = await HandleMenuAsync(); // Menu handling

            if (!exit)
            {
                AnsiConsole.Markup("[bold green]Press Enter to continue. [/]");
                Console.Read();
            }
        }
    }
    protected virtual Task OnReady()
    {
        return Task.CompletedTask;
    }
    protected abstract Task MainAsync();
    protected abstract Task<bool> HandleMenuAsync();
}