namespace Flashcards.CoreyJordan.Display;
internal class InputModel
{
    public ConsoleUI UIConsole { get; set; } = new();

    internal int GetInt(string prompt)
    {
        int output;

        Console.Write(prompt);
        string input = Console.ReadLine()!;

        while (int.TryParse(input, out output) == false)
        {
            UIConsole.PromptAndReset("Must be an integer. Try again.");
            
            UIConsole.WriteCenter(prompt);
            input = Console.ReadLine()!;
        }
        return output;
    }

    internal string GetString(string prompt)
    {
        UIConsole.WriteCenter(prompt);
        return Console.ReadLine()!;
    }
}
