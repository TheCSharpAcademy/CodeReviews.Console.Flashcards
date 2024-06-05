using Dapper;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Flashcards.Database;

public class StackDatabaseManager
{
    private string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");

    internal void InsertStack(CardStack stack)
    {
        var sql = $@"INSERT INTO Cardstack (CardstackName) VALUES (@CardstackName)";
        using (var connection = new SqlConnection(connectionStr))
        {
            connection.Execute(sql, stack);
        }
    }

    internal List<CardStack> GetStacks()
    {
        using (var connection = new SqlConnection(connectionStr))
        {
            var sql = @"SELECT * FROM Cardstack";

            var readStacks = connection.Query<CardStack>(sql).ToList();
            return readStacks;
        }
    }

    internal void UpdateStack(CardStack cardStack, string cardStackName)
    {
        using (var connection = new SqlConnection(connectionStr))
        {
            var sql = $@"UPDATE Cardstack SET CardstackName = '{cardStackName}' WHERE CardstackId = {cardStack.CardstackId}";

            connection.Execute(sql, cardStack);
        }
    }

    internal void DeleteStack(CardStack cardStack)
    {
        using (var connection = new SqlConnection(connectionStr))
        {
            var sql = $@"DELETE FROM Cardstack WHERE CardstackId = {cardStack.CardstackId} ";
            connection.Execute(sql, cardStack);
        }
    }
}