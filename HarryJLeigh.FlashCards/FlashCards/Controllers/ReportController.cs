using System.Data.SqlClient;
using Dapper;
using FlashCards.Data;

namespace FlashCards.Controllers;

public class ReportController
{
    private DatabaseService DbService { get; }

    public ReportController(DatabaseService dbService) => DbService = dbService;

    public IEnumerable<dynamic>? GetSessionReportData(string year, int stack_id)
    {
        var query = @"SELECT 
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
                          FROM (
                                SELECT 
                                    MONTH(TRY_CAST(studyDate AS DATE)) AS Month,
                                    COUNT(*) AS TotalSessions
                                FROM Study
                                WHERE YEAR(TRY_CAST(studyDate AS DATE)) = @Year AND stack_id = @Stack_id
                                        AND TRY_CAST(studyDate AS DATE) IS NOT NULL
                                GROUP BY MONTH(TRY_CAST(studyDate AS DATE))
                            ) AS SourceTable
                            PIVOT (
                                SUM(TotalSessions) FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                            ) AS PivotTable;
";
        var parameters = new { Year = year, Stack_id = stack_id };
        return ExecuteSessionsQuery(query, parameters);
    }
    
    public IEnumerable<dynamic>? GetAverageReportData(string year, int stack_id)
    {
        var query = @"
            SELECT 
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
FROM (
    SELECT 
        MONTH(TRY_CAST(studyDate AS DATE)) AS Month, 
        AVG(score) AS AverageScore
    FROM Study
    WHERE YEAR(TRY_CAST(studyDate AS DATE)) = @Year AND stack_id = @Stack_id
        AND TRY_CAST(studyDate AS DATE) IS NOT NULL
    GROUP BY MONTH(TRY_CAST(studyDate AS DATE))
) AS SourceTable
PIVOT (
    AVG(AverageScore) FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
) AS PivotTable;";

        var parameters = new { Year = year, Stack_id = stack_id };
        return ExecuteAverageQuery(query, parameters);
    }

    private IEnumerable<dynamic>? ExecuteSessionsQuery(string query, object? parameters = null)
    {
        using SqlConnection connection = DbService.GetConnection();
        return connection.Query<dynamic>(query, parameters);
    }

    private IEnumerable<dynamic>? ExecuteAverageQuery(string query, object? parameters = null)
    {
        using SqlConnection connection = DbService.GetConnection();
        return connection.Query<dynamic>(query, parameters);
    }
}