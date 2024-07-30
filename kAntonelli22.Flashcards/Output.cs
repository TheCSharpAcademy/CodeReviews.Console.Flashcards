using Spectre.Console;
using System.Linq;

namespace Flashcards;

internal class Output
{
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
        ViewStacks(true);
    } // end of CreateStack Method

    public static void EditStack()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("Which Stack do you want to edit?");
        CardStack? stack = CardStack.Stacks.FirstOrDefault(stack => stack.name == input);
        if (stack == null)
            return;
            
        string input = OutputUtilities.DisplayList(CardStack.Stacks);
        string name = AnsiConsole.Ask<string>($"What is the Stacks name? (Current name: {stack.name})");
        int size = AnsiConsole.Ask<int>($"What is the Stacks size? (Current name: {stack.size})");

        stack.name = name;
        stack.size = size;
        ViewStacks(true);
    } // end of EditStack Method

    public static void RemoveStack()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("Which Stack do you want to remove?");
        string input = OutputUtilities.DisplayList(CardStack.Stacks);

        CardStack? stack = CardStack.Stacks.FirstOrDefault(stack => stack.name == input);
        if (stack == null)
            return;

        CardStack.Stacks.Remove(stack);
        ViewStacks(true);
    } // end of EditStack Method

    public static void EditCards()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]EditCards is incomplete[/]");
        ViewStacks(true);
    } // end of EditStack Method

    public static void ViewStacks(bool displayMenu)
    {
        Console.Clear();

        List<Panel> panels = [];

        for (int i = 0; i < CardStack.Stacks.Count(); i++)
        {
            string cards = "";
            CardStack stack = CardStack.Stacks[i];
            for (int j = 0; j < stack.Cards.Count(); j++)
            {
                cards += $"\n{stack.Cards[j].question} = {stack.Cards[j].answer}";
            }
            var panel = new Panel(cards);
            panel.Header = new PanelHeader(stack.name);
            panels.Add(panel);
        }

        AnsiConsole.Write(new Columns(panels));
        if (displayMenu)
            StackOptions();
    } // end of ViewStacks Method

    public static void StackOptions()
    {
        AnsiConsole.WriteLine("Stack Options\n------------------------");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices([
                "Exit Flashcard", "Create Stack", "Edit Stack",
                "Remove Stack", "Edit Cards", "<-- Back"
                ]));

        switch (menu)
        {
            case "Exit Flashcard":
                Environment.Exit(0);
                break;
            case "Create Stack":
                CreateStack();
                break;
            case "Edit Stack":
                EditStack();
                break;
            case "Remove Stack":
                RemoveStack();
                break;
            case "Edit Cards":
                EditCards();
                break;
            case "<-- Back":
                return;
        }
    }
}
