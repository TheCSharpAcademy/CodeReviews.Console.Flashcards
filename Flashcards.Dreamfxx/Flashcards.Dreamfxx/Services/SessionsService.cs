using Flashcards.Dreamfxx.Data;
using Flashcards.Dreamfxx.Dtos;
using Spectre.Console;

namespace Flashcards.Dreamfxx.Services;
public class SessionsService
{
    private readonly DatabaseManager _databaseManager;
    public SessionsService(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public void StartSession()
    {
        var stackService = new StacksService(_databaseManager);
        Console.Clear();

        AnsiConsole.MarkupLine("[yellow]Select the stack name:[/]");
        var cardStack = stackService.ShowAllStacks();

        if (cardStack == null)
        {
            AnsiConsole.MarkupLine("[red]No stacks available.[/]");
            Console.ReadKey();
            return;
        }

        var stack = _databaseManager.GetStackDtos(cardStack.Id);

        if (stack == null || stack.FlashcardsDto == null || !stack.FlashcardsDto.Any())
        {
            AnsiConsole.MarkupLine("[red]There are no cards in this stack.[/]");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        AnsiConsole.MarkupLine($"[green]Selected stack: {cardStack.Name}[/]\n");
        AnsiConsole.MarkupLine("[yellow]Press any key to start the session. For each card:[/]");
        AnsiConsole.MarkupLine("[grey]1. Question will be shown[/]");
        AnsiConsole.MarkupLine("[grey]2. Press any key to see the answer[/]");
        AnsiConsole.MarkupLine("[grey]3. Press 'Y' if you got it right, 'N' if you got it wrong[/]\n");
        Console.ReadKey();

        int correctAnswers = 0;
        int wrongAnswers = 0;

        foreach (var card in stack.FlashcardsDto)
        {
            bool? isCorrect = ShowFlashcardAndGetResult(card);
            if (isCorrect.HasValue)
            {
                if (isCorrect.Value) correctAnswers++;
                else wrongAnswers++;
            }
        }

        // Register the study session
        _databaseManager.RegisterStudySession(cardStack.Id, correctAnswers, wrongAnswers);

        // Show session results
        Console.Clear();
        AnsiConsole.MarkupLine($"[green]Session Complete![/]");
        AnsiConsole.MarkupLine($"Correct answers: {correctAnswers}");
        AnsiConsole.MarkupLine($"Wrong answers: {wrongAnswers}");
        AnsiConsole.MarkupLine($"Success rate: {(correctAnswers * 100.0 / (correctAnswers + wrongAnswers)):F1}%");
        Console.ReadKey();
    }

    private bool? ShowFlashcardAndGetResult(FlashcardDto card)
    {
        Console.Clear();
        AnsiConsole.MarkupLine($"[yellow]Question {card.PresentationId}:[/] {card.Question}");
        Console.ReadKey();

        Console.Clear();
        AnsiConsole.MarkupLine($"[yellow]Question {card.PresentationId}:[/] {card.Question}");
        AnsiConsole.MarkupLine($"[green]Answer:[/] {card.Answer}");
        AnsiConsole.MarkupLine("\n[grey]Did you get it right? - Y/N[/]");

        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Y) return true;
            if (key == ConsoleKey.N) return false;
        }
    }

    public void ShowStudySessionsByMonth()
    {
        var sessions = _databaseManager.GetSessionsInMonth(DateTime.Now.Year);

        if (sessions == null || !sessions.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No study sessions found for this year.[/]");
            Console.ReadKey();
            return;
        }

        var table = CreateSessionsTable();

        int id = 1;
        foreach (var session in sessions)
        {
            session.Id = id++;
            AddSessionToTable(table, session);
        }

        Console.Clear();
        AnsiConsole.MarkupLine($"[green]Study Sessions for {DateTime.Now.Year}[/]\n");
        AnsiConsole.Write(table);
        Console.ReadKey();
    }

    private Table CreateSessionsTable()
    {
        var table = new Table();
        table.AddColumn(new TableColumn("[yellow]ID[/]").Centered());
        table.AddColumn(new TableColumn("[yellow]Stack Name[/]"));
        table.AddColumn(new TableColumn("[blue]Jan[/]").Centered());
        table.AddColumn(new TableColumn("[blue]Feb[/]").Centered());
        table.AddColumn(new TableColumn("[blue]Mar[/]").Centered());
        table.AddColumn(new TableColumn("[blue]Apr[/]").Centered());
        table.AddColumn(new TableColumn("[blue]May[/]").Centered());
        table.AddColumn(new TableColumn("[blue]Jun[/]").Centered());
        table.AddColumn(new TableColumn("[blue]Jul[/]").Centered());
        table.AddColumn(new TableColumn("[blue]Aug[/]").Centered());
        table.AddColumn(new TableColumn("[blue]Sep[/]").Centered());
        table.AddColumn(new TableColumn("[blue]Oct[/]").Centered());
        table.AddColumn(new TableColumn("[blue]Nov[/]").Centered());
        table.AddColumn(new TableColumn("[blue]Dec[/]").Centered());

        table.Border(TableBorder.Square);
        return table;
    }

    private void AddSessionToTable(Table table, SessionPivotDto session)
    {
        table.AddRow(
            session.Id.ToString(),
            session.StackName ?? "N/A",
            (session.January ?? 0).ToString(),
            (session.February ?? 0).ToString(),
            (session.March ?? 0).ToString(),
            (session.April ?? 0).ToString(),
            (session.May ?? 0).ToString(),
            (session.June ?? 0).ToString(),
            (session.July ?? 0).ToString(),
            (session.August ?? 0).ToString(),
            (session.September ?? 0).ToString(),
            (session.October ?? 0).ToString(),
            (session.November ?? 0).ToString(),
            (session.December ?? 0).ToString()
        );
    }
}