using Flashcards.Database;
using Flashcards.Models;
using Flashcards.DTO;
using Flashcards.Services;
using Dapper;
using System.Data.SqlClient;

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
                string sql = $"INSERT INTO {ConfigSettings.TableNameStudySessions} (StackID, SessionDate, Score) VALUES (@StackID, @SessionDate, @Score)";
                dbConnection.Execute(sql, new { studySession.StackID, studySession.SessionDate, studySession.Score });
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Unable to insert new study session.", ex.Message);
            throw;
        }
    }

    public bool DeleteStudySession(StudySession studySession)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"DELETE FROM {ConfigSettings.TableNameStudySessions} WHERE SessionID = @SessionID";
                dbConnection.Execute(sql, new { studySession.SessionID });
                return true;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Unable to delete study session.", ex.Message);
            return false;
        }
    }

    public IEnumerable<ReportMonthlySessionCount>? GetStudySessionReportData(int year)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $@"
                    DECLARE @Year int = @SelectedYear;

                    WITH MonthlySessions AS (
	                    SELECT 
		                    StackName,
		                    MONTH(SessionDate) AS SessionMonth,
		                    COUNT(*) AS SessionCount
	                    FROM vw_StudySessionsWithStacks
	                    WHERE YEAR(SessionDate) = @Year
	                    GROUP BY 
		                    StackName,
		                    MONTH(SessionDate)
                    )

                    SELECT 
	                    StackName
	                    ,COALESCE([1], 0) AS Jan
	                    ,COALESCE([2], 0) AS Feb
	                    ,COALESCE([3], 0) AS Mar
	                    ,COALESCE([4], 0) AS Apr
	                    ,COALESCE([5], 0) AS May
	                    ,COALESCE([6], 0) AS Jun
	                    ,COALESCE([7], 0) AS Jul
	                    ,COALESCE([8], 0) AS Aug
	                    ,COALESCE([9], 0) AS Sep
	                    ,COALESCE([10], 0) AS Oct
	                    ,COALESCE([11], 0) AS Nov
	                    ,COALESCE([12], 0) AS Dec
                    FROM MonthlySessions
                        PIVOT
                        (
                            SUM(SessionCount)
                            FOR SessionMonth IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                        ) AS PivotTable;
                 ";
                
                var result = dbConnection.Query<ReportMonthlySessionCount>(sql, new { SelectedYear = year });
                return result;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error retrieving report data.", ex.Message);
            return null;
        }
    }

    public IEnumerable<ReportAverageSessionScore>? GetAverageScoreReportData(int year)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $@"
                    DECLARE @Year int = @SelectedYear;

                    WITH MonthlyAverages AS (
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
                        StackName
	                    ,COALESCE([1], 0) AS Jan
                        ,COALESCE([2], 0) AS Feb
                        ,COALESCE([3], 0) AS Mar
                        ,COALESCE([4], 0) AS Apr
                        ,COALESCE([5], 0) AS May
                        ,COALESCE([6], 0) AS Jun
                        ,COALESCE([7], 0) AS Jul
                        ,COALESCE([8], 0) AS Aug
                        ,COALESCE([9], 0) AS Sep
                        ,COALESCE([10], 0)  AS Oct
                        ,COALESCE([11], 0)  AS Nov
                        ,COALESCE([12], 0)  AS Dec
                    FROM
                        MonthlyAverages
                    PIVOT
                    (
                        AVG(AvgScore)
                        FOR SessionMonth IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                    ) AS PivotTable;
                ";

                var result = dbConnection.Query<ReportAverageSessionScore>(sql, new { SelectedYear = year });
                return result;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error retrieving report data.", ex.Message);
            return null;
        }
    }

}
