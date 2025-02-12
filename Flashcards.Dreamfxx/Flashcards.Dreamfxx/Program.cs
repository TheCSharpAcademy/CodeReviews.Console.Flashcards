using Flashcards.Dreamfxx.Data;
using Flashcards.Dreamfxx.Services;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

var databaseManager = new DatabaseManager(connectionString);
var stacksService = new StacksService(databaseManager);
var flashcardsService = new FlashcardsService(databaseManager);
var sessionsService = new SessionsService(databaseManager);

