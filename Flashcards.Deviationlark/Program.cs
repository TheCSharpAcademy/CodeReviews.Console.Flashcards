namespace Flashcards
{
    class Program
    {
        public static void Main(string[] args)
        {
            Database database = new();
            database.CreateTable();

            GetUserInput getUserInput = new();
            getUserInput.MainMenu();
        }
        
    }
    

}
