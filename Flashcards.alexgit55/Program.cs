namespace Flashcards.alexgit55
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var DataAccess = new DataAccess();

            SeedData.SeedRecords();

            UserInterface.DisplayMainMenu();
        }
    }
}
