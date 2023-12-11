using Flashcards.Controllers;
using Flashcards.Models;
using Flashcards.Utils;
using Spectre.Console;

namespace Flashcards.UI;

public class ManageFlashcards
{
    private readonly FlashcardsController _flashcardsController;
    private readonly StacksController _stacksController;
    private Dictionary<string, string> _idMapping = new Dictionary<string, string>();

    public ManageFlashcards(FlashcardsController flashcardsController, StacksController stacksController)
    {
        _flashcardsController = flashcardsController;
        _stacksController = stacksController;
    }

    public async Task ShowMenu()
    {
        AnsiConsole.Clear();

        AnsiConsole.WriteLine("Manage Flashcards");

        bool exitManageFlashcards = false;
        Stack stack = null;
        List<Flashcard> flashcards = null;

        stack = await GetStackFromUser();

        if (stack == null)
        {
            return;
        }

        flashcards = await _flashcardsController.GetFlashcardsByStackNameAsync(stack.Name);

        // Create mapping from Id showned to user and actual id in DB
        CreateIdMapping(flashcards);

        while (!exitManageFlashcards)
        {
            AnsiConsole.Clear();

            AnsiConsole.WriteLine($"Current working stack: {stack.Name}");

            ShowMenuItems();
            Console.Write("Enter a number: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    exitManageFlashcards = true;
                    break;
                case "2":
                    await ShowFlashcards(stack.Name);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    break;
                case "3":
                    await HandleAddFlashcard(stack.Id);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    break;
                case "4":
                    await HandleUpdateFlashcard(stack);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    break;
                case "5":
                    await HandleDeleteFlashcard(stack.Name);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Invalid number.Try again");
                    break;
            }
        }
    }

    private void CreateIdMapping(List<Flashcard> flashcards)
    {
        _idMapping.Clear();

        for (int i = 0; i < flashcards.Count; i++)
        {
            _idMapping.Add((i + 1).ToString(), flashcards[i].Id.ToString());
        }
    }

    private async Task<Stack> GetStackFromUser()
    {
        Stack stack = null;

        await ShowStacks();

        String stackInput = GetStringUserInput("Enter a stack to interact with(0 to exit): ");

        if (stackInput == "0")
        {
            return null;
        }

        stack = await _stacksController.GetStackByNameAsync(stackInput);

        while (stack == null)
        {
            Console.WriteLine("No record found.Try again.");

            stackInput = GetStringUserInput("Enter a stack to interact with(0 to exit): ");

            if (stackInput == "0")
            {
                return null;
            }

            stack = await _stacksController.GetStackByNameAsync(stackInput);
        }

        return stack;
    }

    private async Task<Flashcard> HandleDeleteFlashcard(string stackName)
    {
        await ShowFlashcards(stackName);

        string strFlashcardId = GetStrFlashcardIdFromUser("Enter a id: ");

        _idMapping.TryGetValue(strFlashcardId, out string mappedId);

        int.TryParse(mappedId, out int flashcardId);

        var flashcardToBeDeleted = await _flashcardsController.GetFlashcardByIdAsync(flashcardId);
        if (flashcardToBeDeleted == null)
        {
            Console.WriteLine("Fail to find record.");
            return null;
        }

        var result = await _flashcardsController.DeleteFlashcardById(flashcardToBeDeleted.StackId, flashcardId);



        if (result != null)
        {
            Console.WriteLine("Record has been deleted!");
        }
        else
        {
            Console.WriteLine("Fail to delete record!");
        }

        return result;
    }

    private async Task<Flashcard> HandleAddFlashcard(int stackId)
    {
        string question = GetStringUserInput("Enter a question: ");

        string answer = GetStringUserInput("Enter a answer: ");

        var newFlashcard = await _flashcardsController.AddFlashcardAsync(stackId, new Flashcard
        {
            Question = question,
            Answer = answer,
            StackId = stackId

        });

        if (newFlashcard != null)
        {
            Console.WriteLine("Record has been added!");
        }
        else
        {
            Console.WriteLine("Fail to add record!");
        }

        return newFlashcard;
    }

    private async Task<Flashcard> HandleUpdateFlashcard(Stack stack)
    {
        await ShowFlashcards(stack.Name);

        string strFlashcardId = GetStrFlashcardIdFromUser("Enter a id: ");

        _idMapping.TryGetValue(strFlashcardId, out string mappedId);

        int.TryParse(mappedId, out int flashcardId);

        var flashcardToBeUpdated = await _flashcardsController.GetFlashcardByIdAsync(flashcardId);
        if (flashcardToBeUpdated == null)
        {
            Console.WriteLine("Fail to find record.");
            return null;
        }

        string question = GetStringUserInput("Enter a question: ");

        string answer = GetStringUserInput("Enter a answer: ");

        flashcardToBeUpdated.Question = question;
        flashcardToBeUpdated.Answer = answer;
        flashcardToBeUpdated.StackId = stack.Id;

        var result = await _flashcardsController.UpdateFlashcardById(flashcardId, flashcardToBeUpdated);

        if (result != null)
        {

            Console.WriteLine("Record has been updated!");
        }
        else
        {
            Console.WriteLine("Fail to update record!");
        }

        return flashcardToBeUpdated;
    }

    public void ShowMenuItems()
    {
        Console.WriteLine("1. Go back to Main Menu");
        Console.WriteLine("2. Show flashcards");
        Console.WriteLine("3. Add flashcards");
        Console.WriteLine("4. Update flashcard");
        Console.WriteLine("5. Delete flashcard");
    }

    public async Task ShowFlashcards(string stackName)
    {
        var flashcards = await _flashcardsController.GetAllFlashcardsAsync();

        flashcards = flashcards
            .Where(fc => fc.Stack.Name == stackName)
            .OrderBy(fc => fc.Id)
            .ToList();

        CreateIdMapping(flashcards);

        List<FlashcardDto> flashcardDtos = new List<FlashcardDto>();

        for (int i = 0; i < flashcards.Count; i++)
        {
            flashcardDtos.Add(new FlashcardDto
            {
                Id = i + 1,
                Question = flashcards[i].Question,
                Answer = flashcards[i].Answer,
            });
        }

        Table table = new Table();
        table.AddColumns("Id", "Question", "Answer");

        foreach (var flashcardDto in flashcardDtos)
        {
            table.AddRow(flashcardDto.Id.ToString(), flashcardDto.Question, flashcardDto.Answer);
        }

        AnsiConsole.Write(table);
    }

    public async Task ShowStacks()
    {
        var stacks = await _stacksController.GetAllStacksAsync();

        Table table = new Table();
        table.AddColumns("Stack name");

        foreach (var stack in stacks)
        {
            table.AddRow(stack.Name);
        }

        AnsiConsole.Write(table);
    }

    private string GetStringUserInput(string message)
    {
        Console.Write(message);
        string input = Console.ReadLine();
        while (!Validate.IsValidString(input))
        {
            Console.WriteLine("Invalid input.Try again.");
            Console.Write(message);
            input = Console.ReadLine();
        }

        return input;
    }
    private string GetStrFlashcardIdFromUser(string message)
    {
        Console.Write(message);
        string input = Console.ReadLine();

        while (!Validate.IsValidString(input) || !Validate.IsValidNumber(input))
        {
            Console.WriteLine("Invalid number.Try again.");
            Console.Write(message);
            input = Console.ReadLine();
        }

        return input;
    }
}