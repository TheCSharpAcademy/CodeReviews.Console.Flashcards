using Spectre.Console;

class DisplayOther
{
    public static void DisplayStacks(List<Stack> dataSet)
    {
        var table = new Table();

        table.AddColumns(["Id", "Name"]);
        foreach(Stack data in dataSet)
        {
            table.AddRow(data.Id.ToString(), data.Name);
        }
        AnsiConsole.Write(table);
    }

    public static void DisplayCards(List<FlashcardDTO> dataSet)
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