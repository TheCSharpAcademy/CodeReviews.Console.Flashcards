using FlashCards.Controllers;
using FlashCards.Views.Menus;

namespace FlashCards
{
    internal class Program
    {
        static void Main()
        {
            DbInitializer initializer = new();
            MainMenu main = new();
            main.Show();
        }
    }
}
