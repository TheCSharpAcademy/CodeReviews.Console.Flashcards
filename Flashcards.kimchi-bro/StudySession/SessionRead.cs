using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

internal class SessionRead
{
    internal static void ShowAllSessions()
    {
        Console.Clear();
        var sessions = GetSessionList();
        if (DisplayInfoHelpers.NoRecordsAvailable(sessions)) return;

        AnsiConsole.MarkupLine($"List of all study sessions:");

        var table = new Table();
        int num = 1;
        table.AddColumn("No.");
        table.AddColumn("Stack Name");
        table.AddColumn("Date");
        table.AddColumn("Score");
        foreach (var session in sessions)
        {
            table.AddRow(
                new Markup($"{num}"),
                new Markup($"[yellow]{session.SessionDate:yyyy-MM-dd}[/]"),
                new Markup($"[yellow]{session.StackName}[/]"),
                new Markup($"[yellow]{session.SessionScore}[/]"));
            num++;
        }
        AnsiConsole.Write(table);

        AnsiConsole.Markup("\n[yellow]Press any key to continue...[/] ");
        Console.ReadKey(true);
        Console.Clear();
    }

    private static List<Session> GetSessionList()
    {
        try
        {
            using var connection = new SqlConnection(Config.ConnectionString);
            connection.Open();
            var query = @$"
                SELECT
                    SessionId AS SessionId,
                    StackName AS StackName,
                    SessionDate AS SessionDate,
                    SessionScore AS SessionScore
                FROM Session
                ORDER BY SessionDate";
            return connection.Query<Session>(query).ToList();
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
}
