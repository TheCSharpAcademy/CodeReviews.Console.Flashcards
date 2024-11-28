using Microsoft.Data.SqlClient;
using Dapper;
using FlashCards.Data;

namespace FlashCards.Controllers;

public class FlashcardController (DatabaseService DbService)
{
    private DatabaseService DbService { get; } = DbService;
    
    public List<FlashCardDto> GetAllFlashcards(int stack_id) =>
        ExecuteQuery("Select flashCardId, front, back FROM Flashcards WHERE stack_id = @Stack_id",
            new { Stack_id = stack_id });

    public List<FlashCardDto> GetAmountOfFlashcards(string limit, int stack_id) =>
        ExecuteQuery($"SELECT TOP {limit} flashCardId, front, back FROM Flashcards WHERE stack_id = @Stack_id",
            new { Stack_id = stack_id });

    public void InsertFlashcard(string front, string back, int stack_id)
    {
        string query = @"INSERT INTO Flashcards (front, back, stack_id)
                         VALUES (@Front, @Back, @Stack_id)";
        var parameters = new { Front = front, Back = back, Stack_id = stack_id };
        ExecuteQuery(query, parameters);
    }

    public void UpdateFrontOfFlashcard(string flashcardFront, string front, int stack_id)
    {
        var query = $"Update Flashcards SET front = @Front WHERE front = @FlashcardFront AND stack_id = @Stack_id";
        var parameters = new { Front = front, FlashcardFront = flashcardFront, Stack_id = stack_id };
        ExecuteNonQuery(query, parameters);
    }

    public void UpdateBackOfFlashcard(string flashcardBack, string back, int stack_id)
    {
        var query = $"Update Flashcards SET back = @Back WHERE back = @FlashcardBack AND stack_id = @Stack_id";
        var parameters = new { Back = back, FlashcardBack = flashcardBack, Stack_id = stack_id };
        ExecuteNonQuery(query, parameters);
    }

    public void DeleteFlashcard(string flashcardFront, int stack_id)
    {
        var query = "DELETE FROM Flashcards WHERE front = @Front AND stack_id = @Stack_id";
        var parameters = new { Front = flashcardFront, Stack_id = stack_id };
        ExecuteNonQuery(query, parameters);
    }

    public void DeleteAllFlashcards(int stack_id) =>
        ExecuteNonQuery($"DELETE FROM Flashcards WHERE stack_id = {stack_id}");

    private List<FlashCardDto> ExecuteQuery(string query, object? parameters = null)
    {
        using SqlConnection connection = DbService.GetConnection();
        return connection.Query<FlashCardDto>(query, parameters).ToList();
    }

    private void ExecuteNonQuery(string query, object? parameters = null)
    {
        using SqlConnection connection = DbService.GetConnection();
        connection.Execute(query, parameters);
    }

    public List<int> GetAllFlashcardIds(int stack_id)
    {
        List<FlashCardDto> flashcards = GetAllFlashcards(stack_id);
        List<int> flashcardIds = new List<int>();
        foreach (FlashCardDto flashcard in flashcards)
        {
            flashcardIds.Add(flashcard.flashcardId);
        }

        return flashcardIds;
    }
}