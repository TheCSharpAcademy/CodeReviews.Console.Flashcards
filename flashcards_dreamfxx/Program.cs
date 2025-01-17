using Flashcards.DreamFXX.Data;
using Flashcards.DreamFXX.Models;
using Flashcards.DreamFXX.Services;
using Microsoft.Extensions.Configuration;
using Spectre.Console;


string dir = Directory.GetCurrentDirectory();
string rootDir = Path.Combine(dir, @"..\..\..\");

var cnnConfig = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"{rootDir}//appsettings.json", optional: true, reloadOnChange: true)
    .Build();

string? connectionString = cnnConfig.GetConnectionString("DefaultConnection");

var dbManager = new DbManager(connectionString);
var cardStackService = new StackofCards();
var cardService = new CardService();
//var studySessionService = new StudySessionService();

dbManager.CheckDatabaseExists();

var mainMenuRoute = new List<MainMenuRoute>
{
        new () { Id = 1, Description = "Create new stack" },
        new () { Id = 2, Description = "Edit existing stack" },
        new () { Id = 3, Description = "Delete existing stack" },
        new () { Id = 4, Description = "Create new card" },
        new () { Id = 5, Description = "Edit existing card" },
        new () { Id = 6, Description = "Delete existing card" },
        new () { Id = 7, Description = "Study a stack" },
        new () { Id = 8, Description = "Show list of study sessions per month" },
        new () { Id = 0, Description = "Exit" }
};

while (true)
{
    Console.Clear();

    MainMenuRoute menuSelection = AnsiConsole.Prompt(
       new SelectionPrompt<MainMenuRoute>()
       .Title("[yellow]Write, edit, organize and most importantly...[/]\n\t[yellow][underline]- EDUCATE! -[/]")
       .PageSize(10)
       .AddChoices(mainMenuRoute)
       .UseConverter(route => route.Description));

    if (menuSelection.Id == 0)
    {
        AnsiConsole.Markup("[red]Goodbye![/]\n");
        break;
    }

    switch (menuSelection.Id)
    {
        case 1:
            Console.Clear();
            CreateCardStacks();
            break;
        case 2:
            Console.Clear();
            //cardStackService.EditCardStack();
            break;
        case 3:
            Console.Clear();
            //cardStackService.DeleteCardStack();
            break;
        case 4:
            Console.Clear();
            dbManager.CheckDatabaseExists();
            break;
    }

    Console.ReadKey();
}
    }
}