using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

internal class StackRead
{
    internal static void ShowAllStacks()
    {
        Console.Clear();
        var stacks = GetStackList();
        if (DisplayInfoHelpers.NoRecordsAvailable(stacks)) return;

        AnsiConsole.MarkupLine($"List of available stacks in database:\n");
        var table = new Table();
        int num = 1;
        table.AddColumn("No.");
        table.AddColumn("Name");
        foreach (var stack in stacks)
        {
            table.AddRow(
                new Markup($"{num}"),
                new Markup($"[yellow]{stack.StackName}[/]"));
            num++;
        }
        AnsiConsole.Write(table);
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }

    private static Dictionary<string, Stack> MakeStackMap()
    {
        var stacks = GetStackList();
        var stackList = MakeStackList(stacks);
        var stackMap = new Dictionary<string, Stack>();

        for (int i = 0; i < stacks.Count; i++)
        {
            stackMap.Add(stackList[i], stacks[i]);
        }
        return stackMap;
    }

    private static List<string> MakeStackList(List<Stack> stacks)
    {
        var tableData = new List<string>();
        foreach (var stack in stacks)
        {
            tableData.Add($"{stack.StackName}");
        }
        return tableData;
    }

    internal static List<Stack> GetStackList()
    {
        try
        {
            using var connection = new SqlConnection(Config.ConnectionString);
            connection.Open();
            var query = @$"
                SELECT
                    StackId AS StackId,
                    StackName AS StackName
                FROM Stack
                ORDER BY StackName";
            return connection.Query<Stack>(query).ToList();
        }
        catch (SqlException ex)
        {
            DisplayErrorHelpers.SqlError(ex);
            return [];
        }
        catch (Exception ex)
        {
            DisplayErrorHelpers.GeneralError(ex);
            return [];
        }
    }

    internal static Stack GetStack()
    {
        var stackMap = MakeStackMap();
        if (DisplayInfoHelpers.NoRecordsAvailable(stackMap.Keys)) return new Stack();

        var choice = DisplayInfoHelpers.GetChoiceFromSelectionPrompt(
            "Choose stack:", stackMap.Keys);
        if (choice == DisplayInfoHelpers.Back) return new Stack();

        var success = stackMap.TryGetValue(choice, out Stack chosenStack);
        if (!success) return new Stack();

        return chosenStack;
    }
}
