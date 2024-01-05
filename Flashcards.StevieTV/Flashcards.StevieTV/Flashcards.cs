using System.Configuration;
using Flashcards.StevieTV.Database;

namespace Flashcards.StevieTV;

internal static class Flashcards
{
    private static readonly string DatabaseName = ConfigurationManager.AppSettings.Get("Database");

    private static void Main()
    {
        DatabaseManager.CreateDatabase(DatabaseName);
    }
}