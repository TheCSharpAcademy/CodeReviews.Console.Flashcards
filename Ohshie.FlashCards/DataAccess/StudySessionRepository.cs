using System.Data.SqlClient;
using Ohshie.FlashCards.StudySessionManager;

namespace Ohshie.FlashCards.DataAccess;

public class StudySessionRepository : DbBase
{
    public void CreateSession(StudySession session)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = $"INSERT INTO StudySessions (DATE, DeckName, SolvedCards) " +
                                       $"VALUES ('{session.Date}', '{session.DeckName}', '{session.FlashcardsSolved}')";

           tableCommand.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    public List<StudySession> FetchSessions()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "SELECT * FROM StudySessions";

            var reader = tableCommand.ExecuteReader();

            var deckList = ReadFromDbToSessionList(reader);

            return deckList;
        }
    }

    private List<StudySession> ReadFromDbToSessionList(SqlDataReader reader)
    {
        List<StudySession> studySessions = new();

        while (reader.Read())
        {
            var session = new StudySession()
            {
                Id = reader.GetInt32(0),
                Date = reader.GetString(1),
                DeckName = reader.GetString(2),
                FlashcardsSolved = reader.GetInt32(3)
            };

            studySessions.Add(session);
        }

        return studySessions;
    }
}