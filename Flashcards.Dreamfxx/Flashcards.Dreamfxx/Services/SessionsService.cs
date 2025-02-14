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

    internal void ShowStudySessions()
    {
        throw new NotImplementedException();
    }
}
