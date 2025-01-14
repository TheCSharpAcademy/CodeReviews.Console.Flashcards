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

        //Map stack names to lists of SessionDataDto
        var sessionDataMap = new Dictionary<string, List<SessionsData>>();

        foreach (var sessionDataDto in allSessionsData)
        {
            //Check if sessionDataByStackName doesn't have StackName key 
            if (!sessionDataMap.TryGetValue($"{sessionDataDto.StackName}", out var sessions))
            {
                //If it doesn't =>
                //Create new List<SessionDataDto>
                sessions = new List<SessionsData>();
                //Add new list to the dictionary with the StackName as the key
                sessionDataMap[$"{sessionDataDto.StackName}"] = sessions;
            }
            //If StackName as key already exists
            sessions.Add(sessionDataDto);
        }

        var years = sessionDataMap.Values //get ordered ienumerable<int> for years
            .SelectMany(s => s)
            .Select(s => s.SessionYear)
            .Distinct()
            .OrderBy(y => y);

        foreach (var year in years)
        {
            var table = new Table();
            table.AddColumn("Stack Name"); //first column

            //+12 columns
            for (int month = 1; month <= 12; month++)
            {
                table.AddColumn(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[month - 1]);
            }

            foreach (var sessionData in sessionDataMap)
            {
                List<string> row = [sessionData.Key]; //stack name as first item in first column

                //for other 12 columns (months) =>
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
