using System.Data.SqlClient;
using System.Xml.Linq;

namespace Lonchanick9427.FlashCard.DB;

public static class DeckDB
{
    private static string connectionString = "Data Source = localhost;" +
            "Initial Catalog = FlashCards;" +
            "User = sa;" +
            "Password = 091230;";
    public static void Add(Deck p)
    {
        string query = "insert into deck values(@name,@descrption);";
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", p.Name);
            command.Parameters.AddWithValue("@descrption", p.Description);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public static List<Deck> Get()
    {
        string query = "select Id, Name_, Description from deck;";
        List<Deck> personas = new List<Deck>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string description = reader.GetString(2);
                Deck p = new Deck(id, name, description);
                personas.Add(p);
            }
            reader.Close();
            connection.Close();

        }
        return personas;
    }

    public static void Delete(int Id)
    {
        var query = "DELETE FROM deck " +
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
