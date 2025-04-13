using Flashcards.Database;
using Flashcards.Model;
namespace Flashcards
{
    internal class Program
    {
        //Server=(localdb)\\Flashcard DB;Database=FlashcardDB;Trusted_Connection=True;
        
        static void Main(string[] args)
        {
            DatabaseManager.InitializeDatabase();

            UserInputManager.Menu();
        }
    }
}
