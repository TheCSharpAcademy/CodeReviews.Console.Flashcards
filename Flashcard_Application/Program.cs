using Flashcard_Application.DataServices;
using Flashcards.UI;

namespace Flashcard_Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateDatabaseAndTable.DbAndTableCreation();
            MainMenu.MainMenuPrompt();
        }
    }
}
