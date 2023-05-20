using FlashcardsLibrary.DTOs;
using System.Data.SqlClient;

namespace FlashcardsLibrary.Data;
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

    public static List<PackOverviewDTO> GetAllPacks()
    {
        List<PackOverviewDTO> packs = new();
        using (SqlConnection connection = new(ConnManager.GetConnectionString(ConnManager.FlashCardDb)))
        {
            connection.Open();
            SqlCommand getPacks = connection.CreateCommand();
            getPacks.CommandText = @"SELECT * FROM dbo.decks;";
            SqlDataReader reader = getPacks.ExecuteReader();
            while (reader.Read())
            {
                packs.Add(new PackOverviewDTO
                (
                    reader.GetInt32(0),
                    reader.GetString(1)
                ));
            }
        }
        return packs;
    }
}
