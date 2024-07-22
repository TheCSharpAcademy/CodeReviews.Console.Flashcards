namespace Flashcards.Arashi256.Config
{
    internal class DatabaseConnection
    {
        private string? _databaseConnectionString;

        public DatabaseConnection()
        {
            AppManager appManager = new AppManager();
            _databaseConnectionString = appManager.DatabaseConnectionString;
        }

        public string? DatabaseConnectionString { get { return _databaseConnectionString; } }
    }
}