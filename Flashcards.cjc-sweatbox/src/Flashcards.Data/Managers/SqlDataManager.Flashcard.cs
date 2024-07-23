using System.Data;
using System.Data.SqlClient;
using Flashcards.Data.Entities;
using Flashcards.Data.Exceptions;

namespace Flashcards.Data.Managers;

/// <summary>
/// Partial class for Flashcard entity specific data manager methods against an T-SQL database.
/// </summary>
public partial class SqlDataManager
{
    #region Methods

    public void AddFlashcard(int stackId, string question, string answer)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();
        
        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.AddFlashcard";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@StackId", SqlDbType.Int).Value = stackId;
        command.Parameters.Add("@Question", SqlDbType.NVarChar).Value = question;
        command.Parameters.Add("@Answer", SqlDbType.NVarChar).Value = answer;
        command.ExecuteNonQuery();
    }

    public void DeleteFlashcard(int id)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.DeleteFlashcard";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        command.ExecuteNonQuery();
    }

    public StackEntity GetFlashcard(int id)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.GetFlashcard";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        
        using SqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new StackEntity(reader);
        }
        else
        {
            throw new EntityNotFoundException($"Unable to get Flashcard where Id = {id}");
        }
    }

    public IReadOnlyList<FlashcardEntity> GetFlashcards()
    {
        var output = new List<FlashcardEntity>();

        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.GetFlashcards";
        command.CommandType = CommandType.StoredProcedure;

        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            output.Add(new FlashcardEntity(reader));
        }

        return output;
    }

    public IReadOnlyList<FlashcardEntity> GetFlashcards(int stackId)
    {
        var output = new List<FlashcardEntity>();

        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.GetFlashcardsByStackId";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@StackId", SqlDbType.Int).Value = stackId;

        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            output.Add(new FlashcardEntity(reader));
        }

        return output;
    }

    public void SetFlashcard(int id, string question, string answer)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"{Schema}.SetFlashcard";
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        command.Parameters.Add("@Question", SqlDbType.NVarChar).Value = question;
        command.Parameters.Add("@Answer", SqlDbType.NVarChar).Value = answer;
        command.ExecuteNonQuery();
    }

    #endregion
}
