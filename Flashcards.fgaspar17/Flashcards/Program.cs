using Flashcards;
using FlashcardsLibrary;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string? setupConnectionString = builder.GetConnectionString("SetupConnection");

GlobalConfig.InitializeSetupConnectionString(setupConnectionString);
string? database = builder.GetValue<string>("DatabaseName");

CancelSetup.CancelString = builder.GetValue<string>("CancelString");

DatabaseConfig databaseConfig = new DatabaseConfig();
databaseConfig.InitializeDatabase(database);

GlobalConfig.InitializeConnectionString(database);

Application.Run();