using Microsoft.Data.SqlClient;
using Flashcards.yemiOdetola.Models;

namespace Flashcards.yemiOdetola.Repositories;

public class StackRepository
{
  string connectionString = "Server=localhost,1433;Database=FlashCards;User Id=sa;Password=<YourStrong@Passw0rd>;TrustServerCertificate=True;";
  public List<Stack> GetStacks()
  {
    List<Stack> stacks = new List<Stack>();

    using SqlConnection sqlConnection = new SqlConnection(connectionString);
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

  public Stack? GetSingleStack(string Name)
  {
    using SqlConnection sqlConnection = new SqlConnection(connectionString);
    sqlConnection.Open();
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"SELECT * FROM stacks WHERE name = '{Name}'";

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

  public void CreateStack(string Name)
  {
    using SqlConnection sqlConnection = new SqlConnection(connectionString);
    sqlConnection.Open();
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"INSERT INTO stacks (name) VALUES ('{Name}')";
    sqlCommand.ExecuteNonQuery();
  }


  public void DeleteStack(string Name)
  {
    using SqlConnection sqlConnection = new SqlConnection(connectionString);
    sqlConnection.Open();
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"DELETE FROM stacks WHERE name = '{Name}'";
    sqlCommand.ExecuteNonQuery();
  }

  public void UpdateStack(string Name, string newName)
  {
    using SqlConnection sqlConnection = new SqlConnection(connectionString);
    sqlConnection.Open();
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"UPDATE stacks SET name = '{newName}' WHERE name = '{Name}'";
    sqlCommand.ExecuteNonQuery();
  }
}
