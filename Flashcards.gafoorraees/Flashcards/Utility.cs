namespace Flashcards;

internal class Utility
{
    public static string ValidString(string input)
    {
        while (string.IsNullOrWhiteSpace(input) || input.All(char.IsDigit))
        {
            Console.WriteLine("Input cannot be empty or contain all numbers. Please enter a valid stack name.");
            input = Console.ReadLine();
        }

        return input;
    }

    public static void ReturnToMenu()
    {
        var menu = new SelectionMenu();

        menu.Menu();
    }
}
