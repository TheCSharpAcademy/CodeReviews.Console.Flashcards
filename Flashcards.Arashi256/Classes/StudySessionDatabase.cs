using Dapper;
using Flashcards.Arashi256.Config;
using Flashcards.Arashi256.Models;
using System.Data.SqlClient;

namespace Flashcards.Arashi256.Classes
{
    internal class StudySessionDatabase
    {
        private Database _database;
        private DatabaseConnection _connection;

        public StudySessionDatabase()
        {
            _database = new Database();
            _database.TestConnection();
            _connection = _database.GetConnection();
        }

        public int AddNewStudySession(StudySession s)
        {
            var sql = "INSERT INTO dbo.studysessions (StackId, TotalCards, Score, DateStudied) VALUES (@StackId, @TotalCards, @Score, @DateStudied)";
            var parameters = new DynamicParameters();
            parameters.Add("@StackId", s.StackId);
            parameters.Add("@TotalCards", s.TotalCards);
            parameters.Add("@Score", s.Score);
            parameters.Add("@DateStudied", s.DateStudied.ToString("yyyy-MM-dd"));
            return _database.ExecuteQuery(sql, parameters);
        }

        public List<StudySession> GetStudySessionResults(string query, DynamicParameters? parameters = null)
        {
            List<StudySession> results = new List<StudySession>();
            using (var connection = new SqlConnection(_connection.DatabaseConnectionString))
            {
                try
                {
                    results = connection.Query<StudySession>(query, parameters).AsList();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("SQL ERROR: " + sqlEx.Message);
                }
                catch (InvalidOperationException invalidOpEx)
                {
                    Console.WriteLine("INVALID OPERATION: " + invalidOpEx.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GENERAL ERROR: " + ex.Message);
                }
            }
            return results;
        }

        public List<StudySessionReportPerStackDto> GetStudySessionReportForStack(string query, DynamicParameters? parameters = null)
        {
            List<StudySessionReportPerStackDto> report = new List<StudySessionReportPerStackDto>();
            using (var connection = new SqlConnection(_connection.DatabaseConnectionString))
            {
                try
                {
                    report = connection.Query<StudySessionReportPerStackDto>(query, parameters).AsList();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("SQL ERROR: " + sqlEx.Message);
                }
                catch (InvalidOperationException invalidOpEx)
                {
                    Console.WriteLine("INVALID OPERATION: " + invalidOpEx.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GENERAL ERROR: " + ex.Message);
                }
            }
            return report;
        }
    }
}
