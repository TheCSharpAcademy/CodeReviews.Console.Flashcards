namespace FlashcardsProgram.Database;

using Dapper;
using System.Reflection;

public class BaseRepository<Entity>
{
    public string TableName;

    public BaseRepository(string tableName)
    {
        TableName = tableName;
    }
    public Entity? Create<CreateDTO>(CreateDTO createPayload)
    {
        try
        {
            if (createPayload == null)
            {
                throw new Exception("Payload must be provided");
            }

            PropertyInfo[] properties = typeof(CreateDTO).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            List<string> fieldNames = properties.Select(field => field.Name).ToList();

            string columnNames = string.Join(", ", fieldNames);
            string valueParams = string.Join(", ", fieldNames.Select(fieldName => $"@{fieldName}"));

            string sql =
                $@"
                    INSERT INTO {TableName} ({columnNames})
                    OUTPUT INSERTED.*
                    VALUES ({valueParams})
                ";
            Console.WriteLine($"\n\nCREATE FLASHCARD {sql}");
            var output = ConnectionManager.Connection.QuerySingleOrDefault<Entity>(sql, createPayload);

            return output;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Create failed. {ex.Message}");
        }

        return default;
    }

    public List<Entity> List()
    {
        try
        {
            string sql =
                $@"
                    SELECT * FROM {TableName};
                ";

            var output = ConnectionManager.Connection.Query<Entity>(sql).ToList();

            return output;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Could not list. {ex.Message}");
        }

        return [];
    }

    public Entity? Update<UpdateDTO>(int id, UpdateDTO updatePayload)
    {
        try
        {
            if (updatePayload == null)
            {
                throw new Exception("Payload must be provided");
            }

            PropertyInfo[] properties = typeof(UpdateDTO).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            List<string> fieldNames = properties.Select(field => field.Name).ToList();

            string columnValuePairs = string.Join(
                ", ",
                fieldNames
                    .Where(fieldName => fieldName != null)
                    .Select(fieldName => $"{fieldName} = @{fieldName}")
            );

            if (columnValuePairs.Length == 0)
            {
                return default;
            }

            string sql =
                $@"
                    UPDATE {TableName}
                    SET {columnValuePairs}
                    OUTPUT inserted.*
                    WHERE Id = {id}
                ";

            var output = ConnectionManager.Connection.QuerySingleOrDefault<Entity>(sql, updatePayload);

            return output;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Update failed. {ex.Message}");
        }

        return default;
    }

    public bool Delete(int id)
    {
        try
        {
            string sql =
                $@"
                    DELETE FROM {TableName}
                    WHERE Id = @Id
                ";

            int rowsAffected = ConnectionManager.Connection.Execute(sql, new { Id = id });

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Delete failed. {ex.Message}");
        }

        return default;
    }

    public Entity GetById(int id)
    {
        try
        {
            string sql =
                $@"
                    SELECT * FROM {TableName}
                    WHERE Id = @Id
                ";

            return ConnectionManager.Connection.QuerySingle<Entity>(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not get {TableName} ID {id}. {ex.Message}");
            throw;
        }
    }
}