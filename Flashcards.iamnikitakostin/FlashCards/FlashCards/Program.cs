using FlashCards.Controllers;
using FlashCards.Data;
using FlashCards.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace FlashCards;

internal class Program : ConsoleController
{
    static void Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

        string? connectionString = config["ConnectionStrings:DefaultConnection"];
        if (connectionString == null)
        {
            ErrorMessage("Dear user, please ensure that you have your connection string set up in the appsettings.json.");
            return;
        }

        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer(connectionString);

        using var context = new DataContext(optionsBuilder.Options);
        context.Database.Migrate();

        var stackService = new Services.StackService(context);
        var flashcardService = new Services.FlashcardService(context);
        var studySessionService = new Services.StudySessionService(context, flashcardService, stackService);
        var userInterface = new UserInterface(stackService, flashcardService, studySessionService);

        userInterface.MainMenu();
    }
}