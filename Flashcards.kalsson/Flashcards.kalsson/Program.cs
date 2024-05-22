using Flashcards.kalsson;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    // Set the base path to the current directory
    .SetBasePath(Directory.GetCurrentDirectory())
    // Add a JSON configuration file named "appsettings.json". It's not optional, and reload if the file changes
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Build the configuration
var configuration = builder.Build();

// Create a new database config object with the built configuration
var dbConfig = new DatabaseConfig(configuration);