using static Flashcards.Helpers;

namespace Flashcards;

class Program
{
    internal static List<StackShowDTO> stackNames = new List<StackShowDTO>();
    internal static List<Session> sessions = new List<Session>();
    
    static void Main(string[] args)
    {
        //Create 'stacks', 'flashcards' and 'sessions' tables if they don't exist
        CreateTablesIfNotExists();
        
        UserInterface.ShowMenu();
    }
}