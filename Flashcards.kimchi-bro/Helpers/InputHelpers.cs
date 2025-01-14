using Spectre.Console;

internal class InputHelpers
{
    internal static string StringLengthCheck(int limit, string message)
    {
        var input = AnsiConsole.Ask<string>(message);
        while (input.Length > limit)
        {
            AnsiConsole.MarkupLine($"[red]Input can not be longer {limit} characters![/]");
            input = AnsiConsole.Ask<string>("Retry:");
        }
        return input;
    }

    internal static int GetPositiveNumberInput(string message)
    {
        var input = AnsiConsole.Ask<int>(message);
        while (input <= 0)
        {
            AnsiConsole.Markup("[red]Invalid input. Only positive numbers accepted.[/]\n");
            input = AnsiConsole.Ask<int>("Enter a valid number:");
        }
        return input;
    }
}
