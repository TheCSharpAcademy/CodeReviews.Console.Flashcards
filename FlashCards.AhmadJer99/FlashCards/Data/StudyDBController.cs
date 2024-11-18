using Dapper;
using FlashCards.Models;

namespace FlashCards.Data;

internal class StudyDBController : DataBaseController<StudySession>
{
    public StudyDBController()
    {
        InitDataBase();
    }
    public override void DeleteRow(int _id) // Not needed
    {
        throw new NotImplementedException();
    }

    public override void InsertRow(StudySession studySession)
    {
        using (var connection = CreateConnection())
        {
            var insertQuery = "INSERT INTO study (FK_stack_id,session_date,score) VALUES (@FK_stack_id,@session_date,@score)";
            connection.Execute(insertQuery, studySession);
        }
    }

    public override List<StudySession> ReadAllRows()
    {
        using (var connection = CreateConnection())
        {
            var readQuery = "SELECT * FROM study";
            List<StudySession> studySessions = connection.Query<StudySession>(readQuery).ToList();
            return studySessions;
        }
    }public  List<StudySession> ReadAllRows(int stackId)
    {
        using (var connection = CreateConnection())
        {
            var readQuery = "SELECT * FROM study";
            List<StudySession> studySessions = connection.Query<StudySession>(readQuery).ToList();
            return studySessions;
        }
    }


    public override void UpdateRow(StudySession classObject) // Not needed
    {
        throw new NotImplementedException();
    }

    internal List<YearlyReport> ReadYearlyStackData(int stackId, string year)
    {
        using (var connection = CreateConnection())
        {
            var reportQuery = @"SELECT 
	                                (SELECT name FROM stacks WHERE id = @stackId) AS StackName,
	                                [January],
	                                [February],
	                                [March],
	                                [April],
	                                [May],
	                                [June],
	                                [July],
	                                [August],
	                                [September],
	                                [October],
	                                [November],
	                                [December]
                                FROM
                                (
                                SELECT
	                                (SELECT name FROM stacks WHERE id = 2) AS StackName,
	                                DATENAME(MONTH,session_date) AS [Month],
	                                score FROM study
	                                WHERE FK_stack_id = @stackId
	                                AND YEAR(session_date) = @year
                                ) AS Src
                                PIVOT
                                (
	                                AVG(score)
	                                FOR [Month] IN ([January],[February],[March],[April],[May],[June],[July],[August],[September],[October],[November],[December])
                                ) AS Pvt";
            var parameters = new DynamicParameters();
            parameters.Add("@stackId", stackId);
            parameters.Add("@year", year);
            List<YearlyReport> yearlyReports = connection.Query<YearlyReport>(reportQuery, parameters).ToList();
            return yearlyReports;
        }
    }
}

