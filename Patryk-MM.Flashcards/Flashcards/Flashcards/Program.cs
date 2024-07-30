using Flashcards;
using Flashcards.Menu;
using Flashcards.Repositories;
using Flashcards.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

var services = new ServiceCollection();

//Register database context
services.AddDbContext<AppDbContext>();

//Register repositories
services.AddScoped<IFlashcardRepository, FlashcardRepository>();
services.AddScoped<IStackRepository, StackRepository>();
services.AddScoped<IStudySessionRepository, StudySessionRepository>();

//Register services
services.AddScoped<FlashcardService>();
services.AddScoped<StackService>();
services.AddScoped<StudySessionService>();

//Register menu managers
services.AddScoped<FlashcardManager>();
services.AddScoped<StackManager>();
services.AddScoped<StudySessionManager>();

//Register main menu class
services.AddScoped<MainMenu>();

var serviceProvider = services.BuildServiceProvider();

// Check for pending migrations
using (var scope = serviceProvider.CreateScope()) {
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var pendingMigrations = dbContext.Database.GetPendingMigrations();


    if (pendingMigrations.Any()) {
        Utilities.ClearConsole();
        if (AnsiConsole.Confirm("There are pending database migrations. Do you want to apply them?")) {
            dbContext.Database.Migrate();
            Console.WriteLine("Database migrations applied successfully.");
        } else {
            Console.WriteLine("Closing the app...");
            return;
        }
        await Task.Delay(2000);
    }
}

AnsiConsole.Clear();

var mainMenu = serviceProvider.GetRequiredService<MainMenu>();
await mainMenu.Run();

//TODO: testing and code refactoring
