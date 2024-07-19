using System.Configuration;
using Flashcards.Models;

namespace Flashcards.Controllers;

public static class StackController
{
    private static readonly string ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;

    public static List<StackDTO> GetAllStacks()
    {
        using var connection = new SqlConnection(ConnectionString);

        string SQL = "SELECT * FROM stacks;";
        var results = connection.Query<StackDTO>(SQL);

        return results.ToList();
    }
}