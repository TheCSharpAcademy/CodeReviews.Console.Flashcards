using Dapper;
using STUDY.ConsoleApp.Flashcards.Enums;
using STUDY.ConsoleApp.Flashcards.Models;

namespace STUDY.ConsoleApp.Flashcards.Controllers;

public class StudySessionController
{
    private readonly DatabaseHelper _databaseHelper = new();
    public void AddStudySession(StudySession session)
    {
        using var connection = _databaseHelper.GetConnection();
        var sql = "INSERT INTO study_sessions(session_date, stack_id, score) VALUES (@SessionDate, @StackId, @Score)";
        connection.Execute(sql,
            new { SessionDate = DateTime.Now, StackId = session.StackId, Score = session.Score });

    }
}