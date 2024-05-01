using Flashcards.Database;
using Flashcards.Models;
using Flashcards.DTO;
using Flashcards.Services;
using Dapper;
using Spectre.Console;

namespace Flashcards.DAO;

public class StudySessionDao
{
    private readonly DatabaseContext _dbContext;

    public StudySessionDao(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void InsertNewStudySession(StudySessionDto studySession)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"INSERT INTO {ConfigSettings.tbStudySessionsName} (StackID, SessionDate, Score) VALUES (@StackID, @SessionDate, @Score)";
                dbConnection.Execute(sql, new { studySession.StackID, studySession.SessionDate, studySession.Score });
            }
        }
        catch
        {
            throw;
        }
    }

    public bool DeleteStudySession(StudySession studySession)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"DELETE FROM {ConfigSettings.tbStudySessionsName} WHERE SessionID = @SessionID";
                dbConnection.Execute(sql, new { studySession.SessionID });
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public IEnumerable<dynamic>? GetStudySessionReportData(int year)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $@"
                    DECLARE @Year int = @SelectedYear;

                    ;WITH MonthlySessions AS (
                        SELECT
                            StackName,
                            MONTH(SessionDate) AS SessionMonth,
                            COUNT(*) AS SessionCount
                        FROM
                            vw_StudySessionsWithStacks
                        WHERE
                            YEAR(SessionDate) = @Year
                        GROUP BY
                            StackName, MONTH(SessionDate)
                    )

                    SELECT
                        StackName,
                        [1] AS Jan, [2] AS Feb, [3] AS Mar, [4] AS Apr, [5] AS May, [6] AS Jun,
                        [7] AS Jul, [8] AS Aug, [9] AS Sep, [10] AS Oct, [11] AS Nov, [12] AS Dec
                    FROM
                        MonthlySessions
                    PIVOT
                    (
                        SUM(SessionCount)
                        FOR SessionMonth IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                    ) AS PivotTable;
                    ";
                
                var result = dbConnection.Query(sql, new { SelectedYear = year });
                return result;
            }
        }
        catch
        {
            throw;
        }
    }

    public IEnumerable<dynamic>? GetAverageScoreReportData(int year)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $@"
                DECLARE @Year int = @SelectedYear;

                ;WITH MonthlyAverages AS (
                    SELECT
                        StackName,
                        MONTH(SessionDate) AS SessionMonth,
                        AVG(CAST(Score AS FLOAT)) AS AvgScore  -- Ensure the division is done in floating-point arithmetic
                    FROM
                        vw_StudySessionsWithStacks
                    WHERE
                        YEAR(SessionDate) = @Year
                    GROUP BY
                        StackName, MONTH(SessionDate)
                )

                SELECT
                    StackName,
                    [1] AS Jan, [2] AS Feb, [3] AS Mar, [4] AS Apr, [5] AS May, [6] AS Jun,
                    [7] AS Jul, [8] AS Aug, [9] AS Sep, [10] AS Oct, [11] AS Nov, [12] AS Dec
                FROM
                    MonthlyAverages
                PIVOT
                (
                    AVG(AvgScore)
                    FOR SessionMonth IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                ) AS PivotTable;
                ";

                var result = dbConnection.Query(sql, new { SelectedYear = year });
                return result;
            }
        }
        catch
        {
            throw;
        }
    }

}
