using Spectre.Console;

namespace Flashcards;

internal class EditStack
{
    // public static void EditStack()
    // {
    //     Console.Clear();
    //     AnsiConsole.MarkupLine("Which Stack do you want to edit?");
    //     string input = OutputUtilities.DisplayList(CardStack.Stacks);
    //     CardStack? stack = CardStack.Stacks.FirstOrDefault(stack => stack.name == input);
    //     if (stack == null)
    //         return;
            
    //     EditMenu();
    //     string name = AnsiConsole.Ask<string>($"What is the Stacks name? (Current name: {stack.name})");
    //     int size = AnsiConsole.Ask<int>($"What is the Stacks size? (Current name: {stack.size})");

    //     stack.name = name;
    //     stack.size = size;
    // } // end of EditStack Method

    public static void RenameStack()
    {
        if (Output.CurrentStack != null)
        {
            string newName = AnsiConsole.Ask<string>($"What is the Stacks name? (Current name: [blue]{Output.CurrentStack.StackName}[/])");
            Output.CurrentStack.StackName = newName;
        }
        else
            Environment.Exit(0);
    } // end of RenameStack Method

    public static void ResizeStack()
    {
        if (Output.CurrentStack != null)
        {
            int newSize = AnsiConsole.Ask<int>($"What is the Stacks size? (Current name: [blue]{Output.CurrentStack.StackSize}[/])");
            if (newSize > Output.CurrentStack.StackSize)
            {
                for (int i = Output.CurrentStack.StackSize; i < newSize; i++)
                    new Card($"Question {i}", $"Answer {i}", Output.CurrentStack);
            }
            else if (newSize < Output.CurrentStack.StackSize)
                Output.CurrentStack.Cards.RemoveRange(newSize, Output.CurrentStack.Cards.Count - newSize);
            
            Output.CurrentStack.StackSize = newSize;
        }
        else
            Environment.Exit(0);
    } // end of ResizeStack Method

    public static void EditCards()
    {
        Console.Clear();
        if (Output.CurrentStack != null)
        {
            OutputUtilities.DisplayCards(Output.CurrentStack.Cards);
            AnsiConsole.MarkupLine("[yellow]EditCards is incomplete[/]");
        }
    } // end of EditCards Method

    public static void EditMenu()
    {
        Console.Clear();
        Output.ViewStacks(false);
        AnsiConsole.MarkupLine("Which Stack do you want to edit?");
        string input = OutputUtilities.DisplayStack(CardStack.Stacks);
        Output.CurrentStack = CardStack.Stacks.FirstOrDefault(stack => stack.StackName == input);
        if (Output.CurrentStack == null)
            return;
        
        Console.Clear();
        Output.ViewStacks(false);
        AnsiConsole.WriteLine("Edit Options\n------------------------");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices([
                "Exit Flashcard", "Rename Stack", "Resize Stack",
                "Edit Cards", "<-- Back"
                ]));

        switch (menu)
        {
            case "Exit Flashcard":
                Environment.Exit(0);
                break;
            case "Rename Stack":
                RenameStack();
                break;
            case "Resize Stack":
                ResizeStack();
                break;
            case "Edit Cards":
                EditCards();
                break;
            case "<-- Back":
                return;
        }
    }
}
