using Flashcards.DataAccess;

namespace Flashcards;

internal static class Program
{
    private const string _defaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    public static string DateTimeFormat = _defaultDateTimeFormat;

    static void Main()
    {
        PrepareDatabase();
        var connectionString = ReadConfiguration();
        IDataAccess _dataAccess = new SqlDataAccess(connectionString);
        var screen = UI.MainMenu.Get(_dataAccess);
        screen.Show();
        Console.Clear();
    }

    private static void PrepareDatabase()
    {
        // Ugly workaround to get past Codacy which seemingly can't handle SQL Server projects.
        string? connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("PreparationConnectionString");
        if (connectionString == null)
        {
            return;
        }
        _ = bool.TryParse(System.Configuration.ConfigurationManager.AppSettings.Get("AddSampleData"), out bool addSampleData);
        SqlServerPreparation.Prepare(connectionString, addSampleData);
    }

    private static string ReadConfiguration()
    {
        string? connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");
        if (connectionString == null)
        {
            Console.Clear();
            Console.WriteLine("Please add a connection string to the App.config file.");
            Environment.Exit(1);
        }
        if (System.Configuration.ConfigurationManager.AppSettings.Get("DateTimeFormat") is string dateTimeFormat)
        {
            try
            {
                DateTime.Now.ToString(dateTimeFormat);
                DateTimeFormat = dateTimeFormat;
            }
            catch (FormatException ex)
            {
                Console.Clear();
                Console.WriteLine($"Invalid DateTimeFormat: {ex.Message}");
                Console.WriteLine("\nEnter a valid format in the App.config file, or remove it to use the default.");
                Environment.Exit(1);
            }
        }
        return connectionString;
    }
}
