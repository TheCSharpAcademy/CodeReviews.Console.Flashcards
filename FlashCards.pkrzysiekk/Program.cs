using FlashCards.Controllers;
using FlashCards.Views.Menus;

namespace FlashCards
{
    internal class Program
    {
        private static void Main()
        {
            DbInitializer initializer = new();
            MainMenu main = new();
            main.Show();
        }
    }
}