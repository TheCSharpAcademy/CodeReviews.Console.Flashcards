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
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO dbo.study_session
                                    (playerName, pack_id, pack_size, date, cycles)
                                    VALUES (@PlayerName,
                                    (SELECT id FROM dbo.decks
                                    WHERE name = @Name),
                                    @PackSize, @Date, @Cycles);";
            command.Parameters.AddWithValue("@PlayerName", sessionModel.Player);
            command.Parameters.AddWithValue("@Name", sessionModel.Pack);
            command.Parameters.AddWithValue("@PackSize", sessionModel.PackSize);
            command.Parameters.AddWithValue("@Date", sessionModel.Date);
            command.Parameters.AddWithValue("@Cycles", sessionModel.Cycles);
            command.ExecuteNonQuery();
        }
    }
}
