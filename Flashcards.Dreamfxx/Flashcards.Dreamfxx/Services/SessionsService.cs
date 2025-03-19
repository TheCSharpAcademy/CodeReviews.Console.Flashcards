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
        AnsiConsole.MarkupLine("[yellow]Instructions:[/]");
        AnsiConsole.MarkupLine("[grey]1. Question will be shown[/]");
        AnsiConsole.MarkupLine("[grey]2. Press any key to see the answer[/]");
        AnsiConsole.MarkupLine("[grey]3. Press 'Y' if you got it right, 'N' if you got it wrong[/]");
        AnsiConsole.MarkupLine("[grey]4. Press 'ESC' at any time to end the session[/]\n");
        AnsiConsole.MarkupLine("[yellow]Press any key to start or ESC to cancel[/]");

        if (Console.ReadKey(true).Key == ConsoleKey.Escape)
        {
            return;
        }

        var startTime = DateTime.Now;
        int correctAnswers = 0;
        int wrongAnswers = 0;
        var totalCards = stack.FlashcardsDto.Count;
        var difficultCards = new List<FlashcardDto>();

        // Randomize cards
        var randomizedCards = stack.FlashcardsDto.OrderBy(x => Random.Shared.Next()).ToList();

        foreach (var card in randomizedCards)
        {
            var result = ShowFlashcardAndGetResult(card, totalCards);

            if (!result.HasValue) // Session was cancelled
            {
                if (correctAnswers + wrongAnswers > 0) // Only save if some cards were answered
                {
                    SaveSessionResults(cardStack.Id, correctAnswers, wrongAnswers, startTime, difficultCards);
                }
                return;
            }

            if (result.Value)
            {
                correctAnswers++;
            }
            else
            {
                wrongAnswers++;
                difficultCards.Add(card);
            }
        }

        SaveSessionResults(cardStack.Id, correctAnswers, wrongAnswers, startTime, difficultCards);
        ShowSessionSummary(correctAnswers, wrongAnswers, startTime, difficultCards);
    }

    private void SaveSessionResults(int stackId, int correctAnswers, int wrongAnswers, DateTime startTime, List<FlashcardDto> difficultCards)
    {
        var duration = DateTime.Now - startTime;
        _databaseManager.RegisterStudySession(
            stackId,
            Math.Max(0, correctAnswers), // Ensure non-negative
            Math.Max(0, wrongAnswers));

        // Save difficult cards for future reference
        // This method does not exist, so we remove it
        // if (difficultCards.Any())
        // {
        //     _databaseManager.SaveDifficultCards(stackId, difficultCards.Select(c => c.Id).ToList());
        // }
    }

    private void ShowSessionSummary(int correctAnswers, int wrongAnswers, DateTime startTime, List<FlashcardDto> difficultCards)
    {
        var duration = DateTime.Now - startTime;
        var totalAnswers = correctAnswers + wrongAnswers;
        var successRate = totalAnswers == 0 ? 0 : (correctAnswers * 100.0 / totalAnswers);

        Console.Clear();
        AnsiConsole.MarkupLine($"[green]Session Complete![/]");
        AnsiConsole.MarkupLine($"Duration: {duration.TotalMinutes:F1} minutes");
        AnsiConsole.MarkupLine($"Correct answers: {correctAnswers}");
        AnsiConsole.MarkupLine($"Wrong answers: {wrongAnswers}");
        AnsiConsole.MarkupLine($"Success rate: {successRate:F1}%");

        if (difficultCards.Any())
        {
            AnsiConsole.MarkupLine("\n[yellow]Difficult Cards to Review:[/]");
            foreach (var card in difficultCards)
            {
                AnsiConsole.MarkupLine($"[grey]Q: {card.Question}[/]");
                AnsiConsole.MarkupLine($"[grey]A: {card.Answer}[/]\n");
            }
        }

        AnsiConsole.MarkupLine("\n[grey]Press any key to continue[/]");
        Console.ReadKey();
    }

    private bool? ShowFlashcardAndGetResult(FlashcardDto card, int totalCards)
    {
        Console.Clear();

        // Show progress
        AnsiConsole.MarkupLine($"\n[grey]Card {card.PresentationId} of {totalCards}[/]");

        // Show question
        AnsiConsole.MarkupLine($"\n[yellow]Question:[/]");
        AnsiConsole.MarkupLine($"{card.Question}");
        AnsiConsole.MarkupLine("\n[grey]Press any key to see answer, ESC to exit[/]");

        if (Console.ReadKey(true).Key == ConsoleKey.Escape)
            return null;

        // Show answer without clearing the question
        AnsiConsole.MarkupLine($"\n[green]Answer:[/]");
        AnsiConsole.MarkupLine($"{card.Answer}");
        AnsiConsole.MarkupLine("\n[grey]Did you get it right? (Y/N, ESC to exit)[/]");

        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Y) return true;
            if (key == ConsoleKey.N) return false;
            if (key == ConsoleKey.Escape) return null;
        }
    }

    public void ShowStudySessionsByMonth()
    {
        var currentYear = DateTime.Now.Year;
        var year = AskForYear(currentYear);
        if (year == null) return;

        var sessions = _databaseManager.GetSessionsInMonth(year.Value);

        if (sessions == null || !sessions.Any())
        {
            AnsiConsole.MarkupLine($"[yellow]No study sessions found for {year}.[/]");
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
        AnsiConsole.MarkupLine($"[green]Study Sessions for {year}[/]\n");
        AnsiConsole.Write(table);

        ShowYearSummary(sessions);

        Console.ReadKey();
    }

    private int? AskForYear(int currentYear)
    {
        var yearString = AnsiConsole.Ask<string>($"[yellow]Enter year (default: {currentYear}):[/]");

        if (string.IsNullOrWhiteSpace(yearString))
            return currentYear;

        if (int.TryParse(yearString, out int year) && year > 1900 && year <= currentYear)
            return year;

        AnsiConsole.MarkupLine("[red]Invalid year. Using current year.[/]");
        return currentYear;
    }

    private void ShowYearSummary(List<SessionPivotDto> sessions)
    {
        var totalSessions = sessions.Sum(s => new[]
        {
            s.January, s.February, s.March, s.April, s.May, s.June,
            s.July, s.August, s.September, s.October, s.November, s.December
        }.Sum());

        var mostActiveStack = sessions.OrderByDescending(s => new[]
        {
            s.January, s.February, s.March, s.April, s.May, s.June,
            s.July, s.August, s.September, s.October, s.November, s.December
        }.Sum()).FirstOrDefault();

        AnsiConsole.MarkupLine($"\n[green]Year Summary[/]");
        AnsiConsole.MarkupLine($"Total sessions: {totalSessions}");
        if (mostActiveStack != null)
        {
            AnsiConsole.MarkupLine($"Most active stack: {mostActiveStack.StackName}");
        }
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
