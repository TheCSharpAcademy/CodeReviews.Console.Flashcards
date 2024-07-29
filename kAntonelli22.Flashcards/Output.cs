using Spectre.Console;
using System.Linq;

namespace Flashcards;

internal class Output
{
    public static void PracticeStack()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]PracticeStack is incomplete[/]");
        ReturnToMenu("");
    } // end of PracticeStack Method

    public static void CreateStack()
    {
        Console.Clear();
        string name = AnsiConsole.Ask<string>("What is the Stacks name?");
        int size = AnsiConsole.Ask<int>("What is the Stacks size?");
        CardStack stack = new(name, size);

        bool empty = AnsiConsole.Confirm("Do you want to leave the Stack empty?");
        if (empty)
        {
            for (int i = 0; i < size; i++)
                new Card($"Card {i}", $"Card {i}", stack);
        }
        else if (!empty)
        {
            for (int i = 0; i < size; i++)
            {
                string question = AnsiConsole.Ask<string>("What is the cards question?");
                string answer = AnsiConsole.Ask<string>("What is the answer?");
                new Card(question, answer, stack);
            }
        }
        // insert into database
        ReturnToMenu("");
    } // end of CreateStack Method

    public static void EditStack()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]EditStack is incomplete[/]");
        ReturnToMenu("");
    } // end of EditStack Method

    public static void ViewStacks()
    {
        Console.Clear();
        var options = new SelectionPrompt<string>();
        for (int i = 0; i < CardStack.Stacks.Count; i++)
        {
            options.AddChoice($"{CardStack.Stacks[i].name}");
        }
        options.AddChoice("<-- Back");
        var menu = AnsiConsole.Prompt(options);

        if (CardStack.Stacks.Any(stack => stack.name == menu))
            ViewCards(menu);
        else if (menu == "<-- Back")
            return;

    } // end of ViewStacks Method

    public static void ViewCards(string stackName)
    {
        Console.Clear();
        CardStack? stack = CardStack.Stacks.FirstOrDefault(stack => stack.name == stackName);
        if (stack == null)
            return;

        string cards = "";
        for (int i = 0; i < stack.Cards.Count(); i++)
        {
            cards += $"{stack.Cards[i].question.PadRight(10)} ......... {stack.Cards[i].answer.PadLeft(10)}\n";
        }
        var panel = new Panel(cards);
        panel.Header = new PanelHeader(stackName);

        ReturnToMenu("");
    } // end of ViewCards Method

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
