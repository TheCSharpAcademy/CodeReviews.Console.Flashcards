using Dapper;
using FlashcardsProgram.Database;
using FlashcardsProgram.Stacks;
using FlashcardsProgram.StudySession;

namespace FlashcardsProgram.Reports;

public class ReportsRepository
{
    public List<Dictionary<string, object?>> GetSessionScoresPerMonthPerStack(int year)
    {
        string sql = $@"
            SELECT *
            FROM
            (
                SELECT {StackDao.TableName}.Name as Name,
                DATENAME(MONTH, StudySessions.DateTime) as Month,
                (
                    CONVERT(FLOAT, SUM(NumCorrect))/
                    NULLIF(CONVERT(FLOAT, SUM(NumAttempted)), 0)
                    * 100
                ) AS Score
                FROM {StudySessionDAO.TableName}
                INNER JOIN {StackDao.TableName}
                    ON {StudySessionDAO.TableName}.StackId = {StackDao.TableName}.Id
                WHERE YEAR(StudySessions.DateTime) = @Year
                GROUP BY {StackDao.TableName}.Name, DATENAME(MONTH, StudySessions.DateTime)
            ) AS SourceTable
            PIVOT
            (
                AVG(Score)
                FOR Month IN (
                    [January], [February], [March], [April], [May], [June],
                    [July], [August], [September], [October], [November], [December]
                )
            ) AS PivotTable;
        ";

        return ConnectionManager.Connection
            .Query(
                sql,
                new { Year = year }
            )
            .ToList()
            .Where(obj => obj != null)
            .Select(resObj => new Dictionary<string, object?>(resObj))
            .ToList();
    }
}