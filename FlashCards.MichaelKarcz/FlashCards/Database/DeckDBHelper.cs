using Dapper;
using FlashCards.DTOs;
using FlashCards.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace FlashCards.Database
{
    internal static class DeckDBHelper
    {
        private static string CONNECTION_STRING = ConfigurationManager.AppSettings.Get("flashcardsConnectionString");

        internal static bool CheckForRecords()
        {
            SqlConnection conn = GeneralDBHelper.CreateSQLConnection(CONNECTION_STRING);
            string sql = @"SELECT TOP (1) * FROM Decks";

            List<DeckDTO> returnedRecords = conn.Query<DeckDTO>(sql).ToList();

            return returnedRecords.Count > 0;
        }

        internal static List<Deck> GetAllDecks()
        {
            SqlConnection conn = GeneralDBHelper.CreateSQLConnection(CONNECTION_STRING);
            string sql = @"SELECT * FROM Decks";
            List<Deck> returnedRecords = conn.Query<Deck>(sql).ToList();

            return returnedRecords;
        }

        internal static int InsertDeck(Deck deck)
        {
            SqlConnection conn = GeneralDBHelper.CreateSQLConnection(CONNECTION_STRING);
            string sql = @"INSERT INTO Decks (Name) VALUES (@Name);
                            SELECT CAST(SCOPE_IDENTITY() as int)";
            int newId = conn.QuerySingle<int>(sql, new { Name = deck.Name });

            return newId;
        }

        internal static bool UpdateDeckById(int id, Deck deck)
        {
            SqlConnection conn = GeneralDBHelper.CreateSQLConnection(CONNECTION_STRING);
            string sql = @"UPDATE Decks
                            SET Name = @Name
                            WHERE Id = @Id";
            int rowsAffected = conn.Execute(sql, new { Name = deck.Name, Id = id });

            return rowsAffected > 0;
        }

        internal static bool DeleteDeckById(int id)
        {
            SqlConnection conn = GeneralDBHelper.CreateSQLConnection(CONNECTION_STRING);
            string sql = @"DELETE FROM Decks WHERE Id = @Id";
            int rowsAffected = conn.Execute(sql, new { Id = id });

            return rowsAffected > 0;
        }
    }
}
