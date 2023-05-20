namespace FlashcardsLibrary.Display;
internal class InputModel
{
    internal string GetMenuChoice()
    {
        Console.Write("\tSelect an option: ");
        string input = Console.ReadLine()!;
        return input;
    }
}
