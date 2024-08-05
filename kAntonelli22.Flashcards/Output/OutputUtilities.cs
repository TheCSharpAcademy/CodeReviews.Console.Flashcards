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
            options.AddChoice($"{list[i].front}?    {list[i].back}");
        }
        options.AddChoice("<-- Back To Menu");
        var menu = AnsiConsole.Prompt(options);
        return menu;
    }

    public static void ViewCards(string stackName)
    {
        Console.Clear();
        // CardStack? stack = CardStack.Stacks.FirstOrDefault(stack => stack.name == stackName);
        // if (stack == null)
        //     return;

        // string cards = "";
        // for (int i = 0; i < stack.Cards.Count(); i++)
        // {
        //     cards += $"{stack.Cards[i].question.PadRight(10)} . . . . . . {stack.Cards[i].answer.PadLeft(10)}\n";
        // }
        // var panel = new Panel(cards);
        // panel.Header = new PanelHeader(stackName);
        // AnsiConsole.Write(panel)
    } // end of ViewCards Method

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