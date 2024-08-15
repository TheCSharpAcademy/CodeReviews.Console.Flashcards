using Spectre.Console;
using Flashcards.DatabaseUtilities;

namespace Flashcards;

internal class EditStack
{
    public static void RenameStack()
    {
        if (Output.CurrentStack == null)
            return;
        
        string newName;
        while (true)
        {
            newName = AnsiConsole.Ask<string>($"What is the Stacks name? (Current name: [blue]{Output.CurrentStack.StackName}[/])");
            if (OutputUtilities.NameUnique(newName, CardStack.Stacks))
                break;
            AnsiConsole.MarkupLine("[red]Stack names must be unique[/]");
        }
        string query = $"UPDATE dbo.Stacks SET StackName = '{newName}' WHERE StackName = '{Output.CurrentStack.StackName}'";
        DatabaseHelper.RunQuery(query);
        Output.CurrentStack.StackName = newName;
    } // end of RenameStack Method

    public static void AddCards()
    {
        if (Output.CurrentStack == null)
            return;

        int size = AnsiConsole.Ask<int>("How many cards do you want to add?");
        for (int i = 0; i < size; i++)
        {
            string question = AnsiConsole.Ask<string>("What is the cards question?");
            string answer = AnsiConsole.Ask<string>("What is the answer?");
            Card card = new(question, answer, Output.CurrentStack);
            DatabaseHelper.InsertCard(card, Output.CurrentStack.Id);
        }
    } // end of AddCards Method

    public static void RemoveCard()
    {
        Console.Clear();
        if (Output.CurrentStack == null)
            return;

        AnsiConsole.MarkupLine("Which Card do you want to remove?");
        string input = OutputUtilities.DisplayCards(Output.CurrentStack.Cards);
        Card card = Output.CurrentStack.Cards[int.Parse(input[..1]) - 1];

        Output.CurrentStack.Cards.Remove(card);
        string query = $"DELETE FROM dbo.Cards WHERE Front = '{card.Front}'";
        DatabaseHelper.RunQuery(query);
    } // end of RemoveCard Method

    public static void EditCards()
    {
        Console.Clear();
        if (Output.CurrentStack == null)
            return;

        string input = OutputUtilities.DisplayCards(Output.CurrentStack.Cards);
        Card card = Output.CurrentStack.Cards[int.Parse(input[..1]) - 1];
        string newFront;
        string newBack;

        while (true)
        {
            bool nameGood = true;
            newFront = AnsiConsole.Ask<string>($"What is the cards question? (Current question: [blue]{card.Front}[/])");
            foreach(Card currentCard in Output.CurrentStack.Cards)
            {
                if (currentCard.Front.Equals(newFront) && !currentCard.Front.Equals(card.Front))
                {
                    AnsiConsole.MarkupLine("[red]The cards question must be unique[/]");
                    nameGood = false;
                    break;
                }
            }
            if (nameGood)
            {
                newBack = AnsiConsole.Ask<string>($"What is the answer? (Current answer: [blue]{card.Back}[/])");
                string query = $"UPDATE dbo.Cards SET Front = '{newFront}', Back = '{newBack}' WHERE Front = '{card.Front}'";
                DatabaseHelper.RunQuery(query);
                card.Front = newFront;
                card.Back = newBack;
                return;
            }
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
                "Exit Flashcard", "Rename Stack", "Add Cards",
                "Remove Card", "Edit Cards", "<-- Back"
                ]));

        switch (menu)
        {
            case "Exit Flashcard":
                Environment.Exit(0);
                break;
            case "Rename Stack":
                RenameStack();
                break;
            case "Add Cards":
                AddCards();
                break;
            case "Remove Card":
                RemoveCard();
                break;
            case "Edit Cards":
                EditCards();
                break;
            case "<-- Back":
                return;
        }
        EditMenu();
    }
}
