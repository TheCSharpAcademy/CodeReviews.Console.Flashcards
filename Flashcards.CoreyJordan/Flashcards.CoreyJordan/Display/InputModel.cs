namespace Flashcards.CoreyJordan.Display;
internal class InputModel
{
    public ConsoleUI UIConsole { get; set; } = new();

    internal bool Confirm(string prompt)
    {
        bool confirmed = false;
        if (GetString($"{prompt}(Y/n): ").ToUpper() == "Y")
        {
            confirmed = true;
        }
        return confirmed;
    }

    internal DateTime GetDate(string prompt)
    {
        throw new NotImplementedException();
    }

    internal int GetInt(string prompt)
    {
        int output;

        UIConsole.WriteCenter(prompt);
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
