using Flashcards.yemiOdetola.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards.yemiOdetola.Repositories;

public class FlashCardsRepository
{
  string ConnectionString = "Server=localhost,1433;Database=FlashCards;User Id=sa;Password=<YourStrong@Passw0rd>;TrustServerCertificate=True;";

  public List<FlashCard> GetCardsStack(string Name)
  {
    List<FlashCard> flashcards = new List<FlashCard>();

    using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
    sqlConnection.Open();
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"SELECT * FROM flashcards JOIN stacks ON flashcards.stack_id = stacks.id WHERE name = '{Name}'";

    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
    if (sqlDataReader.HasRows)
    {
      while (sqlDataReader.Read())
      {
        FlashCard flashcard = new FlashCard(
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

  public List<FlashCardDto> GetStackCardsDto(string Name)
  {
    List<FlashCardDto> flashcards = new List<FlashCardDto>();

    using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
    sqlConnection.Open();
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"SELECT * FROM flashcards JOIN stacks ON flashcards.stack_id = stacks.id WHERE name = '{Name}'";

    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
    if (sqlDataReader.HasRows)
    {
      while (sqlDataReader.Read())
      {
        FlashCardDto flashcard = new FlashCardDto(
            sqlDataReader.GetString(2),
            sqlDataReader.GetString(3)
        );
        flashcards.Add(flashcard);
      }
    }

    return flashcards;
  }

  public void AddCard(int stackId, string word, string category)
  {
    using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
    sqlConnection.Open();
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"INSERT INTO flashcards (stack_id, word, category) VALUES ({stackId}, '{word}', '{category}')";
    sqlCommand.ExecuteNonQuery();
  }

  public void UpdateCard(int cardId, int stackId, string word, string category)
  {
    using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
    sqlConnection.Open();
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"UPDATE flashcards SET stack_id = {stackId}, word = '{word}', category = '{category}' WHERE id = {cardId}";
    sqlCommand.ExecuteNonQuery();
  }

  public void DeleteCard(int cardId)
  {
    using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
    sqlConnection.Open();
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"DELETE FROM flashcards WHERE id = {cardId}";
    sqlCommand.ExecuteNonQuery();
  }

}
