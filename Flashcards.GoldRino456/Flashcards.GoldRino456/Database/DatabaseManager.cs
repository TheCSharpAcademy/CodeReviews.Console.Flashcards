using Flashcards.GoldRino456.Database.Controllers;
using Microsoft.Extensions.Configuration;

namespace Flashcards.GoldRino456.Database
{
    internal class DatabaseManager
    {
        public static DatabaseManager Instance { get; } = new();
        private string _connectionString;

        //DB Controllers
        public FlashcardController FlashcardCtrl { get; private set; }
        public StackController StackCtrl { get; private set; }
        public StudySessionController StudySessionCtrl { get; private set; }

        public void InitializeDatabase()
        {
            FetchConnectionString(out _connectionString);
            InitializeControllers();
        }

        private void InitializeControllers()
        {
            StackCtrl = new StackController(_connectionString);
            FlashcardCtrl = new FlashcardController(_connectionString);
            StudySessionCtrl = new StudySessionController(_connectionString);
        }

        private void FetchConnectionString(out string connectionString)
        {
            IConfiguration config = new ConfigurationBuilder()
                        .AddJsonFile("appSettings.json")
                        .Build();

            connectionString = config.GetConnectionString("DefaultConnection");
        }
    }
}
