using Microsoft.Extensions.Configuration;
using FlashcardsApp.Services;
using FlashcardsApp.UI.Core;

namespace FlashcardsApp
{
    class Program
    {
        static IConfiguration? config;               // a variable of type IConfiguration
        static DatabaseService? databaseService;
        static void Main(string[] args)
        {
            try
            {
                config = new ConfigurationBuilder()     // Starting with an empty configuration but will return an IConfiguration object
                    .SetBasePath(Directory.GetCurrentDirectory())       // tells the builder where to look for configuration files (appsettings.json)
                    .AddJsonFile("appsettings.json")                    // what file it should be reading
                    .Build();                                           // creates the the IConfiguration object

                string? connectionString = config.GetConnectionString("Default");    //GetConnectionString("Default") specifically looks for "ConnectionString" in appsettings.json
                                                                                     // and return the value associated with "Default"
                if (connectionString == null)
                {
                    throw new Exception("Connection string not found\n");
                    
                }

                databaseService = new(connectionString);
                databaseService.TestConnection();

                Console.WriteLine("Successfully connected to server!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading database: {ex.Message}\n");
                Console.WriteLine("Press Any Key to Exit...");
                Console.ReadKey();
                return;
            }

            MenuHandler menuHandler = new(databaseService);

            menuHandler.MainMenu();
        }
    }
}