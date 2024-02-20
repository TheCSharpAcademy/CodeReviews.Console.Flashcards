using System.Data.SqlClient;
using Flashcards.Mateusz_Platek.Models;

namespace Flashcards.Mateusz_Platek.Repositories;

public class StacksRepository
{
    public List<Stack> GetStacks()
    {
        List<Stack> stacks = new List<Stack>();

        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = "SELECT * FROM stacks";
        
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        if (sqlDataReader.HasRows)
        {
            while (sqlDataReader.Read())
            {
                Stack stack = new Stack(
                    sqlDataReader.GetInt32(0),
                    sqlDataReader.GetString(1)
                );
                stacks.Add(stack);
            }
        }
        
        return stacks;
    }

    public Stack? GetStack(string stackName)
    {
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = $"SELECT * FROM stacks WHERE name = '{stackName}'";

        Stack? stack = null;
        
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        if (sqlDataReader.HasRows)
        {
            while (sqlDataReader.Read())
            {
                stack = new Stack(
                    sqlDataReader.GetInt32(0),
                    sqlDataReader.GetString(1)
                );
            }
        }

        return stack;
    }

    public void AddStack(string stackName)
    {
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = $"INSERT INTO stacks (name) VALUES ('{stackName}')";
        sqlCommand.ExecuteNonQuery();
    }

    public void UpdateStack(string stackName, string newName)
    {
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = $"UPDATE stacks SET name = '{newName}' WHERE name = '{stackName}'";
        sqlCommand.ExecuteNonQuery();
    }

    public void DeleteStack(string stackName)
    {
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = $"DELETE FROM stacks WHERE name = '{stackName}'";
        sqlCommand.ExecuteNonQuery();
    }
}