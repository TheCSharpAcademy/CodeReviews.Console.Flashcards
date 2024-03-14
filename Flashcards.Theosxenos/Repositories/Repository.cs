namespace Flashcards.Repositories;

public class Repository
{
    private readonly FlaschardDatabase db = new();

    public void CreateStack(Stack stack)
    {
        try
        {
            using var connection = db.GetConnection();
            connection.Execute("INSERT INTO Stacks (Name) VALUES (@Name);", stack);
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            throw new ArgumentException($"A stack with the name '{stack.Name}' already exists.");
        }
    }

    public List<Stack> GetAllStacks()
    {
        using var connection = db.GetConnection();
        var query = connection.QueryMultiple("SELECT * FROM Stacks; SELECT * FROM Flashcards ORDER BY Position;");
        var stacks = query.Read<Stack>().ToList();
        var flashcards = query.Read<Flashcard>().ToList();

        foreach (var stack in stacks) stack.Flashcards = flashcards.Where(f => f.StackId == stack.Id).ToList();

        return stacks;
    }

    public void UpdateStack(Stack stack)
    {
        try
        {
            using var connection = db.GetConnection();
            connection.Execute("UPDATE Stacks SET Name = @Name WHERE Id = @Id", stack);
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            throw new ArgumentException($"A stack with the name '{stack.Name}' already exists.");
        }
    }

    public void DeleteStack(Stack stack)
    {
        using var connection = db.GetConnection();
        connection.Execute("DELETE FROM Stacks WHERE Id = @Id", new { stack.Id });
    }
}