using FlashCards.Database;
using FlashCards.UI;

namespace Flashcards;

class Program
{
    public static void Main(string[] args)
    {
        DatabaseManager.Start();
        Menu.ShowMainMenu();
    }
}