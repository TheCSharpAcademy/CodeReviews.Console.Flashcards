namespace Flashcards.CoreyJordan.Display;
internal class InputModel
{
    internal string GetMenuChoice()
    {
        Console.Write("\tSelect an option: ");
        string input = Console.ReadLine()!;
        return input;
    }

    internal string GetString(string v)
    {
        throw new NotImplementedException();
    }
}
