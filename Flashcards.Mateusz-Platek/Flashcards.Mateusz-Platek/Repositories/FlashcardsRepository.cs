using System.Data.SqlClient;
using Flashcards.Mateusz_Platek.Models;

namespace Flashcards.Mateusz_Platek.Repositories;

public class FlashcardsRepository
{
    public List<Flashcard> GetFlashcardsOfStack(string stackName)
    {
        List<Flashcard> flashcards = new List<Flashcard>();
        
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = $"SELECT * FROM flashcards JOIN stacks ON flashcards.stack_id = stacks.stack_id WHERE name = '{stackName}'";
        
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        if (sqlDataReader.HasRows)
        {
            while (sqlDataReader.Read())
            {
                Flashcard flashcard = new Flashcard(
                    sqlDataReader.GetInt32(0),
                    sqlDataReader.GetInt32(1),
                    sqlDataReader.GetString(2),
                    sqlDataReader.GetString(3)
                );
                flashcards.Add(flashcard);
            }
        }

        return flashcards;
    }
    
    public List<FlashcardDTO> GetFlashcardsDTOOfStack(string stackName)
    {
        List<FlashcardDTO> flashcards = new List<FlashcardDTO>();
        
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = $"SELECT * FROM flashcards JOIN stacks ON flashcards.stack_id = stacks.stack_id WHERE name = '{stackName}'";
        
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        if (sqlDataReader.HasRows)
        {
            while (sqlDataReader.Read())
            {
                FlashcardDTO flashcard = new FlashcardDTO(
                    sqlDataReader.GetString(2),
                    sqlDataReader.GetString(3)
                );
                flashcards.Add(flashcard);
            }
        }

        return flashcards;
    }

    public void AddFlashcard(int stackId, string word, string translation)
    {
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = $"INSERT INTO flashcards (stack_id, word, translation) VALUES ({stackId}, '{word}', '{translation}')";
        sqlCommand.ExecuteNonQuery();
    }

    public void UpdateFlashcard(int flashcardId, int stackId, string word, string translation)
    {
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = $"UPDATE flashcards SET stack_id = {stackId}, word = '{word}', translation = '{translation}' WHERE flashcard_id = {flashcardId}";
        sqlCommand.ExecuteNonQuery();
    }

    public void DeleteFlashcard(int flashcardId)
    {
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = $"DELETE FROM flashcards WHERE flashcard_id = {flashcardId}";
        sqlCommand.ExecuteNonQuery();
    }
}