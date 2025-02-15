using Flashcards.Dreamfxx.Data;
using Flashcards.Dreamfxx.Dtos;
using Flashcards.Dreamfxx.UserInput;
using Spectre.Console;

namespace Flashcards.Dreamfxx.Services;
public class FlashcardsService
{
    private readonly DatabaseManager _databaseManager;

    public FlashcardsService(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public void CreateCard()
    {
        var stackService = new StacksService(_databaseManager);
        AnsiConsole.MarkupLine("Select the stack name:");
        var stack = stackService.ShowAllStacks();

        Console.Clear();

        while (true)
        {
            AnsiConsole.MarkupLine($"Selected stack: {stack.Name}\n");
            string? question = GetUserInput.GetUserString("Enter the question:");

            string? answer = GetUserInput.GetUserString("Enter the answer:");

            _databaseManager.CreateCard(question, answer, stack.Id);

            AnsiConsole.MarkupLine("Card created successfully!");
            AnsiConsole.MarkupLine("Do you want to add another card? - y/n");

            if (Console.ReadLine() == "n")
            {
                AnsiConsole.MarkupLine("Bye!");
                break;
            }
            Console.Clear();
        }
    }

    public void EditCard()
    {
        Console.Clear();

        while (true)
        {
            AnsiConsole.MarkupLine("Select the card stack:");
            var cardStackService = new StacksService(_databaseManager);
            var stack = cardStackService.ShowAllStacks();

            if (stack == null)
            {
                Console.ReadKey();
                break;
            }

            else if (stack.Name == "Cancel")
            {
                AnsiConsole.MarkupLine("Operation canceled!");
                Console.ReadKey();
                break;
            }

            int stackId = stack.Id;
            var card = _databaseManager.GetCards().FirstOrDefault(c => c.Id == stackId);

            if (card == null)
            {
                AnsiConsole.MarkupLine("No card found for the selected stack.");
                Console.ReadKey();
                break;
            }

            AnsiConsole.MarkupLine($"Current question: {card.Question} \n");
            string? question = GetUserInput.GetUserString("Enter the new question\n(Leave blank if you don't want to edit):");

            if (string.IsNullOrEmpty(question))
            {
                question = card.Question;
            }

            AnsiConsole.MarkupLine($"Current answer: {card.Answer} \n");
            string? answer = GetUserInput.GetUserString("Enter the new answer(Leave blank if you don't want to edit):");

            if (string.IsNullOrEmpty(answer))
            {
                answer = card.Answer;
            }

            AnsiConsole.MarkupLine("Old card: ");
            AnsiConsole.MarkupLine($"Question: {card.Question} - Answer: {card.Answer}\n");

            AnsiConsole.MarkupLine("New card: ");
            AnsiConsole.MarkupLine($"Question: {question} - Answer: {answer}\n");

            if (!AccepttheOperation("update"))
            {
                break;
            }

            _databaseManager.UpdateCard(question, answer, card.Id);

            AnsiConsole.MarkupLine("Card updated successfully!");
            AnsiConsole.MarkupLine("Do you want to edit another card?(y/n)");

            if (Console.ReadLine() == "n")
            {
                break;
            }
        }
    }

    public void DeleteCard()
    {
        Console.Clear();

        while (true)
        {
            AnsiConsole.MarkupLine("Select the card stack\n");
            var stackService = new StacksService(_databaseManager);
            var stacks = stackService.ShowAllStacks();

            if (stacks == null)
            {
                Console.ReadKey();
                break;
            }

            int stackId = stacks.Id;

            AnsiConsole.MarkupLine("Select the card ID:");
            var card = ShowAndSelectById(stackId);

            if (card == null)
            {
                Console.ReadKey();
                break;
            }

            AnsiConsole.MarkupLine($"You picked this flashcard:");
            AnsiConsole.MarkupLine($"Question: {card.Question} - Answer: {card.Answer}");

            if (!AccepttheOperation("delete"))
            {
                break;
            }

            _databaseManager.DeleteCard(card.Id);

            AnsiConsole.MarkupLine("Card deleted successfully!");
            AnsiConsole.MarkupLine("Do you want to delete another card?(y/n)");

            if (Console.ReadLine() == "n")
            {
                break;
            }
        }
    }

    public FlashcardDto? ShowAndSelectById(int stackId)
    {
        var stack = _databaseManager.GetStackDtos(stackId);

        if (stack == null || stack.FlashcardsDto == null)
        {
            Console.ReadKey();
            return null;
        }

        var cards = stack.FlashcardsDto;

        var menuSelection = AnsiConsole.Prompt(
            new SelectionPrompt<FlashcardDto>()
            .Title("Select a card")
            .PageSize(10)
            .AddChoices(cards)
            .UseConverter(card => $"ID: {card.PresentationId}, Question: {card.Question}, Answer: {card.Answer}"));

        return menuSelection;
    }

    public bool AccepttheOperation(string operation)
    {
        var menuSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"Are you sure you want to {operation} this card?")
            .PageSize(10)
            .AddChoices(new[] { "Yes", "No" })
            .UseConverter(option => option));

        if (menuSelection == "Yes")
        {
            return true;
        }
        AnsiConsole.MarkupLine("Cancelled.");
        return false;
    }
}
