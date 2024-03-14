namespace Flashcards.Repositories;

public class PracticeSessionRepository
{
    private readonly FlaschardDatabase db = new();

    public void CreateSession(Session session)
    {
        using var connection = db.GetConnection();
        connection.Execute(
            "INSERT INTO Sessions (StackId, Score, SessionDate) VALUES (@StackId, @Score, @SessionDate);", session);
    }

    public List<int> GetLogYears()
    {
        using var connection = db.GetConnection();
        return connection.Query<int>("SELECT DISTINCT YEAR(SessionDate) FROM Sessions;").ToList();
    }

    public IEnumerable<dynamic> GetMonthlyAverageByYear(int year)
    {
        CheckYearHasSessions(year);

        var sql = $"""
                   DECLARE @months NVARCHAR(MAX), @sql NVARCHAR(MAX);
                     
                   -- Generate a list of month numbers for pivoting
                   -- Ensure uniqueness in the month numbers for pivoting
                   SELECT @months = STRING_AGG(QUOTENAME(SessionMonth), ',') WITHIN GROUP (ORDER BY SessionMonth)
                   FROM (SELECT DISTINCT MONTH(SessionDate) as SessionMonth FROM Sessions WHERE YEAR(SessionDate) = {year}) AS UniqueMonths;
                     -- FORMAT(SessionDate, 'MMM') AS
                   -- Debugging: Print the corrected months to check
                   PRINT @months;
                     
                   -- Construct dynamic SQL for pivot operation
                   SET @sql = N'SELECT s.Name, '+ @months +' FROM (
                                  SELECT StackId, Score, MONTH(SessionDate) as Month
                                  FROM Sessions WHERE YEAR(SessionDate) = {year} ) as SourceTable
                              PIVOT( AVG(Score) FOR Month IN (' + @months + ')
                                ) AS PivotTable
                              JOIN Stacks s ON PivotTable.StackId = s.Id;';
                     
                   -- Execute the constructed SQL
                   EXEC sp_executesql @sql;
                   """;

        using var connection = db.GetConnection();
        return connection.Query(sql);
    }

    private void CheckYearHasSessions(int year)
    {
        using var connection = db.GetConnection();
        var yearSessions =
            connection.ExecuteScalar<int>("select count(*) from Sessions where year(SessionDate) = @year",
                new { year });
        if (yearSessions == 0)
            throw new NotFoundException($"No sessions found for {year}.");
    }
}