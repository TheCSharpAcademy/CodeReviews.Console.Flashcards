namespace Flashcards;

class UserInput
{
    public static void InsertNewStack()
    {
        Helpers.ClearConsole();
        Console.WriteLine("Please write the name of the stack. Valid characters are a to z, A to Z and 0 to 9."+
        "The maximum length of the name is 50 characters.");
    }
}