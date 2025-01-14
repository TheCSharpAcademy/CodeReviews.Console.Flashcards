using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

internal class SessionsGenerator
{
    internal static void GenerateRandomSessions()
    {
        Console.Clear();
        var stacks = StackRead.GetStackList();
        if (stacks.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No stack names found in database.[/]");
            AnsiConsole.MarkupLine("Create at least one first.");
            DisplayInfoHelpers.PressAnyKeyToContinue();
            return;
        }

        var numberOfSessions = InputHelpers.GetPositiveNumberInput(
            "Enter a number of random sessions to generate:");

        int numberOfDays;
        if (numberOfSessions >= 5000) numberOfDays = Random.Shared.Next(2500, 5000);
        else if (numberOfSessions >= 3000) numberOfDays = Random.Shared.Next(1000, 2500);
        else if (numberOfSessions >= 2000) numberOfDays = Random.Shared.Next(500, 1000);
        else if (numberOfSessions >= 1000) numberOfDays = Random.Shared.Next(300, 500);
        else if (numberOfSessions >= 300) numberOfDays = Random.Shared.Next(100, 300);
        else numberOfDays = Random.Shared.Next(30, 100);

        var sessions = GenerateRandomSessions(stacks, numberOfSessions, numberOfDays);

        PopulateDatabaseWithSessions(sessions);

        string session = (numberOfSessions == 1) ? "session" : "sessions";
        AnsiConsole.MarkupLine($"[green]New {numberOfSessions} {session} created successfully![/]");
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }

    private static void PopulateDatabaseWithSessions(List<Session> sessions)
    {
        using var connection = new SqlConnection(Config.ConnectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            var parameters = new DynamicParameters();
            foreach (var session in sessions)
            {
                parameters.Add("@StackId", session.StackId);
                parameters.Add("@StackName", session.StackName);
                parameters.Add("@SessionDate", session.SessionDate);
                parameters.Add("@SessionScore", session.SessionScore);
                connection.Execute(@"
                    INSERT INTO Session (StackId, StackName, SessionDate, SessionScore)
                    VALUES (@StackId, @StackName, @SessionDate, @SessionScore)",
                    parameters, transaction);
            }
            transaction.Commit();
        }
        catch (SqlException ex)
        {
            transaction.Rollback();
            DisplayErrorHelpers.SqlError(ex);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            DisplayErrorHelpers.GeneralError(ex);
        }
    }

    private static List<Session> GenerateRandomSessions(
        List<Stack> stacks, int numberOfSessions, int numberOfDays)
    {
        var sessions = new List<Session>();
        for (int i = 0; i < numberOfSessions; i++)
        {
            var stack = stacks[Random.Shared.Next(stacks.Count)];
            var date = DateTime.Now.AddDays(Random.Shared.Next(-numberOfDays, 0));
            var score = Random.Shared.Next(0, 6);

            sessions.Add( new Session
            {
                StackId = stack.StackId,
                StackName = stack.StackName,
                SessionDate = date,
                SessionScore = score
            });
        }

        sessions = sessions.OrderBy(s => s.SessionDate).ToList();
        return sessions;
    }
}
