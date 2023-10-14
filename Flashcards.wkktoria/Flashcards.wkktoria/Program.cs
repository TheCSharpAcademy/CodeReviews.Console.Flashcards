using System.Configuration;
using Flashcards.wkktoria.Database;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions;

var databaseName = ConfigurationManager.AppSettings.Get("DatabaseName");
var databasePassword = ConfigurationManager.AppSettings.Get("DatabasePassword");
var connectionString = $"Server=localhost,1433;User Id=sa;Password={databasePassword};TrustServerCertificate=true;";

var databaseManager = new DatabaseManager(connectionString, databaseName!);
databaseManager.Initialize();
databaseManager.CreateTables();

var stackService = new StackService(connectionString, databaseName!);
var cardService = new CardService(connectionString, databaseName!);
var sessionService = new SessionService(connectionString, databaseName!);

var ui = new UserInterface(stackService, cardService, sessionService);
ui.Run();