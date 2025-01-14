using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Spectre.Console;

internal class Config
{
    public static string ConnectionString { get; set; }
    private const string AppSettingsFile = "appsettings.json";

    static Config()
    {
        CheckConfigFile();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(AppSettingsFile)
            .Build();

        ConnectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
    }

    private static void CheckConfigFile()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), AppSettingsFile);

        if (!File.Exists(filePath))
        {
            var config = new
            {
                ConnectionStrings = new
                {
                    DefaultConnection = @"Server=(localdb)\\Flashcards;Database=FlashcardDB;Integrated Security=True;TrustServerCertificate=True;"
                }
            };

            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            try
            {
                File.WriteAllText(filePath, json);
            }
            catch (UnauthorizedAccessException ex)
            {
                AnsiConsole.MarkupLine($"[red]Failed to write to \"{AppSettingsFile}\" file.[/]");
                AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
                DisplayInfoHelpers.PressAnyKeyToContinue();
            }
        }
    }
}
