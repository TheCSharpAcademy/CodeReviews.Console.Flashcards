
using System.Data.SqlClient;

namespace Lonchanick9427.FlashCard.DB;

public static class StudySessionDB
{
    private static string connectionString = "Data Source = localhost;" +
            "Initial Catalog = FlashCards;" +
            "User = sa;" +
            "Password = 091230;";
    public static void Add(StudySession p)
    {
        string query = "INSERT INTO studySession VALUES(@User_, @Init, @Fin, @Score, @StackFk);";
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@User_", p.User_);
            command.Parameters.AddWithValue("@Init", p.Init);
            command.Parameters.AddWithValue("@Fin", p.Fin);
            command.Parameters.AddWithValue("@Score", p.Score);
            command.Parameters.AddWithValue("@StackFk", p.StackFk);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public static List<StudySession> ShowAll()
    {
        string query = "SELECT * FROM studySession;";
        List<StudySession> sessions = new();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string user = reader.GetString(1);
                DateTime init = reader.GetDateTime(2);
                DateTime fin = reader.GetDateTime(3);
                int score = reader.GetInt32(4);
                int stackId = reader.GetInt32(5);
                StudySession aux = new StudySession() 
                {
                    Id = id,
                    StackFk = stackId,
                    Score= score,
                    User_ = user,
                    Init= init,
                    Fin= fin,
                };
                sessions.Add(aux);
            }
            reader.Close();
            connection.Close();

        }
        return sessions;
    }

    public static void Delete(int Id)
    {
        var query = "DELETE FROM studySession " +
                    "WHERE id = @Id;";
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", Id);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

}
