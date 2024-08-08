using Spectre.Console;
using Flashcards.DatabaseUtilities;

namespace Flashcards;

internal class Output
{
    public static CardStack? CurrentStack;
    public static void CreateStack()
    {
        Console.Clear();
        string name;
        while (true)
        {
            name = AnsiConsole.Ask<string>("What is the Stacks name?");
            if (OutputUtilities.NameUnique(name, CardStack.Stacks))
                break;
            AnsiConsole.MarkupLine("[red]Stack names must be unique[/]");
        }
        int size = AnsiConsole.Ask<int>("What is the Stacks size?");
        CurrentStack = new(name, size);

        bool empty = AnsiConsole.Confirm("Do you want to leave the Stack empty?");
        if (empty)
        {
            for (int i = 0; i < size; i++)
                new Card($"Question {i}", $"Answer {i}", CurrentStack);
        }
        else if (!empty)
        {
            for (int i = 0; i < size; i++)
            {
                string question = AnsiConsole.Ask<string>("What is the cards question?");
                string answer = AnsiConsole.Ask<string>("What is the answer?");
                new Card(question, answer, CurrentStack);
            }
        }
        DatabaseHelper.InsertStack(CurrentStack);
    } // end of CreateStack Method

    public static void RemoveStack()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("Which Stack do you want to remove?");
        string input = OutputUtilities.DisplayStack(CardStack.Stacks);

        CurrentStack = CardStack.Stacks.FirstOrDefault(stack => stack.StackName == input);
        if (CurrentStack == null)
            return;

        CardStack.Stacks.Remove(CurrentStack);
    } // end of EditStack Method

    public static void ViewStacks(bool displayMenu)
    {
        Console.Clear();

        List<Table> tables = [];

        for (int i = 0; i < CardStack.Stacks.Count; i++)
        {
            CardStack stack = CardStack.Stacks[i];
            var table = new Table();
            table.Border = TableBorder.Markdown;
            table.Title = new($"[blue]{stack.StackName} - {stack.StackSize}[/]");

            TableColumn[] columns = [new(""), new("[blue]Card Front[/]"), new("[blue]Card Back[/]")];
            table.AddColumns(columns);

            for (int j = 0; j < stack.Cards.Count; j++)
            {
                table.AddRow($"{j + 1}", $"{stack.Cards[j].Front}", $"{stack.Cards[j].Back}");
            }
            tables.Add(table);
        }

        AnsiConsole.Write(new Columns(tables));
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
                "Remove Stack", "<-- Back"
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
                EditStack.EditMenu();
                break;
            case "Remove Stack":
                RemoveStack();
                break;
            case "<-- Back":
                return;
        }
        ViewStacks(true);
    }
}
