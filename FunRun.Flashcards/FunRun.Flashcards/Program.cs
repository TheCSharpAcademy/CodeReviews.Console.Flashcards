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
       string connectionString = configuration.GetConnectionString("SQLiteConnection")!;
       services.AddSingleton(provider => new SqlServerConnectionFactory(connectionString));
       services.AddScoped<IDbConnection>(provider =>
       {
           var factory = provider.GetRequiredService<SqlServerConnectionFactory>();
           return factory.CreateConnection();
       });

       services.AddSingleton<CodingTrackerApp>();

       services.AddScoped<ISessionCrudService, SessionCrudService>();
       services.AddScoped<IUserInputService, UserInputService>();
       services.AddScoped<IDataAccess, DataAccess>();

   })
    .ConfigureLogging(logger =>
    {
        logger.ClearProviders();
        logger.AddDebug();

    }).Build();


var init = host.Services.GetRequiredService<SqlServerConnectionFactory>();
init.InitializeDatabase();

var app = host.Services.GetRequiredService<CodingTrackerApp>();
await app.RunApp();