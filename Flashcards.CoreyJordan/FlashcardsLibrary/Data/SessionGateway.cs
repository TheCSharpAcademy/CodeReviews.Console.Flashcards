using FlashcardsLibrary.Models;
using System.Data.SqlClient;

namespace FlashcardsLibrary.Data;
public class SessionGateway : ConnManager
{
    public static void InsertSession(SessionModel sessionModel)
    {
        using (SqlConnection connection = new(FlashCardDb))
        {
            connection.Open();
            SqlCommand addSession = connection.CreateCommand();
            addSession.CommandText = @"INSERT INTO dbo.study_session
                                    (playerName, pack_id, pack_size, date, cycles)
                                    VALUES (@PlayerName,
                                    (SELECT id FROM dbo.decks
                                    WHERE name = @Name),
                                    @PackSize, @Date, @Cycles);";
            addSession.Parameters.AddWithValue("@PlayerName", sessionModel.Player);
            addSession.Parameters.AddWithValue("@Name", sessionModel.Pack);
            addSession.Parameters.AddWithValue("@PackSize", sessionModel.PackSize);
            addSession.Parameters.AddWithValue("@Date", sessionModel.Date);
            addSession.Parameters.AddWithValue("@Cycles", sessionModel.Cycles);
            addSession.ExecuteNonQuery();
        }
    }

    public static List<string> GetAllUsers()
    {
        List<string> users = new();
        using (SqlConnection connection = new(FlashCardDb))
        {
            connection.Open();
            SqlCommand getAll = connection.CreateCommand();
            getAll.CommandText = @"SELECT playerName FROM dbo.study_session;";
            SqlDataReader reader = getAll.ExecuteReader();
            while (reader.Read())
            {
                users.Add(reader.GetString(0));
            }
        }
        return users;
    }

    public static List<SessionModel> GetAllSessions()
    {
        throw new NotImplementedException();
    }

    public static List<SessionModel> GetSessionsByUser(string userChoice)
    {
        throw new NotImplementedException();
    }
}
