using Dapper;
using FlashCards.DTOs;
using FlashCards.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace FlashCards.Database
{
    internal static class StudySessionDBHelper
    {
        private static string CONNECTION_STRING = ConfigurationManager.AppSettings.Get("flashcardsConnectionString");

        internal static bool CheckForRecords()
        {
            SqlConnection conn = GeneralDBHelper.CreateSQLConnection(CONNECTION_STRING);
            string sql = @"SELECT TOP (1) * FROM StudySessions";
            List<StudySessionDTO> returnedRecords = conn.Query<StudySessionDTO>(sql).ToList();

            return returnedRecords.Count > 0;
        }

        internal static List<StudySessionDTO> GetAllStudySessions()
        {
            SqlConnection conn = GeneralDBHelper.CreateSQLConnection(CONNECTION_STRING);
            string sql = @"SELECT * FROM StudySessions ORDER BY DeckId, SessionDate";
            List<StudySessionDTO> returnedRecords = conn.Query<StudySessionDTO>(sql).ToList();

            return returnedRecords;
        }

        internal static List<StudySessionDTO> GetAllStudySessionsOfDeck(Deck deck)
        {
            SqlConnection conn = GeneralDBHelper.CreateSQLConnection(CONNECTION_STRING);
            string sql = @"SELECT * FROM StudySessions WHERE DeckId = @DeckId ORDER BY SessionDate";
            List<StudySessionDTO> returnedRecords = conn.Query<StudySessionDTO>(sql, new {DeckId = deck.Id}).ToList();

            return returnedRecords;
        }

        internal static bool InsertStudySession(StudySession studySession)
        {
            SqlConnection conn = GeneralDBHelper.CreateSQLConnection(CONNECTION_STRING);
            string sql = @"INSERT INTO StudySessions (Score, CardsStudied, SessionDate, DeckId, DeckName) VALUES (@Score, @CardsStudied, @SessionDate, @DeckId, @DeckName)";
            int rowsAffected = conn.Execute(sql, new { Score = studySession.Score, CardsStudied = studySession.CardsStudied, SessionDate = studySession.SessionDate, DeckId = studySession.DeckId, DeckName = studySession.DeckName});

            return  rowsAffected > 0;
        }

        internal static bool UpdateStudySessionById(int id, StudySession studySession)
        {
            SqlConnection conn = GeneralDBHelper.CreateSQLConnection(CONNECTION_STRING);
            string sql = @"UPDATE StudySessions
                            SET Score = @Score, SessionDate = @SessionDate, DeckId = @DeckId, DeckName = @DeckName
                            WHERE Id = @Id";
            int rowsAffected = conn.Execute(sql, new {Score = studySession.Score, SessionDate = studySession.SessionDate, DeckId = studySession.DeckId, DeckName = studySession.DeckName, Id = id });

            return rowsAffected > 0;
        }

        internal static bool DeleteStudySessionById(int id)
        {
            SqlConnection conn = GeneralDBHelper.CreateSQLConnection(CONNECTION_STRING);
            string sql = @"DELETE FROM StudySessions WHERE Id = @Id";
            int rowsAffected = conn.Execute(sql, new { Id = id });

            return rowsAffected > 0;
        }

        internal static bool DeleteAllStudySessionsOfDeck(int deckId)
        {
            SqlConnection conn = GeneralDBHelper.CreateSQLConnection(CONNECTION_STRING);
            string sql = @"DELETE FROM StudySessions WHERE DeckId = @DeckId";
            int rowsAffected = conn.Execute(sql, new { DeckId = deckId });

            return rowsAffected > 0;
        }
    }
}
