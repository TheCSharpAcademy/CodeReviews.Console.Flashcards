using Flashcards.Database;
using Flashcards.Services;

namespace Flashcards.Application;

public class AppConfigurationHandler
{
    private DatabaseInitializer _dbInitializer;
    private DatabaseUtility _dbValidater;
    private InputHandler _inputHandler;

    public AppConfigurationHandler(DatabaseContext dbContext, InputHandler inputHandler)
    {
        _dbInitializer = new DatabaseInitializer(dbContext);
        _dbValidater = new DatabaseUtility(dbContext);
        _inputHandler = inputHandler;
    }

    public void ConfigureApplication()
    {
        if (!_dbValidater.CheckDatabaseExists())
        {
            _dbInitializer.InitializeDatabase();
        }

        if (!_dbValidater.ValidateDatabaseSchema())
        {
            bool recreateDatabase = _inputHandler.ConfirmAction("Database schema is invalid. Would you like to recreate the database?");
            
            if (recreateDatabase)
            {
                _dbValidater.DropDatabase();
                _dbInitializer.InitializeDatabase();
            }
            else
            {
                Utilities.DisplayInformationConsoleMessage("Exiting application.");
                Environment.Exit(1);
            }

            ConfigureApplication();
        }
    }
}
