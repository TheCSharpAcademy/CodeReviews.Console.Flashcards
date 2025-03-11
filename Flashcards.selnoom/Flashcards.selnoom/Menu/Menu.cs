using Flashcards.selnoom.Data;
using Flashcards.selnoom.Models;
using Flashcards.selnoom.Helpers;

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
            //TODO add validation
            Console.Clear();
            Console.Write(
                "Please select an option:\n" +
                "1 - Create a stack\n" +
                "2 - View stacks\n" +
                "3 - Update a stack\n" +
                "4 - Delete a stack\n" +
                "0 - Exit program\n");

            string userInput = Console.ReadLine();

            switch (int.Parse(userInput))
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
                    //TODO include message or somethin
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

    private bool ShowStacks()
    {
        Console.Clear();
        Console.WriteLine("Stacks:\n");
        List<FlashcardStack> stacks = new();
        stacks = _stackRepository.GetStacks();
        if (stacks.Count > 0)
        {
            foreach (FlashcardStack stack in stacks)
            {
                Console.WriteLine($"{stack.StackName}");
            }
            return true;
        }
        else
        {
            Console.WriteLine("There are currently no stacks.\n");
            return false;
        }
    }

    private bool ShowFlashcards(int stackId)
    {
        List<Flashcard> flashcards = _flashcardRepository.GetFlashcardsByStack(stackId);
        if (flashcards.Count > 0)
        {
            Console.WriteLine("Flashcards:\n");
            foreach (Flashcard flashcard in flashcards)
            {
                Console.WriteLine($"Question:{flashcard.Question}\n");
            }
            return true;
        }
        else
        {
            Console.WriteLine("There are currently no flashcards in this stack.\n");
            return false;
        }
    }

    private void UpdateStackMenu()
    {
        //TODO add validation
        Console.Clear();
        Console.WriteLine(
            "Do you wish to alter the name of a stack or to modify its flashcards?\n" +
            "1 - Flashcards\n" +
            "2 - Stack name\n" +
            "0 - Return to menu");
        string userInput = Console.ReadLine();

        switch (int.Parse(userInput))
        {
            case 1:
                FlashcardMenu();
                break;
            case 2:
                UpdateStackName();
                break;
            default:
                //TODO include message or somethin
                return;
        }
    }

    internal void UpdateStackName()
    {
        //TODO add validations

        if (!ShowStacks())
        {
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Type the Id of the stack you wish to alter or 0 to return to the menu");
        string userInput = Console.ReadLine();
        if (userInput == "0")
        {
            return;
        }

        //TODO Validate if the Id actually exists

        Console.WriteLine("Type new stack name or 0 to return to the menu");
        string stackName = Console.ReadLine();
        if (stackName == "0")
        {
            return;
        }

        _stackRepository.UpdateStack(int.Parse(userInput), stackName);

        Console.WriteLine("\nStack updated! Press enter to continue");
        Console.ReadLine();
    }

    internal void DeleteStackMenu()
    {
        //TODO add validations

        if (!ShowStacks())
        {
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Type the Id of the stack you wish to delete or 0 to return to the menu");
        string userInput = Console.ReadLine();
        if (userInput == "0")
        {
            return;
        }

        //TODO Validate if the Id actually exists

        _stackRepository.DeleteStack(int.Parse(userInput));

        Console.WriteLine("\nStack deleted! Press enter to continue");
        Console.ReadLine();
    }

    internal void FlashcardMenu()
    {
        if (!ShowStacks())
        {
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Type the Id of the stack you wish to alter or 0 to return to the menu");
        string stackId = Console.ReadLine();
        if (stackId == "0")
        {
            return;
        }

        //TODO Validate if the Id actually exists

        ShowFlashcards(int.Parse(stackId));

        Console.WriteLine(
            "Select an option:\n" +
            "1 - Create a new flashcard\n" +
            "2 - Update a flashcard\n" +
            "3 - Delete a flashcard\n" +
            "0 - Return to menu");
        string userInput = Console.ReadLine();

        switch (int.Parse(userInput))
        {
            case 1:
                CreateFlashcardMenu(int.Parse(stackId));
                break;
            case 2:
                UpdateFlashCardMenu(int.Parse(stackId));
                break;
            case 3:
                DeleteFlashCardMenu(int.Parse(stackId));
                break;
            case 0:
                return;
            default:
                //TODO include message or somethin
                return;
        }
    }

    internal void CreateFlashcardMenu(int stackId)
    {
        //TODO add error handling incase question already exists
        Console.Clear();
        Console.WriteLine("Type the flashcards question or 0 to return to the menu:");
        string question = Console.ReadLine();
        if (question == "0")
        {
            return;
        }

        Console.Clear();
        Console.WriteLine("Type the flashcards answer or 0 to return to the menu:");
        string answer = Console.ReadLine();
        if (answer == "0")
        {
            return;
        }

        //TODO manually validate if question already exists in stack

        _flashcardRepository.CreateFlashcard(stackId, question, answer);
        Console.WriteLine("\nFlashcard created successfully! Press enter to continue");
        Console.ReadLine();
    }

    internal void DeleteFlashCardMenu(int stackId)
    {
        //TODO add validation
        Console.Clear();
        if(!ShowFlashcards(stackId))
        {
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Type the Id of the flashcard you wish to delete or 0 to return to the menu");
        string flashcardId = Console.ReadLine();
        if (flashcardId == "0")
        {
            return;
        }

        //TODO Validate if the Id actually exists

        _flashcardRepository.DeleteFlashcard(int.Parse(flashcardId));

        Console.WriteLine("\nFlashcard deleted successfully! Press enter to continue");
        Console.ReadLine();
    }

    internal void UpdateFlashCardMenu(int stackId)
    {
        //TODO add validation
        Console.Clear();
        if (!ShowFlashcards(stackId))
        {
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Type the Id of the flashcard you wish to delete or 0 to return to the menu");
        string flashcardId = Console.ReadLine();
        if (flashcardId == "0")
        {
            return;
        }

        //TODO Validate if the Id actually exists

        Console.WriteLine("Type the new question for the flashcard or 0 to return to the menu");
        string question = Console.ReadLine();
        if (question == "0")
        {
            return;
        }

        Console.WriteLine("Type the new answer for the flashcard or 0 to return to the menu");
        string answer = Console.ReadLine();
        if (answer == "0")
        {
            return;
        }

        _flashcardRepository.UpdateFlashcard(int.Parse(flashcardId), question, answer);
    }
}
