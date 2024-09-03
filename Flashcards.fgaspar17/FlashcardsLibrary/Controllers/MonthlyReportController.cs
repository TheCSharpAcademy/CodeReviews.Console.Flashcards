using CodingTrackerLibrary;

namespace FlashcardsLibrary;
public class MonthlyReportController
{
    public static List<MonthlyReport> GetSessionsPerMonthReport(int year)
    {
        string sql = @$"SELECT 
                        StackName, 
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
                        (SELECT 
                             S.Name AS StackName, 
                             MONTH(SS.SessionDate) AS SessionMonth 
                         FROM 
                             StudySessions SS
                         LEFT JOIN 
                             Stacks S ON SS.StackId = S.StackId
                         WHERE YEAR(SS.SessionDate) = {year}) AS Derived
                    PIVOT
                        (COUNT(SessionMonth) FOR SessionMonth IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])) AS Pivoted;";
        return SqlExecutionService.GetListModels<MonthlyReport>(sql);
    }

    public static List<MonthlyReport> GetAverageScorePerMonthReport(int year)
    {
        string sql = @$"SELECT 
                        StackName, 
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
                        (SELECT S.Name AS StackName, MONTH(SS.SessionDate) AS SessionMonth, CAST(CAST(SUM(CASE WHEN SQ.IsCorrect = 1 THEN 1 ELSE 0 END) AS NUMERIC(14,2))
						                        /
                                                COUNT(*) * 100 AS INT) AS Score 
                        FROM StudySessions SS
                        LEFT JOIN Stacks S ON SS.StackId = S.StackId
                        LEFT JOIN SessionQuestion SQ ON SS.SessionId = SQ.SessionId
                        WHERE YEAR(SS.SessionDate) = {year}
                        GROUP BY S.Name, MONTH(SS.SessionDate)) AS Derived
                        PIVOT(AVG(Score) FOR SessionMonth IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])) AS Pivoted;";

        return SqlExecutionService.GetListModels<MonthlyReport>(sql);
    }
}