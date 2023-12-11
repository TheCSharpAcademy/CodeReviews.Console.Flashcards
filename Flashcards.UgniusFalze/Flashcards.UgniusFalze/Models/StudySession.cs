using System.Data;
using System.Data.SqlClient;

namespace Flashcards.UgniusFalze.Models;

public class StudySession
{
    public int StudySessionId { get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }

    public StudySession(int studySessionId, DateTime date, int score)
    {
        StudySessionId = studySessionId;
        Date = date;
        Score = score;
    }

    public static void InsertStudySession(SqlConnection sqlConn, int score, int stackId)
    {
        SqlCommand sqlCommand = sqlConn.CreateCommand();
        sqlCommand.CommandText = "INSERT INTO dbo.StudySessions (Score, StackId) VALUES (@score, @stackId)";
        sqlCommand.Parameters.Add("@score", SqlDbType.Int).Value = score;
        sqlCommand.Parameters.Add("@stackId", SqlDbType.Int).Value = stackId;
        sqlConn.Open();
        sqlCommand.Prepare();
        sqlCommand.ExecuteNonQuery();
        sqlConn.Close();
    }
    
    
}