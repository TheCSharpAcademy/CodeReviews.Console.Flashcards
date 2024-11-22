using System.Data.SqlClient;
using Dapper;
using FlashCards.Data;

namespace FlashCards.Controllers;

public class StudyController
{
    private readonly DatabaseService DbService = new DatabaseService();
    
    public List<StudyDto> GetAllStudySessions(int stack_id) => ExecuteQuery("SELECT studyDate, score, flashcard_amount FROM Study WHERE stack_id = @stack_id", new { stack_id });
    public void InsertStudySession(string date, int score, int flashcardAmount, int  stack_id)
    {
        var query = @"INSERT INTO Study(studyDate, score, flashcard_amount, stack_id) VALUES (@Date, @Score, @FlashcardAmount, @StackId)";
        var parameters = new { Date = date, Score = score , FlashcardAmount = flashcardAmount,  StackId = stack_id };
        ExecuteNonQuery(query, parameters);
    }
    
    public void DeleteAll(int stack_id) => ExecuteNonQuery("DELETE FROM Study WHERE stack_id = @Stack_id", new { Stack_Id = stack_id});
    
    private List<StudyDto> ExecuteQuery(string query, object? parameters = null)
    {
        using SqlConnection connection = DbService.GetConnection();
        return connection.Query<StudyDto>(query, parameters).ToList();
    }
    
    private void ExecuteNonQuery(string query, object? parameters = null)
    {
        using SqlConnection connection = DbService.GetConnection();
        connection.Execute(query, parameters);
    }
}