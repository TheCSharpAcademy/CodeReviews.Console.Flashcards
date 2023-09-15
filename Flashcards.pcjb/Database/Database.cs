namespace Flashcards;

using System.Data.SqlClient;

class Database
{
    private readonly string databaseConnectionString;

    public Database(string? databaseConnectionString)
    {
        if (String.IsNullOrEmpty(databaseConnectionString))
        {
            throw new InvalidOperationException("Database connection string is empty or not configured.");
        }
        this.databaseConnectionString = databaseConnectionString;
    }

    public List<Stack> ReadAllStacks()
    {
        List<Stack> stacks = new();
        try
        {
            using var connection = new SqlConnection(databaseConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            SELECT id, name
            FROM stacks
            ORDER BY name ASC
            ";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                stacks.Add(new Stack(id, name));
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return stacks;
    }

    public Stack? ReadStackById(long stackId)
    {
        Stack? stack = null;
        try
        {
            using var connection = new SqlConnection(databaseConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            SELECT id, name
            FROM stacks
            WHERE id=@id
            ";
            command.Parameters.AddWithValue("@id", stackId);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                stack = new Stack(id, name);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return stack;
    }

    public Stack? ReadStackByName(string stackName)
    {
        Stack? stack = null;
        try
        {
            using var connection = new SqlConnection(databaseConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            SELECT id, name
            FROM stacks
            WHERE name=@name
            ";
            command.Parameters.AddWithValue("@name", stackName);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                stack = new Stack(id, name);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return stack;
    }

    public bool CreateStack(string name)
    {
        try
        {
            using var connection = new SqlConnection(databaseConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO stacks (name) VALUES (@name)";
            command.Parameters.AddWithValue("@name", name);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public bool UpdateStack(long id, string name)
    {
        try
        {
            using var connection = new SqlConnection(databaseConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            UPDATE stacks 
            SET name=@name 
            WHERE id=@id
            ";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public bool DeleteStack(long stackId)
    {
        try
        {
            using var connection = new SqlConnection(databaseConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            DELETE FROM stacks 
            WHERE id=@id
            ";
            command.Parameters.AddWithValue("@id", stackId);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public List<Flashcard> ReadAllFlashcardsOfStack(long stackId)
    {
        List<Flashcard> cards = new();
        try
        {
            using var connection = new SqlConnection(databaseConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            SELECT id, front, back
            FROM flashcards
            WHERE stack_id=@stack_id
            ";
            command.Parameters.AddWithValue("@stack_id", stackId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var front = reader.GetString(1);
                var back = reader.GetString(2);
                cards.Add(new Flashcard(id, stackId, front, back));
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return cards;
    }

    public bool CreateFlashcard(long stackId, string front, string back)
    {
        try
        {
            using var connection = new SqlConnection(databaseConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            INSERT INTO flashcards 
            (stack_id, front, back) 
            VALUES 
            (@stack_id, @front, @back)";
            command.Parameters.AddWithValue("@stack_id", stackId);
            command.Parameters.AddWithValue("@front", front);
            command.Parameters.AddWithValue("@back", back);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public Flashcard? ReadFlashcardById(long cardId)
    {
        Flashcard? card = null;
        try
        {
            using var connection = new SqlConnection(databaseConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            SELECT id, stack_id, front, back
            FROM flashcards
            WHERE id=@id
            ";
            command.Parameters.AddWithValue("@id", cardId);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var id = reader.GetInt32(0);
                var stackId = reader.GetInt32(1);
                var front = reader.GetString(2);
                var back = reader.GetString(3);
                card = new Flashcard(id, stackId, front, back);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return card;
    }

    public bool UpdateFlashcard(long cardId, string front, string back)
    {
        try
        {
            using var connection = new SqlConnection(databaseConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            UPDATE flashcards 
            SET front=@front, back=@back 
            WHERE id=@id 
            ";
            command.Parameters.AddWithValue("@id", cardId);
            command.Parameters.AddWithValue("@front", front);
            command.Parameters.AddWithValue("@back", back);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public bool DeleteFlashcard(long cardId)
    {
        try
        {
            using var connection = new SqlConnection(databaseConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            DELETE FROM flashcards 
            WHERE id=@id
            ";
            command.Parameters.AddWithValue("@id", cardId);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public bool CreateStudySession(StudySession session)
    {
        try
        {
            using var connection = new SqlConnection(databaseConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            INSERT INTO study_sessions 
            (stack_id, completed_at, score_percent) 
            VALUES 
            (@stack_id, @completed_at, @score_percent)";
            command.Parameters.AddWithValue("@stack_id", session.StackId);
            command.Parameters.AddWithValue("@completed_at", session.CompletedAt);
            command.Parameters.AddWithValue("@score_percent", session.ScorePercent);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }
}