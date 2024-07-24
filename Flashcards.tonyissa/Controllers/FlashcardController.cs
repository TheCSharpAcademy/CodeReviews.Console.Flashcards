using System.Configuration;
using Flashcards.Models;

namespace Flashcards.Controllers;

public static class CardController
{
    private static readonly string ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;

    public static List<CardDTO> GetCardDTOList(int stackId)
    {
        using var connection = new SqlConnection(ConnectionString);

        string sql = "SELECT cards.front,cards.back,stacks.name AS stackname FROM cards JOIN stacks ON stackid = stacks.id WHERE stackid = @StackId;";
        var parameters = new { StackId = stackId };
        var results = connection.Query<CardDTO>(sql, parameters);

        return results.ToList();
    }

    public static void CreateCard(string front, string back, int stackId)
    {
        using var connection = new SqlConnection(ConnectionString);

        string sql = "INSERT INTO cards (front, back, stackid) VALUES (@Front, @Back, @StackId);";
        var parameters = new { Front = front, Back = back, StackId = stackId };
        connection.Execute(sql, parameters);
    }
}