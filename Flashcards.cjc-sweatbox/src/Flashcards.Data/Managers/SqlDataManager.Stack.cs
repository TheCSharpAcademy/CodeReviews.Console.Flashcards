using System.Data;
using System.Data.SqlClient;
using Flashcards.Data.Entities;
using Flashcards.Data.Exceptions;

namespace Flashcards.Data.Managers;

/// <summary>
/// Partial class for Stack entity specific data manager methods against an T-SQL database.
/// </summary>
public partial class SqlDataManager
{
    #region Methods

    public void AddStack(string name)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();
        
        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.AddStack";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = name;
        command.ExecuteNonQuery();
    }

    public void DeleteStack(int id)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.DeleteStack";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        command.ExecuteNonQuery();
    }

    public StackEntity GetStack(int id)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.GetStack";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        
        using SqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new StackEntity(reader);
        }
        else
        {
            throw new EntityNotFoundException($"Unable to get Stack where Id = {id}");
        }
    }

    public StackEntity GetStack(string name)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.GetStackByName";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = name;

        using SqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new StackEntity(reader);
        }
        else
        {
            throw new EntityNotFoundException($"Unable to get Stack where Name = {name}");
        }
    }

    public IReadOnlyList<StackEntity> GetStacks()
    {
        var output = new List<StackEntity>();

        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.GetStacks";
        command.CommandType = CommandType.StoredProcedure;
        
        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            output.Add(new StackEntity(reader));
        }

        return output;
    }

    public void SetStack(int id, string name)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.SetStack";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = name;
        command.ExecuteNonQuery();
    }

    #endregion
}
