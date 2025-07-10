using Dapper;
using FlashCards.DTOs;
using FlashCards.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace FlashCards.Database;
internal static class DeckDBHelper
{
    private static string CONNECTION_STRING = ConfigurationManager.AppSettings.Get("flashcardsConnectionString");

    internal static bool CheckForRecords()
    {
        SqlConnection conn = GeneralDBHelper.CreateSqlConnection(CONNECTION_STRING);
        string sql = @"SELECT TOP (1) * FROM Decks";

        List<DeckDto> returnedRecords = conn.Query<DeckDto>(sql).ToList();

        return returnedRecords.Count > 0;
    }

    internal static List<Deck> GetAllDecks()
    {
        SqlConnection conn = GeneralDBHelper.CreateSqlConnection(CONNECTION_STRING);
        string sql = @"SELECT * FROM Decks";
        List<Deck> returnedRecords = conn.Query<Deck>(sql).ToList();

        return returnedRecords;
    }

    internal static List<string> GetAllDeckNames()
    {
        SqlConnection conn = GeneralDBHelper.CreateSqlConnection(CONNECTION_STRING);
        string sql = @"SELECT Name FROM Decks";
        List<Deck> returnedRecords = conn.Query<Deck>(sql).ToList();

        List<string> deckNames = new List<string>();

        foreach (Deck d in returnedRecords)
        {
            deckNames.Add(d.Name);
        }

        return deckNames;
    }

    internal static int InsertDeck(Deck deck)
    {
        SqlConnection conn = GeneralDBHelper.CreateSqlConnection(CONNECTION_STRING);
        string sql = @"INSERT INTO Decks (Name) VALUES (@Name);
                            SELECT CAST(SCOPE_IDENTITY() as int)";
        int newId = conn.QuerySingle<int>(sql, new { Name = deck.Name });

        return newId;
    }

    internal static bool UpdateDeckById(int id, Deck deck)
    {
        SqlConnection conn = GeneralDBHelper.CreateSqlConnection(CONNECTION_STRING);
        string sql = @"UPDATE Decks
                            SET Name = @Name
                            WHERE Id = @Id";
        int rowsAffected = conn.Execute(sql, new { Name = deck.Name, Id = id });

        return rowsAffected > 0;
    }

    internal static bool DeleteDeckById(int id)
    {
        SqlConnection conn = GeneralDBHelper.CreateSqlConnection(CONNECTION_STRING);
        string sql = @"DELETE FROM Decks WHERE Id = @Id";
        int rowsAffected = conn.Execute(sql, new { Id = id });

        return rowsAffected > 0;
    }
}
