using Dapper;

public class FlashcardRepository
{
    private DatabaseManager _databaseManager;

    public FlashcardRepository(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public void CreateFlashcard(Flashcard flashcard)
    {
        using (var conn = _databaseManager.GetConnection())
        {
            var query = "INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)";
            conn.Execute(query, flashcard);
        }
    }

    public void DeleteFlashcard(int flashcardId)
    {
        using (var conn = _databaseManager.GetConnection())
        {
            var query = "DELETE FROM Flashcards WHERE Id = @id";
            conn.Execute(query, new { Id = flashcardId });
        }
    }

    public void UpdateCard(Flashcard flashcard)
    {
        using (var conn = _databaseManager.GetConnection())
        {
            var query = "UPDATE Flashcards SET Question = @question, Answer = @answer WHERE Id = @id";
            conn.Execute(query, flashcard);
        }
    }

    public List<Flashcard> GetFlashcardsByStack(int stackId)
    {
        using (var conn = _databaseManager.GetConnection())
        {
            var query = "SELECT * FROM Flashcards WHERE StackId = @stackId";
            return conn.Query<Flashcard>(query, new { StackId = stackId }).ToList();
        }
    }
}