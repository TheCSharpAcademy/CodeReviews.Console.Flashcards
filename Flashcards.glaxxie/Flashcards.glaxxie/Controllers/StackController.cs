using Dapper;
using Flashcards.glaxxie.DTO;

namespace Flashcards.glaxxie.Controllers;

internal class StackController
{
    internal static List<StackViewer> GetAllStacks()
    {
        string cmdStr = $@"
        SELECT 
            s.stack_id AS StackId, 
            s.stack_name AS Name,
            COUNT(c.card_id) AS Count
        FROM {Tables.Stacks} s
        LEFT JOIN {Tables.Cards} c ON s.stack_id = c.stack_id
        GROUP BY s.stack_id, s.stack_name
        ";
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        return [.. conn.Query<StackViewer>(cmdStr)];
    }

    internal static int Insert(StackCreation stack)
    {
        string cmdStr = $@"
            INSERT INTO {Tables.Stacks} (stack_name)
            OUTPUT INSERTED.stack_id
            VALUES (@Name)";
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        return conn.ExecuteScalar<int>(cmdStr, new { stack.Name });
    }

    internal static void Update(StackModification stack)
    {
        string cmdStr = $"UPDATE {Tables.Stacks} SET stack_name = @Name WHERE stack_id = @StackId";
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        conn.Execute(cmdStr, new { stack.Name, stack.StackId });
    }

    internal static void Delete(int StackId)
    {
        string cmdStr = $"DELETE FROM {Tables.Stacks} WHERE stack_id = @StackId";
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        conn.Execute(cmdStr, new { StackId });
    }

    internal static bool StackExists(string Name)
    {
        string cmdStr = $"SELECT COUNT(1) FROM {Tables.Stacks} WHERE stack_name = @Name";
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        return conn.ExecuteScalar<int>(cmdStr, new { Name }) > 0;
    }
}