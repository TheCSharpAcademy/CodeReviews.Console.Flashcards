using System.Configuration;

namespace Flashcards.glaxxie.Utilities;

internal sealed class AppConfiguration
{
    internal string ConnectionString { get; }
    internal string CardsTable { get; }
    internal string StacksTable { get; }
    internal string SessionsTable { get; }

    private AppConfiguration() 
    {
        ConnectionString = ConfigurationManager.ConnectionStrings["flashcards"]!.ConnectionString;
        CardsTable = ConfigurationManager.AppSettings["Cards"]!;
        StacksTable = ConfigurationManager.AppSettings["Stacks"]!;
        SessionsTable = ConfigurationManager.AppSettings["Sessions"]!;
    }

    private static AppConfiguration? _instance;
    internal static AppConfiguration Instance
    {
        get
        {
            _instance ??= new AppConfiguration();
            return _instance;
        }
    }
}