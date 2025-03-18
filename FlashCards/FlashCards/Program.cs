namespace FlashCards
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserInterface ui = new UserInterface();
            ui.PrintMainMenu();
            Console.ReadLine();
        }
    }
}
