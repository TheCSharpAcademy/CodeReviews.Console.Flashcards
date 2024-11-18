using FlashcardGame.Helpers;
using FlashcardGame.Views;

namespace FlashcardGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
           
                DatabaseHelpers.InitializeDatabase();
                DatabaseHelpers.InitializeTables();
                MainMenu.Get1UserInput();
          
            
          
        }
    }
}
