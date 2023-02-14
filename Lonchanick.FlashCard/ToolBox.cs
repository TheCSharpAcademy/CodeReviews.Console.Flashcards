using ConsoleTables;
namespace Lonchanick9427.FlashCard;

public class ToolBox
{
    public static string RemoveWhitespace(string str)
    {
        return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
    }

    public static string GetStringInput(string param)
    {
        Console.Write($"{param}: ");
        string value = Console.ReadLine();

        while (string.IsNullOrEmpty(value))
        {
            Console.WriteLine("Empty values are not allowed, Try again");
            Console.Write($"{param}: ");
            value = Console.ReadLine();
        }
        //Console.WriteLine("Done!");
        return value;
    }
    public static int GetIntInput(string param)
    {
        Console.Write($"{param}: "); string readed = Console.ReadLine();
        int aux;
        while (!int.TryParse(readed, out aux))
        {
            Console.Write($"Error: {readed} is not a valid input! try again: ");
            readed = Console.ReadLine();
        }

        return aux;
    }

    public static void DeckPrettyTable(List<Stack> records)
    {
        var table = new ConsoleTable("Id", "Name", "Description");
        foreach (var item in records)
            table.AddRow(item.Id, item.Name, item.Description);
        
        Console.WriteLine("\t DECK-LIST");
        table.Write(ConsoleTables.Format.MarkDown);
    }

    public static void DeckPrettyTable2(List<Stack> records/*, Dictionary<int,int> i*/)
    {
        int aux = 1;
        var table = new ConsoleTable("Id", "Name", "Description");
        foreach (var item in records)
        { table.AddRow(aux, item.Name, item.Description); aux++; }
        
        Console.WriteLine("\t DECK-LIST");
        table.Write(ConsoleTables.Format.MarkDown);
    }

    public static void CardPrettyTable(List<Card> records)
    {
        var table = new ConsoleTable("Id", "Front-face", "Back-face");
        foreach (var item in records)
            table.AddRow(item.Id, item.Front, item.Back);
        Console.WriteLine("\t Cards-List");
        table.Write(ConsoleTables.Format.MarkDown);
    }
    public static void SessionStudyPrettyTable(List<StudySession> records)
    {
        var table = new ConsoleTable("Session Id", "User Name", "Time Init", "Time Fin", "Score","Stack Id");
        foreach (var item in records)
            table.AddRow(item.Id, item.User_, item.Init, item.Fin, item.Score, item.StackFk);
        Console.WriteLine("\t Study Sessions List");
        table.Write(ConsoleTables.Format.MarkDown);
    }
}


/*
 public class DataBaseInteractions
{

    private static string connectionString = "Data Source = localhost;" +
            "Initial Catalog = FlashCards;" +
            "User = sa;" +
            "Password = 091230;";

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

}

 */