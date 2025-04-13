using Flashcards.Database;
using Flashcards.Model;
namespace Flashcards
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            DatabaseManager.InitializeDatabase();

            UserInputManager.Menu();
        }
    }
}
