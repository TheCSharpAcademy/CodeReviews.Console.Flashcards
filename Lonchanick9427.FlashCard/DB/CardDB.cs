
using System.Data.SqlClient;

namespace Lonchanick9427.FlashCard.DB;
public static class CardDB
{
    private static string connectionString = "Data Source = localhost;" +
            "Initial Catalog = FlashCards;" +
            "User = sa;" +
            "Password = 091230;";
    public static void Add(Card p)
    {
        string query = "insert into cards values(@front,@back,@fk);";
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@front", p.Front);
            command.Parameters.AddWithValue("@back", p.Back);
            command.Parameters.AddWithValue("@fk", p.Fk);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public static bool Delete(int Id)
    {
        var query = $"DELETE FROM cards WHERE id = {Id};";
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", Id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
    }
    public static bool Update(int Id, Card param)
    {
        string front = param.Front;
        string back = param.Back;
        var query = $"UPDATE cards SET Front = '{front}', Back = '{back}' WHERE Id = {Id};";
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Front", front);
            command.Parameters.AddWithValue("@Back", back);
            command.Parameters.AddWithValue("@Id", Id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
    }
    public static List<Card> CardsByStackId(int Id)
    {
        string query = $"select * FROM cards WHERE DeckFk = {Id};";
        List<Card> FcList = new List<Card>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string front = reader.GetString(1);
                string back = reader.GetString(2);
                int fk = reader.GetInt32(3);
                Card fc = new Card(id, front, back,fk);
                FcList.Add(fc);
            }
            reader.Close();
            connection.Close();
        }
        return FcList;
    }
}
