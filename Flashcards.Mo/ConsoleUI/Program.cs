using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Flashcards.Data.Context;
using Flashcards.Data.Repositories;
using Flashcards.Services;
using Flashcards.Domain.Interfaces;


class Program
{
    static void Main(string[] args)
    {
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        
            var serviceProvider = new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString!))  
            .AddScoped<IStackRepository, StackRepository>()
            .AddScoped<IFlashcardRepository, FlashcardRepository>()
            .AddScoped<IStudySessionRepository, StudySessionRepository>()
            .AddScoped<StackService>()
            .AddScoped<FlashcardService>()
            .AddScoped<StudySessionService>()
            .AddScoped(sp => connectionString!)  
            .BuildServiceProvider();
        
        while (true)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var stackService = scope.ServiceProvider.GetService<StackService>();
                var flashcardService = scope.ServiceProvider.GetService<FlashcardService>();
                var studySessionService = scope.ServiceProvider.GetService<StudySessionService>();

                if (stackService == null || flashcardService == null || studySessionService == null)
                {
                    Console.WriteLine("Failed to resolve services. Exiting the application.");
                    return;
                }

                Console.Clear();
                Console.WriteLine("Flashcard Application");
                Console.WriteLine("1. Manage Stacks");
                Console.WriteLine("2. Study");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StackManagementUI.Run(stackService, flashcardService);
                        break;
                    case "2":
                        StudySessionUI.Run(stackService, studySessionService);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}