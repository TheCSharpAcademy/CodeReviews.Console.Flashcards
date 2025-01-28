using FunRun.Flashcards;
using FunRun.Flashcards.Data;
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

       //services.AddScoped<ISessionCrudService, SessionCrudService>();
       //services.AddScoped<IUserInputService, UserInputService>();
       //services.AddScoped<IDataAccess, DataAccess>();

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