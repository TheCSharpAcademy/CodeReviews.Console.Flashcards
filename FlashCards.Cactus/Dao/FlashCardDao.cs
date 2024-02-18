using FlashCards.Cactus.DataModel;
using Microsoft.Data.SqlClient;

namespace FlashCards.Cactus.Dao;

public class FlashCardDao
{
    public FlashCardDao(string DBConnStr)
    {
        DBConnectionStr = DBConnStr;
    }

    public string DBConnectionStr { get; set; }

    public int Insert(FlashCard card)
    {
        int rowsAffected = -1;
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $@"INSERT INTO FlashCard(sid, front, back) 
                                    VALUES('{card.SId}', '{card.Front}', '{card.Back}');";
            rowsAffected = command.ExecuteNonQuery();
        }
        return rowsAffected;
    }

    public List<FlashCard> FindAll()
    {
        List<FlashCard> cards = new List<FlashCard>();
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM FlashCard";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string front = reader.GetString(1);
                    string back = reader.GetString(2);
                    int sid = reader.GetInt32(3);
                    FlashCard card = new FlashCard(id, sid, front, back);
                    cards.Add(card);
                }
            }
        }
        return cards;
    }

    public int DeleteById(int id)
    {
        int rowsAffected = -1;
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            string deleteSql = "DELETE FROM FlashCard WHERE fid = @id";
            SqlCommand deleteCommand = new SqlCommand(deleteSql, connection);
            deleteCommand.Parameters.AddWithValue("@id", id);
            rowsAffected = deleteCommand.ExecuteNonQuery();
        }
        return rowsAffected;
    }

    public int Update(FlashCard card)
    {
        int rowsAffected = -1;
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            string updateSql = "UPDATE FlashCard SET front = @front, back=@back WHERE fid = @id";
            SqlCommand updateCommand = new SqlCommand(updateSql, connection);
            updateCommand.Parameters.AddWithValue("@front", card.Front);
            updateCommand.Parameters.AddWithValue("@back", card.Back);
            updateCommand.Parameters.AddWithValue("@id", card.Id);
            rowsAffected = updateCommand.ExecuteNonQuery();
        }
        return rowsAffected;
    }
}

