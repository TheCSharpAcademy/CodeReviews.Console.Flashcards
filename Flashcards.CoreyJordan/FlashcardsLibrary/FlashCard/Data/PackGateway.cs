using System.Data.SqlClient;

namespace FlashcardsLibrary.FlashCard.Data;
public static class PackGateway
{
    public static void InsertPack(string name)
    {
        using (SqlConnection connection = new(ConnManager.GetConnectionString(ConnManager.FlashCardDb)))
        {
            connection.Open();
            SqlCommand insert = connection.CreateCommand();
            insert.CommandText = @"INSERT INTO dbo.decks (name)
                                VALUES (@Name);";
            insert.Parameters.AddWithValue("@Name", name);
            insert.ExecuteNonQuery();
        }
    }
}
