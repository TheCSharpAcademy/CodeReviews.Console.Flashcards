using Dapper;
using Flashcards.glaxxie.DTO;

namespace Flashcards.glaxxie.Controllers;

internal class SessionController
{
    internal static void Insert(SessionCreation session)
    {
        string cmdStr = $"INSERT INTO {Tables.Sessions} (date, cards, score, stack_id) VALUES (@Date, @Cards, @Score, @StackId)";
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        conn.Execute(cmdStr, new { session.Date, session.Cards, session.Score, session.StackId });
    }

    internal static List<int> GetYears()
    {
        string cmdStr = $@"
        SELECT DISTINCT YEAR([date]) AS [Year]
            FROM {Tables.Sessions}
        ORDER BY [Year]
        ";
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        return [.. conn.Query<int>(cmdStr)];
    }

    internal static List<object> GetAverageScore(int year)
    {
        var months = Enumerable.Range(1, 12).Select(m => $"[{year}-{m:D2}]").ToList();
        string monthColumns = string.Join(", ", months);

        using var conn = DatabaseController.GetConnection();
        conn.Open();

        string cmdStr = $@"
        SELECT *
        FROM (
            SELECT
                st.stack_name,
                FORMAT(s.date, 'yyyy-MM') AS Month,
                s.score
            FROM Sessions s
            JOIN Stacks st ON s.stack_id = st.stack_id
            WHERE YEAR(s.date) = @year
        ) AS source
        PIVOT (
            AVG(score)
            FOR Month IN ({monthColumns})
        ) AS Pivoted";

        return [..conn.Query(cmdStr , new{ year})];
    }

    internal static List<object> GetSessionCountsByStack(int year)
    {
        var months = Enumerable.Range(1, 12).Select(m => $"[{year}-{m:D2}]").ToList();
        string monthColumns = string.Join(", ", months);

        using var conn = DatabaseController.GetConnection();
        conn.Open();

        string cmdStr = $@"
        SELECT *
        FROM (
            SELECT
                s.stack_id,
                st.stack_name,
                FORMAT(s.date, 'yyyy-MM') AS Month
            FROM Sessions s
            JOIN Stacks st ON s.stack_id = st.stack_id
            WHERE YEAR(s.date) = @year
        ) AS source
        PIVOT (
            COUNT(stack_id)
            FOR Month IN ({monthColumns})
        ) AS Pivoted";

        return [.. conn.Query(cmdStr, new { year })];
    }
}