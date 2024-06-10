using Dapper;
using Flashcards.kalsson.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards.kalsson.Data;

public class StudySessionRepository
{
    private readonly string _connectionString;

    public StudySessionRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<StudySession> GetAllStudySessions()
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Query<StudySession>("SELECT * FROM StudySessions");
    }

    public void AddStudySession(StudySession studySession)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Execute("INSERT INTO StudySessions (StackId, StudyDate, Score) VALUES (@StackId, @StudyDate, @Score)", studySession);
    }
}