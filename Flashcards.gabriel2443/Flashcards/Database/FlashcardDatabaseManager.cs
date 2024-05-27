using Dapper;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Flashcards.Database;

internal class FlashcardDatabaseManager
{
    private string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");

    internal void AddFlashard(FlashCards flashCard)
    {
        var sql = $"INSERT INTO Flashcards VALUES(@Question, @Answer, @CardstackId)";
        using (var connection = new SqlConnection(connectionStr))
        {
            connection.Execute(sql, flashCard);
        }
    }

    internal List<FlashCardsDto> ReadFlashcardsDTO(CardStack stack)
    {
        var sql = $"SELECT * FROM Flashcards WHERE StackId = {stack.CardstackId}";
        using (var connection = new SqlConnection(connectionStr))
        {
            var flashcards = connection.Query<FlashCardsDto>(sql).ToList();
            return flashcards;
        }
    }

    internal List<FlashCards> ReadFlahcards(CardStack stack)
    {
        var sql = $"SELECT * FROM Flashcards WHERE StackId = {stack.CardstackId}";
        using (var connection = new SqlConnection(connectionStr))
        {
            var flashcards = connection.Query<FlashCards>(sql).ToList();
            return flashcards;
        }
    }

    internal void UpdateFlashcards(FlashCards flashCard, string flashcardQuestion, string flashcardAnswer)
    {
        var sql = "UPDATE Flashcards SET Question = @Question, Answer = @Answer WHERE FlashcardId = @FlashcardId";
        using (var connection = new SqlConnection(connectionStr))
        {
            connection.Execute(sql, new { Question = flashcardQuestion, Answer = flashcardAnswer, FlashcardId = flashCard.FlashcardId });
        }
    }

    internal void DeleteFlashcard(FlashCards flashCards)
    {
        var sql = "DELETE FROM Flashcards WHERE FlashcardId = @FlashcardId";
        using (var connection = new SqlConnection(connectionStr))
        {
            connection.Execute(sql, new { FlashcardId = flashCards.FlashcardId });
        }
    }
}