
using System.Data;
using Spectre.Console;

class DisplayData
{

    public static void Table(List<FlashcardDTO> dataSet, string title = "")
    {
        var table = new Table();

        table.Title(title);
        table.AddColumns(["Id", "Front", "Back"]);


        if (dataSet != null)
        {
            foreach(FlashcardDTO data in dataSet)
            {
                table.AddRow(data.Id.ToString(), data.Front, data.Back);
            }
        }
        
        AnsiConsole.Write(table);
    }

    public static void Table(List<Stack> dataSet, string title = "")
    {
        var table = new Table();

        table.Title(title);
        table.AddColumns([ "Name"]);

        if (dataSet != null)
        {
            foreach(Stack data in dataSet)
            {
                table.AddRow(data.Name);
            }
        }
        AnsiConsole.Write(table);
    }

    public static void Table(List<StudySession> dataSet, string title = "")
    {
        var table = new Table();

        table.Title(title);
        table.AddColumns(["Date", "Score"]);

        if (dataSet != null)
        {
            foreach(StudySession data in dataSet)
            {
                table.AddRow(data.Date.ToString(), data.Score.ToString());
            }
        }
        AnsiConsole.Write(table);
    }

    public static void CardForStudy(FlashcardDTO flashcard)
    {
        Panel panel = new(flashcard.Front);
        AnsiConsole.Write(panel);
    }
}