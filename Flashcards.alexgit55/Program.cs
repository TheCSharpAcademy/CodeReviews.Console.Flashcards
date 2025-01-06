namespace Flashcards.alexgit55
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var DataAccess = new DataAccess();
            DataAccess.CreateTables();

            Console.ReadKey();
        }
    }
}
