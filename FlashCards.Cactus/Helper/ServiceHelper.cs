using FlashCards.Cactus.DataModel;
using FlashCards.Cactus.Service;
using Spectre.Console;

namespace FlashCards.Cactus.Helper;

public class ServiceHelper
{

    public static int GetUserInputId(string idStr, int count)
    {
        int inputId;
        while (!int.TryParse(idStr, out inputId) || inputId < 1 || inputId > count)
        {
            idStr = AnsiConsole.Ask<string>($"Please input a valid id. Type 'q' to quit.");
            if (idStr == "q")
            {
                inputId = -1;
                break;
            }
        }
        return inputId;
    }
    public static void ShowDataRecords(string name, string title, List<List<string>> rows)
    {
        if (rows.Count == 0)
        {
            Console.WriteLine($"No {name} exists.");
            return;
        }

        var table = new Table();
        table.Title(title);
        table.Border(TableBorder.Square);
        table.Collapse();
        if (name.Equals(Constants.FLASHCARD))
        {
            int id = 0;
            table.AddColumn(nameof(FlashCard.Id));
            table.AddColumn(new TableColumn(nameof(FlashCard.Front)).Centered());
            table.AddColumn(new TableColumn(nameof(FlashCard.Back)).Centered());
            rows.ForEach(row => { table.AddRow((++id).ToString(), row[0], row[1]); });
        }
        else if (name.Equals(Constants.STACK))
        {
            table.AddColumn(new TableColumn(nameof(Stack.Name)).Centered());
            rows.ForEach(row => { table.AddRow(row[0]); });
        }
        else if (name.Equals(Constants.STUDYSESSION))
        {
            int id = 0;
            table.AddColumn(nameof(StudySession.Id));
            table.AddColumn(new TableColumn(nameof(StudySession.StackName)).Centered());
            table.AddColumn(new TableColumn(nameof(StudySession.Date)).Centered());
            table.AddColumn(new TableColumn(nameof(StudySession.Time)).Centered());
            table.AddColumn(new TableColumn(nameof(StudySession.Score)).Centered());
            rows.ForEach(row => { table.AddRow((++id).ToString(), row[0], row[1], row[2], row[3]); });
        }
        AnsiConsole.Write(table);
    }
}
