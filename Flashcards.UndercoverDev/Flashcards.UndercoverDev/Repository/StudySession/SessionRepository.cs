using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using Flashcards.UndercoverDev.Models;

namespace Flashcards.UndercoverDev.Repository.StudySessions
{
    public class SessionRepository : ISessionRepository
    {
        const string _databaseName = "FlashcardDB";
        private readonly string _connectionStringTemplate = ConfigurationManager.AppSettings.Get("ConnectionString") ?? "";
        private readonly string _connectionString;

        public SessionRepository()
        {
            _connectionString = string.Format(_connectionStringTemplate, _databaseName);
        }

        public void Post(int stackId, int score, int totalQuestions)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            string insertStudySessionQuery = @"
                INSERT INTO StudySessions (StackId, SessionDate, Score, TotalQuestions)
                VALUES (@StackId, @SessionDate, @Score, @TotalQuestions);";

            connection.Execute(insertStudySessionQuery, new
            {
                StackId = stackId,
                SessionDate = DateTime.Now,
                Score = score,
                TotalQuestions = totalQuestions,
            });
        }

        public List<Session> GetSessionsByStackId(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            string getSessionsQuery = $"SELECT * FROM StudySessions WHERE StackId = '{id}';";

            return connection.Query<Session>(getSessionsQuery).ToList();
        }

        public void Delete(Session session)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            string deleteSessionQuery = $"DELETE FROM StudySessions WHERE Id = '{session.Id}';";
            connection.Execute(deleteSessionQuery);
        }

        public List<Session> GetSessionsByYear(int year)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            string getSessionsQuery = $"SELECT * FROM StudySessions WHERE YEAR(SessionDate) = @Year;";

            return connection.Query<Session>(getSessionsQuery, new {Year = year}).ToList();
        }
    }
}