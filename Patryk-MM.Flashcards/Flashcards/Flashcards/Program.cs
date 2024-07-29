using Flashcards;
using Flashcards.Menu;
using Flashcards.Repositories;
using Flashcards.Services;
using Microsoft.Extensions.DependencyInjection;

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

var mainMenu = serviceProvider.GetRequiredService<MainMenu>();
await mainMenu.Run();

