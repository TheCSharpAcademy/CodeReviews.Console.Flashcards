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

    public static Tuple<int, string> AskUserToTypeAStackName(List<Stack> stacks)
    {
        // Show available stack names.
        List<string> stackNames = stacks.Select(s => s.Name).ToList();
        List<List<string>> rows = new List<List<string>>();
        stackNames.ForEach(name => rows.Add(new List<string>() { name }));
        ShowDataRecords(Constants.STACK, Constants.STACKS, rows);


        string stackName = AnsiConsole.Ask<string>("Please input the [green]name[/] of the Stack where you want to start. Type 'q' to quit.");
        if (stackName.Equals(Constants.QUIT)) return new Tuple<int, string>(-1, "");
        while (!stackNames.Contains(stackName))
        {
            stackName = AnsiConsole.Ask<string>($"[red]{stackName}[/] Stack dose not exist. Please input a valid Stack name. Type 'q' to quit.");
            if (stackName.Equals(Constants.QUIT)) return new Tuple<int, string>(-1, "");
        }
        int sid = stacks.Where(s => s.Name.Equals(stackName)).ToArray()[0].Id;

        return new Tuple<int, string>(sid, stackName);
    }
}
