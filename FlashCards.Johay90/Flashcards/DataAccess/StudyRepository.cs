using Dapper;

public class StudyRepository
{
    private DatabaseManager _databaseManager;

    public StudyRepository(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public void AddStudy(StudySession studySession)
    {

        using (var conn = _databaseManager.GetConnection())
        {
            var query = "INSERT INTO StudySessions (StackId, Date, Score, TotalQuestions) VALUES (@StackId, @Date, @Score, @TotalQuestions)";
            conn.Execute(query, studySession);
        }
    }

    public List<StudySession> GetStudySessions()
    {
        using (var conn = _databaseManager.GetConnection())
        {
            var query = "SELECT * FROM StudySessions";
            return conn.Query<StudySession>(query).ToList();
        }
    }

    public List<MonthlySessionsPivot> GetMonthlyReports(int year)
    {
        using (var conn = _databaseManager.GetConnection())
        {
            var query = @"SELECT 
                            StackId,
                            Year,
                            [1] AS January,
                            [2] AS February,
                            [3] AS March,
                            [4] AS April,
                            [5] AS May,
                            [6] AS June,
                            [7] AS July,
                            [8] AS August,
                            [9] AS September,
                            [10] AS October,
                            [11] AS November,
                            [12] AS December
                        FROM
                            (
                                SELECT 
                                    StackId, 
                                    YEAR(Date) AS Year, 
                                    MONTH(Date) AS Month, 
                                    COUNT(Id) AS SessionCount
                                FROM StudySessions
                                WHERE YEAR(Date) = @Year
                                GROUP BY StackId, YEAR(Date), MONTH(Date)
                            ) AS SourceTable
                        PIVOT
                        (
                            SUM(SessionCount)
                            FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                        ) AS PivotTable
                        ORDER BY StackId, Year;
                        ";
            return conn.Query<MonthlySessionsPivot>(query, new { Year = year }).ToList();
        }
    }
}
