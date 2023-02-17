using System.Data.SqlClient;

namespace Lonchanick9427.FlashCard.DB;
public static class StackDB
{
    private static string connectionString = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=fc; Integrated Security=true;";
    public static void Add(Stack p)
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
    public static List<Stack> Get(out Dictionary<int, int> index)
    {
        index = new Dictionary<int, int>();
        string query = "select Id, Name, Description from deck;";
        List<Stack> personas = new List<Stack>();
        int iindex = 1;
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
                Stack p = new Stack(id, name, description);
                personas.Add(p);
                index.Add(iindex, id);
                iindex++;
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

    public static bool Update(Stack p)
    {
        var query = "UPDATE deck  SET Name_ = @nombre, Description = @description " +
                    "WHERE id = @Id;";

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombre", p.Name);
                command.Parameters.AddWithValue("@Description", p.Description);
                command.Parameters.AddWithValue("@Id", p.Id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
        }catch(Exception ex) 
        { 
            Console.WriteLine("Something happend: ");
            Console.WriteLine(ex.Message.ToString());
            return false;
        }  
    }
}
