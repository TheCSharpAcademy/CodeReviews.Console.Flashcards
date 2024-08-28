namespace Flashcards;

class Program
{
    static void Main(string[] args)
    {
        DataAccess.CreateDatabase();
        DataAccess.CreateTables();
        
        Controller.Run();
    }
}