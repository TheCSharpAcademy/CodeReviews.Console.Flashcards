using Spectre.Console;
class GetInput
{
    public static Stack Selection(List<Stack> dataSet)
    {
        var prompt = new SelectionPrompt<Stack>()
            .Title("[bold green]Select[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more data)[/]");
        
        foreach (var data in dataSet)
        {
            prompt.AddChoice(data);
        }

        prompt.Converter = data => $"{data.Id} {data.Name}";

        return AnsiConsole.Prompt(prompt);
    }

    public static FlashcardDTO Selection(List<FlashcardDTO> dataSet)
    {
        var prompt = new SelectionPrompt<FlashcardDTO>()
            .Title("[bold green]Select[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more data)[/]");
        
        foreach (var data in dataSet)
        {
            prompt.AddChoice(data);
        }

        prompt.Converter = data => $"{data.Id} {data.Front} {data.Back}";

        return AnsiConsole.Prompt(prompt);
    }
    public static string StackName()
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("[bold green]New name for Stack: [/]"));
    }
}