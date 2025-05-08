using Dapper;
using STUDY.ConsoleApp.Flashcards.Enums;
using STUDY.ConsoleApp.Flashcards.Models;
using STUDY.ConsoleApp.Flashcards.Models.DTOs;

namespace STUDY.ConsoleApp.Flashcards.Controllers;

public class FlashcardController
{
    private readonly DatabaseHelper _databaseHelper = new();

    public void CreateFlashcard(int stackId, string front, string back)
    {
        using var connection = _databaseHelper.GetConnection();
        string sql = "INSERT INTO flashcards(card_front, card_back, stack_id) VALUES (@Front, @Back, @StackId)";
        connection.Execute(sql, new { Front = front, Back = back, StackId = stackId});
    }
    
    public List<FlashcardDto> ListAllFlashcards(int stackId)
    {
        using var connection = _databaseHelper.GetConnection();
        var sql = "SELECT ROW_NUMBER() OVER (ORDER BY card_id) AS ViewId," +
                        "card_id AS RealId," +
                        "card_front AS Front, " +
                        "card_back AS Back " +
                        "FROM flashcards " +
                        "WHERE stack_id = @StackId ";
        return connection.Query<FlashcardDto>(sql, new {StackId = stackId}).ToList();
    }
    
    public void EditFlashcard(int flashcardId, EditFlashcardOptions option, string newText)
    {
        using var connection = _databaseHelper.GetConnection();
        switch (option)
        {
            case EditFlashcardOptions.EditFront:
                var sql = "UPDATE flashcards SET card_front = @Front WHERE card_id = @FlashcardId";
                connection.Execute(sql, new { Front = newText, FlashcardId = flashcardId });
                break;
            case EditFlashcardOptions.EditBack:
                var sql2 = "UPDATE flashcards SET card_back = @Back WHERE card_id = @FlashcardId";
                connection.Execute(sql2, new { Back = newText, FlashcardId = flashcardId });
                break;
        }
        
    }
    
    public void DeleteFlashcard(int flashcardId)
    {
        using var connection = _databaseHelper.GetConnection();
        var sql = "DELETE FROM flashcards WHERE card_id = @FlashcardId";
        connection.Execute(sql, new { FlashcardId = flashcardId });
    }
}