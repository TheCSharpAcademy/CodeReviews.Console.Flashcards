using Flashcards.DreamFXX.Data;
using Flashcards.DreamFXX.DTOs;
using Flashcards.DreamFXX.Models;
using Flashcards.DreamFXX.UserInput;
using Spectre.Console;

namespace Flashcards.DreamFXX.Services;

public class CardStackService
{
    private readonly DbManager dbManager;

    public CardStackService(DbManager dbManager)
    {
        this.dbManager = dbManager;
    }

    public StackofCards DisplayExistingStacks()
    {
        var cardStacks = dbManager.GetCardStacks();
        if (cardStacks == null)
        {
            Console.WriteLine("Stack list is empty. Add next records? -- Press Enter");
            Console.ReadKey();
            return null;
        }

        StackofCards? menuSelection = AnsiConsole.Prompt(
            new SelectionPrompt<StackofCards>()
            .Title("What do you want me to do?")
            .PageSize(10)
            .AddChoices(cardStacks)
            .UseConverter(option => option.Name));

        return menuSelection;
    }

    public void DisplayCardStacks()
    {
        var cardStacks = dbManager.GetCardStacks();
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
        var selectedStackId = DisplayExistingStacks().Id;

        var selectedStack = dbManager.GetCardStackDetails(selectedStackId);

        Console.WriteLine($"Selected stack: {selectedStack.CardStackName}");

        foreach (var card in selectedStack.Cards)
        {
            Console.WriteLine($"Question: {card.Question}, Answer: {card.Answer}");
        }
        Console.ReadKey();
    }

    public void CreateCardStack()
    {
        Console.Clear();
        DisplayExistingStacks();
        while (true)
        {
            var (name, description) = GetStackDetailsFromUser();
            if (name == "q") break;

            dbManager.CreateStack(name, description);
            Console.WriteLine("Stack created successfully!");

            if (!AskUserToContinue("Do you want to add another stack? (y/n)")) break;
        }
    }

    public void DisplayExistingStacksInternal()
    {
        Console.WriteLine("Existing stacks: ");
        DisplayCardStacks();
    }

    private static (string name, string description) GetStackDetailsFromUser()
    {
        string name = UserAnswer.GetUserAnswer("Enter the name of the stack (or 'q' to cancel):");
        string description = UserAnswer.GetUserAnswer("Enter the description of the stack:");
        return (name, description);
    }

    private static bool AskUserToContinue(string message)
    {
        Console.WriteLine(message);
        return Console.ReadLine()?.ToLower() == "y";
    }

    public void EditStack()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("Select the stack ID:");
            var cardStack = DisplayExistingStacks();
            if (cardStack == null)
            {
                Console.ReadKey();
                break;
            }

            string name = UserAnswer.GetUserAnswer("Enter the new name of the stack:");

            string description = UserAnswer.GetUserAnswer("Enter the new description of the stack:");

            Console.WriteLine("Old stack: ");
            Console.WriteLine($"Stack name: {cardStack.Name} - Stack description: {cardStack.Description}\n");

            Console.WriteLine("New stack: ");
            Console.WriteLine($"Stack name: {name} - Stack description: {description}\n");

            if (!Confirm("update"))
            {
                break;
            }

            dbManager.UpdateStack(name, description, cardStack.Id);

            Console.WriteLine("Stack updated successfully!");
            Console.WriteLine("Do you want to edit another stack?(y/n)");

            if (Console.ReadLine()?.ToLowerInvariant() == "n")
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
            Console.WriteLine("Select the stack to delete\n(by entering Stack ID - Press Enter):");
            int stackId = DisplayExistingStacks().Id;
            if (!Confirm("delete"))
            {
                break;
            }
            dbManager.DeleteStack(stackId);

            Console.WriteLine("Stack deleted successfully!");
            Console.WriteLine("Do you want to delete another stack?(y/n)");

            if (Console.ReadLine()?.ToLower() == "n")
            {
                break;
            }
        }
    }

    public static bool Confirm(string operation)
    {
        var menuSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"Are you sure you want to {operation} for selected ")
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


