using Flashcards.Tables;

namespace Flashcards;

class Program
{
    static void Main(string[] args)
    {
        DatabaseSetup.EnsureDatabaseSetup();

        var menu = new SelectionMenu();
      
        menu.Menu();
    }
}