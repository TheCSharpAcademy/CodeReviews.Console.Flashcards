using System.Diagnostics;
using Spectre.Console;

namespace Flashcards;

internal class Practice
{
    public static void PracticeMenu()
    {
        Console.Clear();
        Output.ViewStacks(false);
        AnsiConsole.WriteLine("Which Stack?");
        string input = OutputUtilities.DisplayStack(CardStack.Stacks);

        if (CardStack.Stacks.Any(stack => stack.StackName == input))
            PracticeStack(input);
        else if (input == "<-- Back To Menu")
            Program.MainMenu();
    } // end of PracticeMenu Method

    public static void PracticeStack(string stackName)
    {
        Console.Clear();
        for (int i = 3; i > 0; i--)
        {
            Console.Write(i);
            Thread.Sleep(250);
            for (int j = 0; j < 3; j++)
            {
                Console.Write(".");
                Thread.Sleep(250);
            }
            Console.Write("\n");
        }
        Console.Clear();

        Stopwatch stopwatch = new();
        List<TimeSpan> times = [];
        int numCorrect = 0;
        int numComplete = 0;
        CardStack? stack = CardStack.Stacks.FirstOrDefault(stack => stack.StackName == stackName);
        if (stack == null)
            return;

        for(int i = 0; i < stack.Cards.Count; i++)
        {
            Card card = stack.Cards[i];
            string answer = AnsiConsole.Ask<string>($"{card.Front}");
            stopwatch.Start();
            if (answer == card.Back)
            {
                numCorrect++;
                AnsiConsole.MarkupLine("[green]Correct![/]");
            }
            else
            {
                AnsiConsole.MarkupLineInterpolated($"[red]Wrong![/] [blue]Correct Answer: {card.Back}[/]");
            }
            numComplete++;
            times.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            Thread.Sleep(500);
            
        }
        double avgTime = 0;
        for (int i = 0; i < times.Count; i++)
            avgTime += times[i].TotalSeconds;

        avgTime /= times.Count;
        StudySession session = new(DateTime.Now, numComplete, numCorrect, stackName, avgTime);
        DatabaseUtilities.DatabaseHelper.InsertSession(session);
        PracticeOver(stackName);
    } // end of PracticeStack Method

    public static void PracticeOver(string stackName)
    {
        AnsiConsole.WriteLine("\n------------------------");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices(["Exit Program", "Redo Stack", "Try Another", "<-- Back"]));

        switch (menu)
        {
            case "Exit Program":
                Environment.Exit(0);
                break;
            case "Redo Stack":
                PracticeStack(stackName);
                break;
            case "Try Another":
                PracticeMenu();
                break;
            case "<-- Back":
                Program.MainMenu();
                break;
        }
    }
}