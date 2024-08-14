using Spectre.Console;

namespace Flashcards;

internal class OutputUtilities
{
    public static string DisplayStack(List<CardStack> list)
    {
        var options = new SelectionPrompt<string>();
        for (int i = 0; i < list.Count; i++)
        {
            options.AddChoice($"{list[i].StackName}");
        }
        options.AddChoice("<-- Back To Menu");
        var menu = AnsiConsole.Prompt(options);
        return menu;
    }

    public static string DisplayCards(List<Card> list)
    {
        var options = new SelectionPrompt<string>();
        for (int i = 0; i < list.Count; i++)
        {
            options.AddChoice($"{i + 1}. {list[i].Front,-20}{list[i].Back}");
        }
        options.AddChoice("<-- Back To Menu");
        var menu = AnsiConsole.Prompt(options);
        return menu;
    }

    public static bool NameUnique(string name, List<CardStack> list)
    {
        foreach(CardStack stack in list)
        {
            if (stack.StackName.Equals(name))
                return false;
        }
        return true;
    } // End of NameUnique Method

    public static void ReturnToMenu(string message)
    {
        if (message == "")
            Console.WriteLine($"Press enter to return to menu.");
        else
            Console.WriteLine($"{message}. Press enter to return to menu.");
        // InputValidator.CleanString();
        Console.ReadLine();
    } // end of ReturnToMenu Method
}