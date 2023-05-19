namespace FlashcardsLibrary.Display;
internal class InputModel
{
    internal string GetStringInput(string prompt)
    {
        Console.Write(prompt);
        string input = Console.ReadLine()!;
        return input;
    }
}
