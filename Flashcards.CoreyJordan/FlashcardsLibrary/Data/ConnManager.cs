using System.Configuration;

namespace FlashcardsLibrary.Data;
internal class ConnManager
{
    internal static string FlashCardDb { get; } = "FlashCardsDB";

    internal static string GetConnectionString(string dbName)
    {
        return ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
    }
}
