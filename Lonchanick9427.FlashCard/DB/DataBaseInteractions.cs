using System.Data.SqlClient;

namespace Lonchanick9427.FlashCard.DB;

public class DataBaseInteractions
{

    private static string connectionString = "Data Source = localhost;" +
            "Initial Catalog = FlashCards;" +
            "User = sa;" +
            "Password = 091230;";
    /*
        public List<Persona> Get()
        {
            string query = "select id, name, gender from students;";
            List<Persona> personas = new List<Persona>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nombre = reader.GetString(1);
                    string gender = reader.GetString(2);
                    Persona p = new Persona(nombre, gender, id);
                    personas.Add(p);
                }
                reader.Close();
                connection.Close();

            }
            return personas;
        }
    */

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

    /*
        public void AddPost(Post p)
        {
            var query = "insert into jasonPlaceholder (userId,title,body) values(@userId,@title, @body);";
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userId", p.userId);
                command.Parameters.AddWithValue("@title", p.title);
                command.Parameters.AddWithValue("@body", p.body);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }



        public void Edit(Persona p, int Id)
        {
            var query = "UPDATE students " +
                        "SET name = @nombre, gender = @genero " +
                        "WHERE id = @Id;";
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombre", p.Nombre);
                command.Parameters.AddWithValue("@genero", p.Genero);
                command.Parameters.AddWithValue("@Id", Id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(int Id)
        {
            var query = "DELETE FROM students " +
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
    */
}
