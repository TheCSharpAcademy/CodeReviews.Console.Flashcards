using Flashcards.Dreamfxx.Data;
using Flashcards.Dreamfxx.Models;
using Flashcards.Dreamfxx.Services;
using Spectre.Console;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

string? connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string was not found. Check the appsettings.json file.");

var dbManager = new DatabaseManager(connectionString);
var stackService = new StacksService(dbManager);
var cardService = new FlashcardsService(dbManager);
var sessionsService = new SessionsService(dbManager);

dbManager.EnsureDatabaseExists();

var menuRoutes = new List<MenuRoute>
{
    new MenuRoute { Id = 1, Route = "Create new stack of cards" },
    new MenuRoute { Id = 2, Route = "Edit existing stack/s" },
    new MenuRoute { Id = 3, Route = "Delete existing stack/s" },

    new MenuRoute{ Id = 4, Route = "Create new flashcard" },
    new MenuRoute{ Id = 5, Route = "Edit existing flashcard/s" },
    new MenuRoute{ Id = 6, Route = "Delete existing flashcard/s" },

    new MenuRoute{ Id = 7, Route = "Start a session" },
    new MenuRoute{ Id = 8, Route = "Show study sessions in each month" },

    new MenuRoute{ Id = 9, Route = "Drop and recreate tables" },
    new MenuRoute{ Id = 10, Route = "Exit" }
};

bool running = true;

while (running)
{
    Console.Clear();

    var menuSelection = AnsiConsole.Prompt(
        new SelectionPrompt<MenuRoute>()
        .Title("What you want to do?")
        .PageSize(10)
        .AddChoices(menuRoutes)
        .UseConverter(option => option.Route));


    switch (menuSelection.Id)
    {
        case 1:
            stackService.CreateStack();
            break;
        case 2:
            stackService.EditStack();
            break;
        case 3:
            stackService.DeleteStack();
            break;
        case 4:
            cardService.CreateCard();
            break;
        case 5:
            cardService.EditCard();
            break;
        case 6:
            cardService.DeleteCard();
            break;
        case 7:
            sessionsService.StartSession();
            break;
        case 8:
            sessionsService.ShowStudySessionsByMonth();
            break;
        case 9:
            dbManager.DropAndRecreateTables();
            break;
        case 10:
            AnsiConsole.MarkupLine("Goodbye!");
            running = false;
            Environment.Exit(0);
            break;
    }
}

