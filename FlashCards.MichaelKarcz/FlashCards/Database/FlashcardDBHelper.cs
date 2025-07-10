using Microsoft.Data.SqlClient;
using Dapper;
using FlashCards.Models;
using System.Configuration;
using FlashCards.DTOs;

namespace FlashCards.Database;
internal static class FlashcardDBHelper
{

    private static string CONNECTION_STRING = ConfigurationManager.AppSettings.Get("flashcardsConnectionString");

    internal static bool CheckForRecords()
    {
        SqlConnection conn = GeneralDBHelper.CreateSqlConnection(CONNECTION_STRING);
        string sql = @"SELECT TOP (1) * FROM Flashcards";

        List<FlashcardDto> returnedRecords = conn.Query<FlashcardDto>(sql).ToList();

        return returnedRecords.Count > 0;
    }

    internal static List<FlashcardDto> GetAllFlashcards()
    {
        SqlConnection conn = GeneralDBHelper.CreateSqlConnection(CONNECTION_STRING);
        string sql = @"SELECT * FROM Flashcards ORDER BY DeckId";
        List<FlashcardDto> returnedRecords = conn.Query<FlashcardDto>(sql).ToList();

        return returnedRecords;
    }

    internal static List<FlashcardDto> GetAllFlashcardsInDeck(int deckId)
    {
        SqlConnection conn = GeneralDBHelper.CreateSqlConnection(CONNECTION_STRING);
        string sql = @"SELECT * FROM Flashcards WHERE DeckId = @DeckId";
        List<FlashcardDto> returnedRecords = conn.Query<FlashcardDto>(sql, new {DeckId = deckId }).ToList();

        return returnedRecords;
    }

    internal static bool InsertFlashcard(Flashcard flashcard)
    {
        SqlConnection conn = GeneralDBHelper.CreateSqlConnection(CONNECTION_STRING);
        string sql = @"INSERT INTO Flashcards (Front, Back, DeckId) 
                            VALUES (@Front, @Back, @DeckId)";
        int rowsAffected = conn.Execute(sql, new { Front = flashcard.Front, Back = flashcard.Back, DeckId = flashcard.DeckId });

        return rowsAffected > 0;
    }

    internal static bool UpdateFlashcardById(int id, Flashcard flashcard)
    {
        SqlConnection conn = GeneralDBHelper.CreateSqlConnection(CONNECTION_STRING);
        string sql = @"UPDATE Flashcards
                            SET Front = @Front, Back = @Back, DeckId = @DeckId
                            WHERE Id = @Id";
        int rowsAffected = conn.Execute(sql, new { Front = flashcard.Front, Back = flashcard.Back, DeckId = flashcard.DeckId, Id = id });

        return rowsAffected > 0;
    }

    internal static bool DeleteFlashcardById(int id)
    {
        SqlConnection conn = GeneralDBHelper.CreateSqlConnection(CONNECTION_STRING);
        string sql = @"DELETE FROM Flashcards WHERE Id = @Id";
        int rowsAffected = conn.Execute(sql, new { Id = id });

        return rowsAffected > 0;
    }

    internal static bool DeleteAllFlashcardsInDeck(int deckId)
    {
        SqlConnection conn = GeneralDBHelper.CreateSqlConnection(CONNECTION_STRING);
        string sql = @"DELETE FROM Flashcards WHERE DeckId = @DeckId";
        int rowsAffected = conn.Execute(sql, new { DeckId = deckId });

        return rowsAffected > 0;
    }
}
