using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Flashcards.UndercoverDev.Repository.Session
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
    }
}