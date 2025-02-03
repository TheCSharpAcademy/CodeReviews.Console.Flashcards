using Flashcards.jollejonas.Data;
using Flashcards.jollejonas.Models;
using Flashcards.jollejonas.UserInputs;
using Spectre.Console;


namespace Flashcards.jollejonas.Services;

public class CardStackService(DatabaseManager databaseManager)
{
    private readonly DatabaseManager _databaseManager = databaseManager;

    public CardStack DisplayAndSelectCardStacks()
    {
        var cardStacks = _databaseManager.GetCardStacks();
        cardStacks.Add(new CardStack { Id = 0, Name = "Cancel" });
        if (cardStacks == null)
        {
            Console.ReadKey();
            return null;
        }
        var menuSelection = AnsiConsole.Prompt(
            new SelectionPrompt<CardStack>()
            .Title("Select an option")
            .PageSize(10)
            .AddChoices(cardStacks)
            .UseConverter(option => option.Name));

        return menuSelection;
    }

    public void DisplayCardStacks()
    {
        var cardStacks = _databaseManager.GetCardStacks();
        if (cardStacks == null)
        {
            Console.ReadKey();
            return;
        }
        foreach (var stack in cardStacks)
        {
            Console.WriteLine($"ID: {stack.Id}, Name: {stack.Name}, Description: {stack.Description}");
        }
    }

    public void GetSelectedCardStack()
    {
        var selectedStackId = DisplayAndSelectCardStacks().Id;
        var selectedStack = _databaseManager.GetCardStackDTOs(selectedStackId);

        Console.WriteLine($"Selected stack: {selectedStack.CardStackName}");

        foreach (var card in selectedStack.Cards)
        {
            Console.WriteLine($"Question: {card.Question}, Answer: {card.Answer}");
        }
        Console.ReadKey();
    }

    public void CreateStack()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("Existing stacks: ");
            DisplayCardStacks();
            string name = UserInput.GetStringInput("Enter the name of the stack (or 'q' to cancel):");

            if (name == "q")
            {
                Console.WriteLine("Operation canceled!");
                Console.ReadKey();
                break;
            }

            string description = UserInput.GetStringInput("Enter the description of the stack:");

            _databaseManager.CreateStack(name, description);

            Console.WriteLine("Stack created successfully!");
            Console.WriteLine("Do you want to add another stack?(y/n)");

            if (Console.ReadLine().ToLower() == "n")
            {
                break;
            }
        }
    }

    public void EditStack()
    {
        Console.Clear();
        while (true)
        {
            var cardStack = DisplayAndSelectCardStacks();
            if (cardStack == null)
            {
                Console.ReadKey();
                break;
            }
            else if (cardStack.Name == "Cancel")
            {
                Console.WriteLine("Operation canceled!");
                Console.ReadKey();
                break;
            }
            Console.WriteLine($"Current stack name: {cardStack.Name} \n");
            string name = UserInput.GetStringInput("Enter the new name of the stack(Leave blank if you don't want to edit):");

            if (name == null)
            {
                name = cardStack.Name;
            }

            Console.WriteLine($"Current description: {cardStack.Description} \n");
            string description = UserInput.GetStringInput("Enter the new description of the stack(Leave blank if you don't want to edit):");

            if (description == null)
            {
                description = cardStack.Description;
            }

            Console.WriteLine("Old stack: ");
            Console.WriteLine($"Stack name: {cardStack.Name} - Stack description: {cardStack.Description}\n");

            Console.WriteLine("New stack: ");
            Console.WriteLine($"Stack name: {name} - Stack description: {description}\n");

            if (!Confirmations("update"))
            {
                break;
            }

            _databaseManager.UpdateStack(name, description, cardStack.Id);

            Console.WriteLine("Stack updated successfully!");
            Console.WriteLine("Do you want to edit another stack?(y/n)");

            if (Console.ReadLine().ToLower() == "n")
            {
                break;
            }
        }
    }

    public void DeleteStack()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("Select the stack you want to delete:");
            int stackId = DisplayAndSelectCardStacks().Id;

            if (!Confirmations("delete"))
            {
                break;
            }
            _databaseManager.DeleteStack(stackId);

            Console.WriteLine("Stack deleted successfully!");
            Console.WriteLine("Do you want to delete another stack?(y/n)");

            if (Console.ReadLine().ToLower() == "n")
            {
                break;
            }
        }
    }

    public bool Confirmations(string operation)
    {

        var menuSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"Are you sure you want to {operation} this stack?")
            .PageSize(10)
            .AddChoices(new[] { "Yes", "No" })
            .UseConverter(option => option));

        if (menuSelection == "Yes")
        {
            return true;
        }
        return false;
    }
}
