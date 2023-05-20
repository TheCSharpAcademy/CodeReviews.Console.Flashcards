using System.Configuration;

namespace FlashcardsLibrary.FlashCard.Data;
internal class ConnManager
{
    internal static string FlashCardDb { get; } = "FlashCardDb";

    internal static string GetConnectionString(string dbName) 
    { 
        return ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
    }
}
