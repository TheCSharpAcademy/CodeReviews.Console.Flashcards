using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using System.Globalization;

internal class QuickReport
{
    internal static void ShowReportTables()
    {
        var allSessionsData = GetSessionsData();
        if (DisplayInfoHelpers.NoRecordsAvailable(allSessionsData)) return;

        var sessionDataMap = new Dictionary<string, List<SessionsData>>();

        foreach (var sessionDataDto in allSessionsData)
        {
            if (!sessionDataMap.TryGetValue($"{sessionDataDto.StackName}", out var sessions))
            {
                sessions = new List<SessionsData>();
                sessionDataMap[$"{sessionDataDto.StackName}"] = sessions;
            }
            sessions.Add(sessionDataDto);
        }

        var years = sessionDataMap.Values
            .SelectMany(s => s)
            .Select(s => s.SessionYear)
            .Distinct()
            .OrderBy(y => y);

        foreach (var year in years)
        {
            var table = new Table();
            table.AddColumn("Stack Name");

            for (int month = 1; month <= 12; month++)
            {
                table.AddColumn(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[month - 1]);
            }

            foreach (var sessionData in sessionDataMap)
            {
                List<string> row = [sessionData.Key];
                for (int month = 1; month <= 12; month++)
                {
                    var sessionDataDto = sessionData.Value
                        .Where(s => s.SessionYear == year && s.SessionMonth == month)
                        .FirstOrDefault();

                    if (sessionDataDto != null)
                    {
                        row.Add($"[yellow]{sessionDataDto.NumberOfSessions}[/]-([green]{sessionDataDto.AverageScore}[/])");
                    }
                    else
                    {
                        row.Add("");
                    }
                }
                table.AddRow(row.ToArray());
            }

            AnsiConsole.MarkupLine($"Sessions data for [yellow]{year}[/] year in format: " +
                $"[yellow]Total sessions[/]-([green]Average score[/]):\n");

            AnsiConsole.Write(table);

            DisplayInfoHelpers.PressAnyKeyToContinue();
        }
    }

    private static List<SessionsData> GetSessionsData()
    {
        try
        {
            using var connection = new SqlConnection(Config.ConnectionString);
            connection.Open();
            var results = connection.Query<SessionsData>(@"
                SELECT
                    StackName,
                    YEAR(SessionDate) AS SessionYear,
                    MONTH(SessionDate) AS SessionMonth,
                    COUNT(SessionId) AS NumberOfSessions,
                    AVG(SessionScore) AS AverageScore
                FROM
                    Session
                GROUP BY
                    StackName,
                    YEAR(SessionDate),
                    MONTH(SessionDate)
                ORDER BY 
                    StackName");
            return results.ToList();
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
