using System.Configuration;

namespace FlashcardsLibrary.Data;
public abstract class ConnManager
{
    public static string FlashCardDb { get; } = GetConnectionString("FlashCardsDB");

    public static string GetConnectionString(string dbName)
    {
        return ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
    }
}
