
using System.Data;
using Spectre.Console;

class DisplayData
{
    public static string Selection<T>(List<T> dataSet)
    {
        var prompt = new SelectionPrompt<string>()
            .Title("[bold green]Select[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more data)[/]");

        foreach(T data in dataSet)
        {
            switch (data)
            {
                case Stack stack:
                    prompt.AddChoice(stack.Name);
                    break;
                case FlashcardDTO flashcard:
                    prompt.AddChoice(flashcard.Front + " " + flashcard.Back);
                    break;
                default:
                    AnsiConsole.MarkupLine("[bold red]Displaying undefined type[/]");
                    break;
            }
        }

        return AnsiConsole.Prompt(prompt);
    }

    public static void Table<T>(List<T> dataSet, string[] header)
    {
        var table = new Table();

        table.AddColumns(header);
        foreach(T data in dataSet)
        {
            switch (data)
            {
                case Stack stack:
                    table.AddRow(stack.Id.ToString(), stack.Name);
                    break;
                case FlashcardDTO flashcard:
                    table.AddRow(flashcard.Id.ToString(), flashcard.Front, flashcard.Back);
                    break;
                default:
                    AnsiConsole.MarkupLine("[bold red]Displaying undefined type[/]");
                    break;
            }
        }
        AnsiConsole.Write(table);
    }
}