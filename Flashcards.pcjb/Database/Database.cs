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
}