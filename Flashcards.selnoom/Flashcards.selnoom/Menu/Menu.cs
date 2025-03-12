using Flashcards.selnoom.Data;
using Flashcards.selnoom.Models;
using Flashcards.selnoom.Helper;

namespace Flashcards.selnoom.Menu;

internal class Menu
{
    private readonly StackRepository _stackRepository;
    private readonly FlashcardRepository _flashcardRepository;

    public Menu(StackRepository stackRepository, FlashcardRepository flashcardRepository)
    {
        _stackRepository = stackRepository;
        _flashcardRepository = flashcardRepository;
    }

    internal void ShowMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.Write(
                "Please select an option:\n" +
                "1 - Create a stack\n" +
                "2 - View stacks\n" +
                "3 - Update a stack\n" +
                "4 - Delete a stack\n" +
                "0 - Exit program\n");

            string userInput = Console.ReadLine();
            int validatedInput = Validation.ValidateStringToInt(userInput);

            switch (validatedInput)
            {
                case 0:
                    Console.WriteLine("Goodbye!");
                    return;
                case 1:
                    ShowCreateStackMenu();
                    break;
                case 2:
                    ShowStacks();
                    Console.WriteLine("\nPress enter to continue");
                    Console.ReadLine();
                    break;
                case 3:
                    UpdateStackMenu();
                    break;
                case 4:
                    DeleteStackMenu();
                    break;
                default:
                    Console.WriteLine("\nSelected option does not exist. Press enter and try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    internal void ShowCreateStackMenu()
    {
        Console.Clear();
        Console.WriteLine("Please type the name of the stack you wish to create or 0 to return to the menu");
        string userInput = Console.ReadLine();
        if (userInput == "0")
        {
            return;
        }

        _stackRepository.CreateStack(userInput);

        Console.WriteLine("\nStack created! Press enter to continue");
        Console.ReadLine();
    }

    private List<FlashcardStack> ShowStacks()
    {
        Console.Clear();
        Console.WriteLine("Stacks:\n");
        List<FlashcardStack> stacks = new();
        stacks = _stackRepository.GetStacks();
        if (stacks.Count > 0)
        {
            for (int i = 0; i < stacks.Count; i ++)
            {
                Console.WriteLine($"{i + 1}\t{stacks[i].StackName}");
            }
        }
        else
        {
            Console.WriteLine("There are currently no stacks.\n");
        }
        return stacks;
    }

    private List<Flashcard> ShowFlashcards(int stackId)
    {
        List<Flashcard> flashcards = _flashcardRepository.GetFlashcardsByStack(stackId);
        if (flashcards.Count > 0)
        {
            Console.WriteLine("Flashcards:\n");
            for (int i = 0; i < flashcards.Count; i++)
            {
                Console.WriteLine($"{i + 1}\t Question: {flashcards[i].Question}\n");
            }
        }
        else
        {
            Console.WriteLine("There are currently no flashcards in this stack.\n");
        }
        return flashcards;
    }

    private void UpdateStackMenu()
    {
        Console.Clear();
        Console.WriteLine(
            "Do you wish to alter the name of a stack or to modify its flashcards?\n" +
            "1 - Flashcards\n" +
            "2 - Stack name\n" +
            "0 - Return to menu");
        string userInput = Console.ReadLine();
        int validatedInput = Validation.ValidateStringToInt(userInput);

        switch (validatedInput)
        {
            case 1:
                FlashcardMenu();
                break;
            case 2:
                UpdateStackName();
                break;
            case 0:
                return;
            default:
                Console.WriteLine("\nSelected option does not exist. Press enter and try again.");
                Console.ReadLine();
                return;
        }
    }

    internal void UpdateStackName()
    {
        var list = ShowStacks();
        int validatedInput = 0;

        if (list.Count <= 0)
        {
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            return;
        }

        string prompt = "Type the Id of the stack you wish to alter or 0 to return to the menu";

        validatedInput = Validation.GetValidatedId(prompt, 0, list.Count);

        Console.WriteLine("Type new stack name or 0 to return to the menu");
        string stackName = Console.ReadLine();
        if (stackName == "0")
        {
            return;
        }

        int databaseStackId = list[validatedInput - 1].StackId;

        _stackRepository.UpdateStack(databaseStackId, stackName);

        Console.WriteLine("\nStack updated! Press enter to continue.");
        Console.ReadLine();
    }

    internal void DeleteStackMenu()
    {
        var list = ShowStacks();
        int validatedInput = 0;

        if (list.Count <= 0)
        {
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            return;
        }

        string prompt = "Type the Id of the stack you wish to delete or 0 to return to the menu";

        validatedInput = Validation.GetValidatedId(prompt, 0, list.Count);

        int databaseStackId = list[validatedInput - 1].StackId;

        _stackRepository.DeleteStack(databaseStackId);

        Console.WriteLine("\nStack deleted! Press enter to continue.");
        Console.ReadLine();
    }

    internal void FlashcardMenu()
    {
        var stackList = ShowStacks();
        bool inputIsValidated = false;
        string stackId;
        int validatedStackId = 0;

        if (stackList.Count <= 0)
        {
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            return;
        }

        while (!inputIsValidated)
        {
            Console.WriteLine("Type the Id of the stack you wish to alter or 0 to return to the menu");
            stackId = Console.ReadLine();
            validatedStackId = Validation.ValidateStringToInt(stackId);
            if (validatedStackId == 0)
            {
                return;
            }

            if (validatedStackId < 1 || validatedStackId > stackList.Count)
            {
                Console.WriteLine("The selected Id does not exist. Please try again.\n");
            }
            else
            {
                inputIsValidated = true;
            }
        }

        var flashcardList = ShowFlashcards(validatedStackId);

        Console.WriteLine(
            "Select an option:\n" +
            "1 - Create a new flashcard\n" +
            "2 - Update a flashcard\n" +
            "3 - Delete a flashcard\n" +
            "0 - Return to menu");
        string userInput = Console.ReadLine();
        int validatedUserInput = Validation.ValidateStringToInt(userInput);

        switch (validatedUserInput)
        {
            case 1:
                CreateFlashcardMenu(validatedStackId, flashcardList);
                break;
            case 2:
                UpdateFlashCardMenu(validatedStackId, flashcardList);
                break;
            case 3:
                DeleteFlashCardMenu(validatedStackId, flashcardList);
                break;
            case 0:
                return;
            default:
                Console.WriteLine("\nSelected option does not exist. Press enter and try again.");
                Console.ReadLine();
                return;
        }
    }

    internal void CreateFlashcardMenu(int stackId, List<Flashcard> list)
    {
        Console.Clear();
        List<string> questions = list.Select(x => x.Question).ToList();

        string question = Validation.GetValidatedUniqueInput(
            "Type the flashcard's question or 0 to return to the menu:",
            questions,
            "\nThat question already exists. Please type a new one."
        );

        Console.Clear();
        Console.WriteLine("Type the flashcards answer or 0 to return to the menu:");
        string answer = Console.ReadLine();
        if (answer == "0")
        {
            return;
        }

        _flashcardRepository.CreateFlashcard(stackId, question, answer);
        Console.WriteLine("\nFlashcard created successfully! Press enter to continue");
        Console.ReadLine();
    }

    internal void DeleteFlashCardMenu(int stackId, List<Flashcard> list)
    {
        Console.Clear();

        if (list.Count <= 0)
        {
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            return;
        }

        ShowFlashcards(stackId);

        string prompt = "Type the Id of the flashcard you wish to delete or 0 to return to the menu";
        int validatedFlashcardId = Validation.GetValidatedId(prompt, 0, list.Count);

        int databaseFlashcardId = list[validatedFlashcardId - 1].StackId;

        _flashcardRepository.DeleteFlashcard(databaseFlashcardId);

        Console.WriteLine("\nFlashcard deleted successfully! Press enter to continue");
        Console.ReadLine();
    }

    internal void UpdateFlashCardMenu(int stackId, List<Flashcard> list)
    {
        Console.Clear();

        if (list.Count <= 0)
        {
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            return;
        }

        List<string> questions = list.Select(x => x.Question).ToList();
        ShowFlashcards(stackId);
        int validatedFlashcardId = 0;


        string prompt = "Type the Id of the flashcard you wish to update or 0 to return to the menu";
        validatedFlashcardId = Validation.GetValidatedId(prompt, 0, list.Count);

        string question = Validation.GetValidatedUniqueInput(
            "Type the flashcard's question or 0 to return to the menu:",
            questions,
            "\nThat question already exists. Please type a new one."
        );

        Console.WriteLine("Type the new answer for the flashcard or 0 to return to the menu");
        string answer = Console.ReadLine();
        if (answer == "0")
        {
            return;
        }

        int databaseFlashcardId = list[validatedFlashcardId - 1].StackId;

        _flashcardRepository.UpdateFlashcard(databaseFlashcardId, question, answer);
    }
}
