using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace FlashCards.Forser;

public class DataLayer
{
    internal string? DatabaseConnection { get; }
    public DataLayer() 
    { 
        DatabaseConnection = GetConnectionStringFromSettings();
    }
    private string? GetConnectionStringFromSettings()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        return config.GetConnectionString("MSSQL");
    }
    public int NewStackEntry(Stack stack)
    {
        int rows = 0;
        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"INSERT Stacks (Name) VALUES ('{stack.Name}')";

                connection.Open();
                rows = cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

        return rows;
    }
    public int ReturnNumberOfStacks()
    {
        int stackCount = 0;
        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT Count('Name') FROM Stacks";

                connection.Open();
                stackCount = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

        return stackCount;
    }
    public List<Stack> FetchAllStacks()
    {
        List<Stack> listOfStacks = new List<Stack>();

        using (SqlConnection connection = new SqlConnection(DatabaseConnection))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT * FROM Stacks";

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                listOfStacks.Add(new Stack
                {
                    StackId = Convert.ToInt32(reader[0]),
                    Name = reader[1].ToString()
                });
            }

            connection.Close();

            return listOfStacks;
        }
    }
    internal bool CheckStackId(int stackId)
    {
        bool stackValid = false;
        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT Count('Name') FROM Stacks WHERE Id = {stackId}";

                connection.Open();
                stackValid = Convert.ToBoolean(cmd.ExecuteScalar());
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

        return stackValid;
    }
    internal bool DeleteStackById(int stackId)
    {
        bool stackDeleted = false;
        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"DELETE FROM Stacks WHERE Id = {stackId}";

                connection.Open();
                stackDeleted = Convert.ToBoolean(cmd.ExecuteNonQuery());
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

        return stackDeleted;
    }
}