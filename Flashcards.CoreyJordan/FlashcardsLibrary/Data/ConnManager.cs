namespace FlashcardsLibrary.Data;
public abstract class ConnManager
{
    public static string FlashCardDb { get; } = GetConnectionString("FlashCardsDB");

    public static string GetConnectionString(string dbName)
    {
        return System.Configuration.ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
    }
}
