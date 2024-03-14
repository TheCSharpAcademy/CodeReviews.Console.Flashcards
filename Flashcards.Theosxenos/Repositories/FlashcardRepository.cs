namespace Flashcards.Repositories;

public class FlashcardRepository
{
    private readonly FlaschardDatabase db = new();

    public void CreateFlashcard(Flashcard flashcard)
    {
        using var connection = db.GetConnection();
        connection.Execute(
            """
            INSERT INTO Flashcards (StackId, Title, Question, Answer, Position) 
            VALUES (@StackId, @Title, @Question, @Answer, @Position);
            """, flashcard);
    }

    public void UpdateFlashcard(Flashcard flashcard)
    {
        using var connection = db.GetConnection();
        connection.Execute(
            "UPDATE Flashcards SET StackId = @StackId, Title = @Title, Question = @Question, Answer = @Answer WHERE Id = @Id",
            flashcard);
    }

    public void DeleteFlashcards(List<Flashcard> flashcards)
    {
        using var connection = db.GetConnection();
        connection.Execute("DELETE FROM Flashcards WHERE Id = @Id", flashcards);
    }

    public void UpdateFlashcards(List<Flashcard> flashcards)
    {
        using var connection = db.GetConnection();
        connection.Execute(
            "UPDATE Flashcards SET StackId = @StackId, Title = @Title, Question = @Question, Answer = @Answer, Position = @Position WHERE Id = @Id",
            flashcards);
    }

    public int GetLastPositionByStackId(int stackId)
    {
        using var connection = db.GetConnection();
        return connection.ExecuteScalar<int>("SELECT MAX(Position) FROM Flashcards WHERE StackId = @stackId", new { stackId });
    }
}