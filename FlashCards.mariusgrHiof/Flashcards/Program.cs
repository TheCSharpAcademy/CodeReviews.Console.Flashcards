using Flashcards.Controllers;
using Flashcards.Data;
using Flashcards.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

// Retrieve the connection string
string connectionString = configuration.GetConnectionString("DefaultConnection");

// Setup dependency injection
var serviceProvider = new ServiceCollection()
    .AddDbContext<FlashcardsDbContext>(options =>
        options.UseSqlServer(connectionString))
    .BuildServiceProvider();

// Resolve the DbContext from the service provider
var dbContext = serviceProvider.GetRequiredService<FlashcardsDbContext>();

DataAccess dataAccess = new DataAccess(dbContext);
dataAccess.EnsureDbExists();

StacksController stacksController = new StacksController(dataAccess);
FlashcardsController flashcardsController = new FlashcardsController(dataAccess);
StudySessionsController studySessionsController = new StudySessionsController(dataAccess);

UserInterface userInterface = new UserInterface(stacksController, flashcardsController, studySessionsController);

await userInterface.Run();