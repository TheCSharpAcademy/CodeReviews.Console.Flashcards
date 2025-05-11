using Dapper;
using Flashcards.glaxxie.DTO;

namespace Flashcards.glaxxie.Controllers;

internal class CardController
{
    internal static List<CardViewer> GetAllCards()
    {
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        string cmdStr = $@"
            SELECT 
                c.card_id AS CardId, 
                c.stack_id AS StackId,
                c.front AS Front,
                c.back AS Back
            FROM {Tables.Cards} c
        ";
        return [.. conn.Query<CardViewer>(cmdStr)];
    }

    internal static List<CardViewer> GetCardsFromStack(int StackId)
    {
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        string cmdStr = $@"
            SELECT 
                c.card_id AS CardId, 
                c.stack_id AS StackId,
                c.front AS Front,
                c.back AS Back
            FROM {Tables.Cards} c
            WHERE c.stack_id = @StackId
        ";
        return [.. conn.Query<CardViewer>(cmdStr, new { StackId })];
    }

    internal static void Insert(CardCreation card)
    {
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        string cmdStr = $"INSERT INTO {Tables.Cards} (stack_id, front, back) VALUES (@StackId, @Front, @Back)";
        conn.Execute(cmdStr, new { card.StackId, card.Front, card.Back });
    }

    internal static void Update(CardViewer card)
    {
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        string cmdStr = $"UPDATE {Tables.Cards} SET front = @Front, back = @Back WHERE card_id = @CardId";
        conn.Execute(cmdStr, new { card.Front, card.Back, card.CardId });
    }

    internal static void Delete(IEnumerable<int> CardIds)
    {
        using var conn = DatabaseController.GetConnection();
        conn.Open();
        string cmdStr = $"DELETE FROM {Tables.Cards} WHERE card_id IN @CardIds";
        conn.Execute(cmdStr, new { CardIds });
    }
}