
using System.Data;
using Spectre.Console;

class DisplayData
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

    public static void Table(List<Stack> dataSet)
    {
        var table = new Table();

        table.AddColumns(["Id", "Name"]);
        foreach(Stack data in dataSet)
        {
            table.AddRow(data.Id.ToString(), data.Name);
        }
        AnsiConsole.Write(table);
    }

    public static void Table(List<FlashcardDTO> dataSet, string[] header)
    {
        var table = new Table();

        table.AddColumns(["Id", "Front", "Back"]);
        foreach(FlashcardDTO data in dataSet)
        {
            table.AddRow(data.Id.ToString(), data.Front, data.Back);
        }
        AnsiConsole.Write(table);
    }

    
}