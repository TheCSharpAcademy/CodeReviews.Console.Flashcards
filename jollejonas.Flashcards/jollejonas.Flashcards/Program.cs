using jollejonas.Flashcards.Models;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using jollejonas.Flashcards.Data;
using jollejonas.Flashcards.Services;

string currentDirectory = Directory.GetCurrentDirectory();

string projectDirectory = Path.Combine(currentDirectory, @"..\..\..");
string appSettingsPath = Path.Combine(projectDirectory, "Properties");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"{appSettingsPath}\\appsettings.json", optional: true, reloadOnChange: true)
    .Build();

string? connectionString = configuration.GetConnectionString("DefaultConnection");

var dbManager = new DatabaseManager(connectionString);
var cardStackService = new CardStackService(dbManager);
var cardService = new CardService(dbManager);
var studySessionService = new StudySessionService(dbManager);

dbManager.EnsureDatabaseExists();

var mainMenuOption = new List<MainMenuOption>
    {
        new () { Id = 1, Description = "Create a new stack" },
        new () { Id = 2, Description = "Edit a stack" },
        new () { Id = 3, Description = "Delete a stack" },
        new () { Id = 4, Description = "Create a new card" },
        new () { Id = 5, Description = "Edit a card" },
        new () { Id = 6, Description = "Delete a card" },
        new () { Id = 7, Description = "Study a stack" },
        new () { Id = 8, Description = "Show study sessions per month" },
        new () { Id = 0, Description = "Exit" }
    };
while (true)
{
    Console.Clear();

    var menuSelection = AnsiConsole.Prompt(
        new SelectionPrompt<MainMenuOption>()
        .Title("Select an option")
        .PageSize(10)
        .AddChoices(mainMenuOption)
        .UseConverter(option => option.Description));

    if (menuSelection.Id == 0)
    {
        break;
    }

    switch (menuSelection.Id)
    {
        case 1:
            cardStackService.CreateStack();
            break;
        case 2:
            cardStackService.EditStack();
            break;
        case 3:
            cardStackService.DeleteStack();
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
            studySessionService.StartStudySession();
            break;
        case 8:
            studySessionService.DisplaySessionsPerMonth();
            break;
        case 10:
            dbManager.DropAndRecreateTables();
            break;
    }
}