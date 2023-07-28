using Kmakai.FlashCards.Models;
using System.Data.SqlClient;

namespace Kmakai.FlashCards.Controllers;

public class StackController
{
    private static readonly string? ConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");

    public static void CreateStack(string name)
    {

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$" 
                INSERT INTO Stacks (Name)
                VALUES ('{name.ToLower()}')";


            command.ExecuteNonQuery();

            connection.Close();
        }

    }

    public static Stack GetStack(string name)
    {
        if (name == null)
        {
            throw new Exception("Stack name is null");
        }

        Stack stack = new Stack(name.ToLower());

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"
                SELECT * FROM Stacks
                WHERE Name = '{name}'";

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                stack.Id = reader.GetInt32(0);
            }

            connection.Close();
        }
        return stack;
    }

    public static List<Stack> GetStacks()
    {
        var stacks = new List<Stack>();

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"
                SELECT * FROM Stacks
                ORDER BY Id ASC
            ";

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Stack stack = new Stack(reader.GetString(1))
                {
                    Id = reader.GetInt32(0)
                };
                stacks.Add(stack);
            }

            connection.Close();
        }

        return stacks;
    }

    public static void DeleteStack(int stackId)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = @$" 
                DELETE FROM Stacks
                WHERE Id = {stackId}";

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

}
