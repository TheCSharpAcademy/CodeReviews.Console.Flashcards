using Flashcards.Controllers;

namespace Flashcards;

public class Program
{
    public static IConfigurationRoot? Configuration;

    public static void Main()
    {
        ConfigureApplication();
        InitializeDatabase();
        StartApp();
    }

    private static void StartApp()
    {
        var mainController = new MainController();
        mainController.ShowMainMenu();
    }

    private static void ConfigureApplication()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddUserSecrets<Program>(true);

        Configuration = builder.Build();
    }

    private static void InitializeDatabase()
    {
        try
        {
            var db = new FlaschardDatabase();
            db.CreateDb();
            db.CreateTables();
            db.SeedInitialData();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Environment.Exit(-1);
        }
    }
}