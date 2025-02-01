using Flashcards.FunRunRushFlush.App;
using Flashcards.FunRunRushFlush.App.Interfaces;
using Flashcards.FunRunRushFlush.App.Screens;
using Flashcards.FunRunRushFlush.Controller;
using Flashcards.FunRunRushFlush.Controller.Interfaces;
using Flashcards.FunRunRushFlush.Data;
using Flashcards.FunRunRushFlush.Data.Interfaces;
using Flashcards.FunRunRushFlush.Services;
using Flashcards.FunRunRushFlush.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;

var host = Host.CreateDefaultBuilder(args)
   .ConfigureServices((context, services) =>
   {
       var configuration = context.Configuration;
       string connectionString = configuration.GetConnectionString("SQLServerConnection")!;
       services.AddSingleton(provider =>
       {
           var logger = provider.GetRequiredService<ILogger<SqlServerConnectionFactory>>();
           return new SqlServerConnectionFactory(connectionString, logger);
       });
       services.AddScoped<IDbConnection>(provider =>
       {
           var factory = provider.GetRequiredService<SqlServerConnectionFactory>();
           return factory.CreateConnection();
       });
       services.AddSingleton<FlashcardApp>();
       services.AddScoped<IFlashcardScreen, FlashcardScreen>();
       services.AddScoped<IStackScreen, StackScreen>();
       services.AddScoped<IStudySessionScreen, StudySessionScreen>();


       services.AddScoped<IUserInputValidationService, UserInputValidationService>();

       services.AddScoped<ICrudController, CrudController>();
       services.AddScoped<IFlashcardsDataAccess, FlashcardsDataAccess>();
       services.AddScoped<IStackDataAccess, StackDataAccess>();
       services.AddScoped<IStudySessionDataAccess, StudySessionDataAccess>();

   })
    .ConfigureLogging(logger =>
    {
        logger.ClearProviders();
        logger.AddDebug();
        logger.AddConsole();

    }).Build();


var init = host.Services.GetRequiredService<SqlServerConnectionFactory>();
init.InitializeDatabase();

var app = host.Services.GetRequiredService<FlashcardApp>();
await app.RunApp();