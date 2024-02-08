using System.Configuration;

namespace Flashcards.jkjones98;

class Program
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    static void Main(string[] args)
    {
        StackCreator stackCreator = new();
        MainMenu mainMenu = new();
        
        stackCreator.CreateStackTable(connectionString);
        stackCreator.CreateFlashcardTable(connectionString);
        stackCreator.CreateStudySessionTable(connectionString);
        mainMenu.DisplayMenu();
    }
}
