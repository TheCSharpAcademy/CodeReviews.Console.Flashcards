using Flashcards;
using FlashcardsLibrary;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string? connectionString = builder.GetConnectionString("DefaultConnection");
string? setupConnectionString = builder.GetConnectionString("SetupConnection");

GlobalConfig.InitializeConnectionString(connectionString);
GlobalConfig.InitializeSetupConnectionString(setupConnectionString);

CancelSetup.CancelString = builder.GetValue<string>("CancelString");

Application.Run();