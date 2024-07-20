using System.Configuration;
using Flashcards.Models;

namespace Flashcards.Controllers;

public static class StackController
{
    private static readonly string ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;

    public static List<StackDTO> GetAllStacks()
    {
        using var connection = new SqlConnection(ConnectionString);

        string sql = "SELECT * FROM stacks;";
        var results = connection.Query<StackDTO>(sql);

        return results.ToList();
    }

    public static void CreateStack(string name)
    {
        using var connection = new SqlConnection(ConnectionString);

        string sql = "INSERT INTO stacks (name) VALUES (@Name);";
        var parameters = new { Name = name };

        connection.Execute(sql, parameters);
    }
}