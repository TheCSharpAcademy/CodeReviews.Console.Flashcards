using Spectre.Console;

class DisplayOther
{
    public static void DisplayTable<T>(List<T> dataSet)
    {
        var table = new Table();

        table.AddColumns(["Id", "Name"]);
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