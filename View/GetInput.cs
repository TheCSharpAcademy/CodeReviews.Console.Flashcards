using Spectre.Console;
class GetInput
{
    public static string StackName()
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("[bold green]New name for Stack: [/]"));
    }
}