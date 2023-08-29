namespace FlashCards.Ramseis
{
    internal class History
    {
        public static void Menu()
        {
            ConsoleTable.PrintTable(IO.SqlGetHistory());
            Console.Write("\nPress any key to return to main menu...");
            Console.ReadKey();
        }
    }
}
