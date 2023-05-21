using FlashcardsLibrary.Models;
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

    public static List<PackModel> GetPacks()
    {
        List<PackModel> packs = new();
        using (SqlConnection connection = new(ConnManager.GetConnectionString(ConnManager.FlashCardDb)))
        {
            connection.Open();
            SqlCommand getPacks = connection.CreateCommand();
            getPacks.CommandText = @"SELECT name FROM dbo.decks;";
            SqlDataReader reader = getPacks.ExecuteReader();
            while (reader.Read())
            {
                packs.Add(new PackModel
                (
                    reader.GetString(0)
                ));
            }
        }
        return packs;
    }

    public static int UpdatePackName(string currentName, string newName)
    {
        int rowsUpdated;
        using (SqlConnection connection = new(ConnManager.GetConnectionString(ConnManager.FlashCardDb)))
        {
            connection.Open();
            SqlCommand rename = connection.CreateCommand();
            rename.CommandText = @"UPDATE dbo.decks
                                SET name = @NewName
                                WHERE name = @Name;";
            rename.Parameters.AddWithValue("@NewName", newName);
            rename.Parameters.AddWithValue("@Name", currentName);
            rowsUpdated = rename.ExecuteNonQuery();
        }
        return rowsUpdated;
    }

    public static int DeletePack(string choiceName)
    {
        int rowsDeleted;
        using (SqlConnection connection = new(ConnManager.GetConnectionString(ConnManager.FlashCardDb)))
        {
            connection.Open();
            SqlCommand delete = connection.CreateCommand();
            delete.CommandText = @"DELETE FROM dbo.flashcards
                                WHERE deck_id IN
                                (SELECT id FROM dbo.decks
                                WHERE name = @Name);

                                DELETE FROM dbo.decks
                                WHERE name = @Name;";
            delete.Parameters.AddWithValue("@Name", choiceName);
            rowsDeleted = delete.ExecuteNonQuery();
        }
        return rowsDeleted;
    }
}
