using System.Configuration;

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
    }
}