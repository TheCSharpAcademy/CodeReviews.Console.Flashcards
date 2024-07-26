using Flashcards.Controllers;
using Flashcards.Models;

namespace Flashcards.CardView;

public static class CardViewController
{
    public static void InitMainView()
    {
        Console.Clear();

        Table table = new();
        table.AddColumns(["ID", "name"]);

        var stackList = StackController.GetAllStacks();

        foreach (var stack in stackList) table.AddRow(stack.Id.ToString(), stack.Name);

        Console.WriteLine("List of stacks:");
        AnsiConsole.Write(table);

        var input = GetMainInput(stackList);

        if (input == -1) InitCardCreateView(stackList);
        else if (input == 0) return;
        else InitCardView(input);
    }

    public static void InitCardCreateView(List<Stack> stackList)
    {
        var stack = GetMainInput(stackList, false);
        if (stack == 0) return;
        var front = GetNewCardFront();
        if (front == "0") return;
        var back = GetNewCardBack();
        if (back == "0") return;

        CardController.CreateCard(front, back, stack);
        Console.WriteLine("Entry created successfully! Press any key to continue...");
        Console.ReadKey();
    }

    public static void InitCardView(int input)
    {
        var results = CardController.GetCardDTOList(input);

        Table table = new();
        table.AddColumns(["stack", "front", "back"]);

        foreach (var card in results) table.AddRow(card.StackName, card.Front, card.Back);

        Console.WriteLine("List of cards for this stack:");
        AnsiConsole.Write(table);
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public static int GetMainInput(List<Stack> stackList, bool cardView=true)
    {
        Console.WriteLine($"Enter the ID of the stack you would like to select, {(cardView ? "type c to create a card, " : "")}or enter 0 to go back.");
        var input = Console.ReadLine() ?? "";

        if (cardView && input.Equals("c", StringComparison.CurrentCultureIgnoreCase))
        {
            return -1;
        }

        if (!int.TryParse(input, out int index))
        {
            Console.WriteLine("Invalid input. Please input the number of the stack you would like to select.");
            return GetMainInput(stackList);
        }
        else if (!stackList.Exists(stack => stack.Id == index) && index != 0)
        {
            Console.WriteLine("Stack not found. Try again.");
            return GetMainInput(stackList);
        }

        return index;
    }

    public static string GetNewCardFront()
    {
        Console.WriteLine("Enter the front for this card, or press 0 to go back. Max length 1000 characters.");
        var input = Console.ReadLine() ?? "";

        if (string.IsNullOrEmpty(input) || input.Length > 20)
        {
            Console.WriteLine("Invalid name. Try again.");
            return GetNewCardFront();
        }

        return input;
    }

    public static string GetNewCardBack()
    {
        Console.WriteLine("Enter the back for this card, or press 0 to go back. Max length 1000 characters.");
        var input = Console.ReadLine() ?? "";

        if (string.IsNullOrEmpty(input) || input.Length > 20)
        {
            Console.WriteLine("Invalid name. Try again.");
            return GetNewCardBack();
        }

        return input;
    }
}