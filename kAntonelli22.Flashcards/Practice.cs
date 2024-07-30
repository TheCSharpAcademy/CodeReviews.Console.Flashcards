using Spectre.Console;

namespace Flashcards;

internal class Practice
{
    public static void PracticeStack()
    {
        Console.Clear();
        Output.ViewStacks(false);
        string input = OutputUtilities.DisplayList(CardStack.Stacks);

        if (CardStack.Stacks.Any(stack => stack.name == input))
            StartPractice(input);
        else if (input == "<-- Back")
            Program.MainMenu();

        PracticeMenu();
    } // end of PracticeStack Method

    public static void PracticeMenu()
    {
        AnsiConsole.MarkupLine("[yellow]PracticeMenu is incomplete[/]");
        OutputUtilities.ReturnToMenu("");
    } // end of PracticeMenu Method

    public static void StartPractice(string stackName)
    {
        AnsiConsole.MarkupLine("[yellow]StartPractice is incomplete[/]");
    } // end of StartPractice Method
}