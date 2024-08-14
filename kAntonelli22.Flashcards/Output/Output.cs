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
                new Card($"Question {i + 1}", $"Answer {i + 1}", CurrentStack);
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
        string query = $"DELETE FROM dbo.Cards WHERE Stack_Id = '{CurrentStack.Id}'";
        DatabaseHelper.RunQuery(query);
        query = $"DELETE FROM dbo.Stacks WHERE StackName = '{CurrentStack.StackName}'";
        DatabaseHelper.RunQuery(query);
    } // end of EditStack Method

    public static void ViewSessions(bool displayMenu)
    {
        Console.Clear();
        var table = new Table();
        table.Border = TableBorder.Markdown;
        table.Title = new($"[blue]Sessions[/]");

        TableColumn[] columns = [
        new TableColumn(""), 
        new TableColumn("[blue]Session Date[/]").Centered(), 
        new TableColumn("[blue]Stack Studdied[/]").Centered(), 
        new TableColumn("[blue]Questions Attempted[/]").Centered(), 
        new TableColumn("[blue]Questions Correct[/]").Centered(),
        new TableColumn("[blue]Time Per Question[/]").Centered()
        ];
        table.AddColumns(columns);

        for (int i = 0; i < StudySession.Sessions.Count; i++)
        {
            StudySession session = StudySession.Sessions[i];
            table.AddRow($"{i + 1}", $"{session.Date}", $"{session.StackName}", $"{session.NumComplete}", $"{session.NumCorrect}", $"{session.AvgTime}");
        }

        AnsiConsole.Write(table);
        
        // Session Menu
        AnsiConsole.WriteLine("Stack Options\n------------------------");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices([
                "Exit Flashcard", "<-- Back"
                ]));

        switch (menu)
        {
            case "Exit Flashcard":
                Environment.Exit(0);
                break;
            case "<-- Back":
                return;
        }
        ViewSessions(true);
    } // end of ViewStacks Method
    
    public static void ViewStacks(bool displayMenu)
    {
        Console.Clear();

        List<Table> tables = [];

        for (int i = 0; i < CardStack.Stacks.Count; i++)
        {
            CardStack stack = CardStack.Stacks[i];
            var table = new Table();
            table.Border = TableBorder.Markdown;
            table.Title = new($"[blue]{stack.StackName}[/][grey]({stack.StackSize})[/]");

            TableColumn[] columns = [new TableColumn(""), new TableColumn("[blue]Card Front[/]").Centered(), new TableColumn("[blue]Card Back[/]").Centered()];
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
