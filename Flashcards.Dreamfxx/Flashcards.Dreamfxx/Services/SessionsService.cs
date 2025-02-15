using Flashcards.Dreamfxx.Data;
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
        var flashcardsService = new FlashcardsService(_databaseManager);
        Console.Clear();

        AnsiConsole.MarkupLine("Select the stack name:");
        var cardStack = stackService.ShowAllStacks();

        if (cardStack == null)
        {
            AnsiConsole.MarkupLine("There are no cards in this stack.");
            Console.ReadKey();
            return;
        }
        var stack = _databaseManager.GetStackDtos(cardStack.Id);

        if (stack == null || stack.FlashcardsDto == null || !stack.FlashcardsDto.Any())
        {
            AnsiConsole.MarkupLine("There are no cards in this stack.");
            Console.ReadKey();
            return;
        }

        Console.Clear();

        AnsiConsole.MarkupLine($"Selected stack: {cardStack.Name}\n");

        foreach (var card in stack.FlashcardsDto)
        {
            AnsiConsole.MarkupLine($"Question: {card.Question}");
            Console.ReadKey();
            Console.Clear();
            AnsiConsole.MarkupLine($"Answer: {card.Answer}");
            Console.ReadKey();
            Console.Clear();
        }
    }

    public void ShowStudySessionsByMonth()
    {
        var sessions = _databaseManager.GetSessionsInMonth(DateTime.Now.Year);

        if (sessions == null || !sessions.Any())
        {
            AnsiConsole.MarkupLine("No study sessions found.");
            return;
        }

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Stack Name");
        table.AddColumn("January");
        table.AddColumn("February");
        table.AddColumn("March");
        table.AddColumn("April");
        table.AddColumn("May");
        table.AddColumn("June");
        table.AddColumn("July");
        table.AddColumn("August");
        table.AddColumn("September");
        table.AddColumn("October");
        table.AddColumn("November");
        table.AddColumn("December");

        foreach (var session in sessions)
        {
            table.AddRow(
                session.Id.ToString(),
                session.StackName ?? "N/A",
                session.January?.ToString() ?? "0",
                session.February?.ToString() ?? "0",
                session.March?.ToString() ?? "0",
                session.April?.ToString() ?? "0",
                session.May?.ToString() ?? "0",
                session.June?.ToString() ?? "0",
                session.July?.ToString() ?? "0",
                session.August?.ToString() ?? "0",
                session.September?.ToString() ?? "0",
                session.October?.ToString() ?? "0",
                session.November?.ToString() ?? "0",
                session.December?.ToString() ?? "0"
            );
        }

        AnsiConsole.Write(table);
    }
}
