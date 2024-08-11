using Spectre.Console;

namespace Flashcards;

internal class EditStack
{
    public static void RenameStack()
    {
        if (Output.CurrentStack != null)
        {
            string newName;
            while (true)
            {
                newName = AnsiConsole.Ask<string>($"What is the Stacks name? (Current name: [blue]{Output.CurrentStack.StackName}[/])");
                if (OutputUtilities.NameUnique(newName, CardStack.Stacks))
                    break;
                AnsiConsole.MarkupLine("[red]Stack names must be unique[/]");
            }
            string query = $"UPDATE dbo.Stacks SET StackName = '{newName}' WHERE StackName = '{Output.CurrentStack.StackName}'";
            DatabaseUtilities.DatabaseHelper.RunQuery(query);
            Output.CurrentStack.StackName = newName;
        }
        else
            AnsiConsole.WriteLine("[red]Error: no stack selected.[/]");
    } // end of RenameStack Method

    public static void ResizeStack()
    {
        if (Output.CurrentStack != null)
        {
            int newSize = AnsiConsole.Ask<int>($"What is the Stacks size? (Current size: [blue]{Output.CurrentStack.StackSize}[/])");
            if (newSize > Output.CurrentStack.StackSize)
            {
                for (int i = Output.CurrentStack.StackSize; i < newSize; i++)
                    new Card($"Question {i}", $"Answer {i}", Output.CurrentStack);
            }
            else if (newSize < Output.CurrentStack.StackSize)
                Output.CurrentStack.Cards.RemoveRange(newSize, Output.CurrentStack.Cards.Count - newSize);
            
            Output.CurrentStack.StackSize = newSize;
            string query = $"UPDATE dbo.Stacks SET StackSize = '{newSize}' WHERE StackName = '{Output.CurrentStack.StackName}'";
            DatabaseUtilities.DatabaseHelper.RunQuery(query);
            // insert new cards or remove old cards
        }
        else
            AnsiConsole.WriteLine("[red]Error: no stack selected.[/]");
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
