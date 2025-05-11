using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Flashcards.glaxxie.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace Flashcards.glaxxie.Controllers;

internal class StackController
{
    internal static List<StackViewer> GetAllStacks()
    {
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        string cmdStr = $@"
        SELECT 
            s.stack_id AS StackId, 
            s.stack_name AS Name,
            COUNT(c.card_id) AS Count
        FROM {Tables.Stacks} s
        LEFT JOIN {Tables.Cards} c ON s.stack_id = c.stack_id
        GROUP BY s.stack_id, s.stack_name
        ";
        return [.. conn.Query<StackViewer>(cmdStr)];
    }

    internal static void Insert(StackCreation stack)
    {
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        string cmdStr = $"INSERT INTO {Tables.Stacks} (stack_name) VALUES (@Name)";
        conn.Execute(cmdStr, new {stack.Name});
    }

    internal static void Update(StackModification stack)
    {
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        string cmdStr = $"UPDATE {Tables.Stacks} SET stack_name = @Name WHERE stack_id = @StackId";
        conn.Execute(cmdStr, new { stack.Name, stack.StackId });
    }

    internal static void Delete(int StackId)
    {
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        string cmdStr = $"DELETE FROM {Tables.Stacks} WHERE stack_id = @StackId";
        conn.Execute(cmdStr, new { StackId });
    }
}
