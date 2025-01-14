using Spectre.Console;

internal class StudySession
{
    private static readonly Dictionary<string, Action> _sessionActions = new()
    {
        { DisplayInfoHelpers.Back, Console.Clear },
        { "Start new study session", Study },
        { "Show all sessions", SessionRead.ShowAllSessions },
        { "Show quick report", QuickReport.ShowReportTables }
    };

    internal static void ShowStudyMenu()
    {
        Console.Clear();

        var sessionAction = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an action: ")
            .PageSize(10)
            .AddChoices(_sessionActions.Keys));

        _sessionActions[sessionAction]();
    }

    private static void Study()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("Starting study session.\n");

        var stack = StackRead.GetStack();
        if (stack.StackName == null)
        {
            Console.Clear();
            return;
        }

        var flashcards = FlashcardRead.GetListOfFlashcards(stack.StackId);
        if (DisplayInfoHelpers.NoRecordsAvailable(flashcards)) return;

        var limit = 5; //number of cards for study session
        var sessionFlashcards = new List<Flashcard>();
        var flashcardsCopy = flashcards.OrderBy(x => Guid.NewGuid()).ToList();

        for (int i = 0; i < limit; i++)
        {
            sessionFlashcards.Add(flashcardsCopy[i % flashcardsCopy.Count]);
        }

        AnsiConsole.MarkupLine(
            "Flashcards for study session prepared. You can start your session.");
        DisplayInfoHelpers.PressAnyKeyToContinue();

        var (date, score) = StudySessionLoop(sessionFlashcards);
        if (score == -1)
        {
            Console.Clear();
            return;
        }

        ShowSessionInfo(stack, date, score, limit);
        SessionCreate.AddNewSession(stack, date, score);

        DisplayInfoHelpers.PressAnyKeyToContinue();
    }

    private static (DateTime, int) StudySessionLoop(List<Flashcard> flashcards)
    {
        var date = DateTime.Now;
        int card = 1;
        int score = 0;
        foreach (var flashcard in flashcards)
        {
            AnsiConsole.MarkupLine($"Session Date: [yellow]{date:yyyy-MM-dd}[/] " +
                $"Time: [yellow]{date:HH:mm}[/]");
            AnsiConsole.MarkupLine($"Card [yellow]{card}[/], your score: [yellow]{score}[/]\n");

            var panel = new Panel($"{flashcard.FlashcardFront}");
            AnsiConsole.Write(panel);

            var input = AnsiConsole.Ask<string>("Enter your answer for this card (0 for exit):");
            input = input.Trim().ToLower();

            if (input == "0")
            {
                Console.Clear();
                return (date, -1);
            }
            else if (input == flashcard.FlashcardBack)
            {
                AnsiConsole.MarkupLine("\n[green]Correct![/]");
                score++;
            }
            else
            {
                AnsiConsole.MarkupLine("\n[red]Wrong answer.[/]");
                AnsiConsole.MarkupLine($"The correct answer is [yellow]{flashcard.FlashcardBack}[/]");
            }

            DisplayInfoHelpers.PressAnyKeyToContinue();
            card++;
        }
        return (date, score);
    }

    private static void ShowSessionInfo(Stack stack, DateTime date, int score, int limit)
    {
        AnsiConsole.MarkupLine($"Session Date: [yellow]{date:yyyy-MM-dd}[/] " +
                $"Time: [yellow]{date:HH:mm}[/]");
        AnsiConsole.MarkupLine("\n[green]Study session finished![/]");
        AnsiConsole.MarkupLine($"\nYour total session score for [yellow]{stack.StackName}[/] " +
            $"stack is [green]{score}[/] out of {limit}");

        if (score == limit)
        {
            AnsiConsole.MarkupLine("[green]Congratulations! You got a perfect score![/]");
        }
    }
}
