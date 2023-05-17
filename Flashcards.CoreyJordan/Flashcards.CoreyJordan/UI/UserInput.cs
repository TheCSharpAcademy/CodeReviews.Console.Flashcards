namespace Flashcards.CoreyJordan.UI;
internal static class UserInput
{
    internal static int GetInt(string prompt)
    {
        int output;

        string input = GetString(prompt);
        while (int.TryParse(input, out output) == false)
        {
            Console.Write("\tInvalid input. Try again: ");
            input = Console.ReadLine()!;
        }
        return output;
    }

    internal static string GetString(string prompt)
    {
        Console.Write(prompt);
        string input = Console.ReadLine()!;

        while (input.Length == 0)
        {
            Console.Write("\tMust not be blank. Try Again: ");
            input = Console.ReadLine()!;
        }
        return input;
    }
}
