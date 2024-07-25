using Flashcards;
using Flashcards.Menu;
using Flashcards.Repositories;
using Flashcards.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

//Register database context
services.AddDbContext<AppDbContext>();

//Register main menu class
services.AddScoped<MainMenu>();

//Register repositories
services.AddScoped<IFlashcardRepository, FlashcardRepository>();
services.AddScoped<IStackRepository, StackRepository>();

//Register services
services.AddScoped<FlashcardService>();
services.AddScoped<StackService>();

//Register menu managers
services.AddScoped<FlashcardManager>();
services.AddScoped<StackManager>();

var serviceProvider = services.BuildServiceProvider();

var mainMenu = serviceProvider.GetRequiredService<MainMenu>();
await mainMenu.Run();

