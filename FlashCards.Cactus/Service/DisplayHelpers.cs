using FlashCards.Cactus.DataModel;
using Spectre.Console;

namespace FlashCards.Cactus.Service;

public class DisplayHelpers
{
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
            int id = 0;
            table.AddColumn(nameof(Stack.Id));
            table.AddColumn(new TableColumn(nameof(Stack.Name)).Centered());
            rows.ForEach(row => { table.AddRow((++id).ToString(), row[0]); });
        }
        AnsiConsole.Write(table);
    }
}
