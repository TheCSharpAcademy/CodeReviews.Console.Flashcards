using Dapper;
using DatabaseLibrary.Models;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace DatabaseLibrary;

public class FlashcardsDataAccess
{
  private string _connectionString { get; set; }

  public FlashcardsDataAccess(string connectionString)
  {
    _connectionString = connectionString;
  }

  public List<FlashcardDTO> GetFlashcardsList(int? stackId)
  {
    using SqlConnection connection = new SqlConnection(_connectionString);

    string sql = @"SELECT f.display_id, f.question, f.answer, s.name AS stack_name 
                    FROM flashcards f
                    JOIN stacks s ON f.stack_id=s.stack_id
                    WHERE f.stack_id=@StackId";

    List<FlashcardDTO> flashcards = connection.Query<FlashcardDTO>(sql, new { StackId = stackId }).ToList();

    return flashcards;
  }

  public bool GetAllFlashcards(List<FlashcardDTO> flashcards)
  {
    if (flashcards.Count == 0)
    {
      AnsiConsole.Markup("[red]Flashcards list is empty.[/] Create one first.");
      return false;
    }

    flashcards = flashcards.OrderBy(flashcard => flashcard.Display_Id).ToList();

    ConsoleEngine.ShowFlashcardsTable(flashcards);
    return true;
  }

  public bool InsertFlashcard(int? stackId, string? question, string? answer)
  {
    using SqlConnection connection = new SqlConnection(_connectionString);

    int nextDisplayId = GetNextDisplayId(stackId);

    string sql = "INSERT INTO flashcards(question, answer, stack_id, display_id) VALUES(@Question, @Answer, @StackId, @DisplayId)";

    int rowsAffected = connection.Execute(sql, new { Question = question, Answer = answer, StackId = stackId, DisplayId = nextDisplayId });

    if (rowsAffected == 0)
    {
      AnsiConsole.Markup("[red]Inserting Failed![/]");
      return false;
    }

    AnsiConsole.Markup("[green]Flashcard created successfully![/]");
    return true;
  }

  public bool UpdateFlashcard(int? stackId, int? displayId, int? newStackId, string? question, string? answer)
  {
    using SqlConnection connection = new SqlConnection(_connectionString);

    string sql = "UPDATE flashcards SET question=@Question, answer=@Answer, stack_id=@NewStackId WHERE display_id=@DisplayId AND stack_id=@StackId";

    int affectedRows = connection.Execute(sql, new { Question = question, Answer = answer, NewStackId = newStackId, DisplayId = displayId, StackId = stackId });

    if (affectedRows == 0)
    {
      AnsiConsole.Markup("[red]Updating Failed![/]");
      return false;
    }

    AnsiConsole.Markup("[green]Flashcard updated successfully![/]");
    return true;
  }

  public bool UpdateFlashcardId(int? stackId, int? displayId, int? newDisplayId)
  {
    using SqlConnection connection = new SqlConnection(_connectionString);

    string sql = "UPDATE flashcards SET display_id=@NewDisplayId WHERE display_id=@DisplayId AND stack_id=@StackId";

    int affectedRows = connection.Execute(sql, new { NewDisplayId = newDisplayId, DisplayId = displayId, StackId = stackId });

    if (affectedRows == 0)
    {
      AnsiConsole.Markup("[red]Updating Failed![/]");
      return false;
    }

    return true;
  }

  public bool DeleteFlashcard(int? displayId)
  {
    using SqlConnection connection = new SqlConnection(_connectionString);

    string sql = "DELETE FROM flashcards WHERE display_id=@DisplayId";

    int rowsAffected = connection.Execute(sql, new { DisplayId = displayId });

    if (rowsAffected == 0)
    {
      AnsiConsole.Markup("[red]Deleting Failed![/]");
      return false;
    }

    AnsiConsole.Markup("[green]Flashcard deleted successfully![/]");
    return true;
  }


  private int GetNextDisplayId(int? stackId)
  {
    using SqlConnection connection = new SqlConnection(_connectionString);

    string sql = "SELECT ISNULL(MAX(display_id), 0) + 1 FROM flashcards WHERE stack_id=@StackId";

    int nextDisplayId = connection.ExecuteScalar<int>(sql, new { StackId = stackId });

    return nextDisplayId;
  }
}