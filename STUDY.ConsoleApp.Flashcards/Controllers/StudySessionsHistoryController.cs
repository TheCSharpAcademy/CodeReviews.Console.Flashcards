using Dapper;
using STUDY.ConsoleApp.Flashcards.Models;
using STUDY.ConsoleApp.Flashcards.Models.DTOs;

namespace STUDY.ConsoleApp.Flashcards.Controllers;

public class StudySessionsHistoryController
{
    private readonly DatabaseHelper _databaseHelper = new();
    public List<StudySession> GetAllStudySessions()
    {
        using var connection = _databaseHelper.GetConnection();
        var sql = @"SELECT session_id as Id, 
                    session_date as SessionDate, 
                    stack_id as StackId, 
                    score as Score 
                    FROM study_sessions
                    Order by session_id, stack_id";
        return connection.Query<StudySession>(sql).ToList();
    }
    
    public List<ReportDto> NumberOfSessionsPerMonth(int year)
    {
        using var connection = _databaseHelper.GetConnection();
        var sql = @"SELECT stack_name ,
					[1] as January,
					[2] as February,
					[3] as March, 
					[4] as April, 
					[5] as May,
					[6] as June,
					[7] as July,
					[8] as August, 
					[9] as September, 
					[10] as October,
					[11] as November,
					[12] as December

					FROM 
					(
						SELECT Month(session_date) as MonthNumber, stack_name, score
						FROM study_sessions as ss
						INNER JOIN stack as st ON ss.stack_id = st.stack_id
						WHERE YEAR(session_date) = @Year
					) AS source
					PIVOT
					(
						Count(score) FOR MonthNumber IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10] ,[11], [12])
					) as pt;";
        
	    return connection.Query<ReportDto>(sql, new { Year = year }).ToList();
	}

    public List<ReportDto> AverageScorePerMonth(int year)
    {
	    using var connection = _databaseHelper.GetConnection();
	    var sql = @"SELECT stack_name ,
					[1] as January,
					[2] as February,
					[3] as March, 
					[4] as April, 
					[5] as May,
					[6] as June,
					[7] as July,
					[8] as August, 
					[9] as September, 
					[10] as October,
					[11] as November,
					[12] as December

					FROM 
					(
						SELECT Month(session_date) as MonthNumber, stack_name, score
						FROM study_sessions as ss
						INNER JOIN stack as st ON ss.stack_id = st.stack_id
						WHERE YEAR(session_date) = 2025
					) AS source
					PIVOT
					(
						AVG(score) FOR MonthNumber IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10] ,[11], [12])
					) as pt;";
	    return connection.Query<ReportDto>(sql, new { Year = year }).ToList();
    }
}