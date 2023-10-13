using System.Configuration;
using Flashcards.wkktoria;
using Flashcards.wkktoria.Database;
using Flashcards.wkktoria.Services;

var databaseName = ConfigurationManager.AppSettings.Get("DatabaseName");
var databasePassword = ConfigurationManager.AppSettings.Get("DatabasePassword");
var connectionString = $@"Server=localhost,1433;User Id=sa;Password={databasePassword};TrustServerCertificate=true;";

var databaseManager = new DatabaseManager(connectionString, databaseName!);
databaseManager.Initialize();

var stackService = new StackService(connectionString, databaseName!);
var cardService = new CardService(connectionString, databaseName!);

var ui = new UserInterface(stackService, cardService);
ui.Run();