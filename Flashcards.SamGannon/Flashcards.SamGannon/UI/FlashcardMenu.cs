using DataAccess;
using DataAccess.Models;
using Flashcards.SamGannon.DTOs;
using Flashcards.SamGannon.Utilities;

namespace Flashcards.SamGannon.UI;

public class FlashcardMenu
{
    private readonly IMenu _MainMenu;

    public FlashcardMenu(IMenu mainMenu)
    {
        _MainMenu = mainMenu;
    }

    public IDataAccess DataAccess => _MainMenu.DataAccess;

    public void ShowMenu()
    {
        MenuMessages.ShowFlashcardMenu();
        string choice = ConsoleHelper.ReadValidInput(new[] { "1", "2", "3" });

        switch (choice)
        {
            case "1":
                CreateFlashcardSubMenu();
                break;
            case "2":
                DeleteFlashcards();
                break;
            case "3":
                 _MainMenu.ShowMenu();
                break;
            default:
                Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                break;
        }
    }

    private void DeleteFlashcards()
    {
        ConsoleHelper.Map(DataAccess, "Flashcard Menu");
        
        Console.WriteLine("Enter the stack name to see flashcards");
        string stackName = ConsoleHelper.ValidateStackName(DataAccess);
        List<Flashcard> rawFlashcards = DataAccess.GetAllFlashcards(stackName);
        List<FlashcardDto> flashcards = FlashcardDto.ToDto(rawFlashcards);

        TableVisualization.ShowTable(flashcards, "Delete Flashcard");
        Console.WriteLine("\n\nPlease enter the Id of the flashcard you wish to delete:");
        int flashcardId = ValidateFlashcardId();

        DataAccess.DeleteFlashcard(flashcardId, stackName);

        Console.ReadLine();

        ShowMenu();
    }

    private int ValidateFlashcardId()
    {
        int flashcardId;

        while (true)
        {
            Console.WriteLine("Enter Flashcard Id (or 'E' to exit): ");
            string strFlashcardId = Console.ReadLine();

            if (strFlashcardId.Equals("E", StringComparison.OrdinalIgnoreCase))
            {
                _MainMenu.ShowMenu();
            }
            else if (int.TryParse(strFlashcardId, out flashcardId))
            {
                Flashcard flashcard = DataAccess.GetFlashcardsById(flashcardId);

                if (flashcard != null)
                {
                    break; // Valid Flashcard Id, exit loop
                }
                else
                {
                    Console.WriteLine("Flashcard doesn't exist, please try again");
                }

            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid integer or 'E' to exit.");
            }
        }

        return flashcardId;
    }

    private void CreateFlashcardSubMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Create Flashcards ===");

        MenuMessages.FlashCardMessage();
        CreateFlashcard();
    }

    private void CreateFlashcard()
    {
        ConsoleHelper.Map(DataAccess, "Flashcard Menu");
        MenuMessages.FlashCardMessageReminder();
        Console.WriteLine("=== Create Flashcards ===");

        string stackName = ConsoleHelper.ValidateStackName(DataAccess);
        StackModel singleStack = DataAccess.GetStackByName(stackName); // retrieves a model of the data

        Console.WriteLine("Enter the question for the new flashcard:");
        string question = Console.ReadLine();

        Console.WriteLine("Enter the answer for the new flashcard:");
        string answer = Console.ReadLine();

        // Call DatabaseService method to add the new flashcard to the database
        DataAccess.AddFlashcard(question, answer, singleStack.StackId);

        Console.WriteLine("Flashcard created successfully! Press a key to continue");
        Console.ReadLine();
        ShowMenu();
    }

    private void EditFlashcards()
    {
        Console.Clear();
        Console.WriteLine("=== Edit Flashcards ===");

        MenuMessages.EditMessage();
        StackModel stack = GetStack();

        SelectFlashcardFromStack(stack);
    }

    private void SelectFlashcardFromStack(StackModel stack) 
    {
        Console.Clear();
        List<Flashcard> flashcards = DataAccess.GetFlashcardsByStackId(stack.StackId);
        TableVisualization.ShowTable(flashcards, "Flaschcard Menu");
    }

    private StackModel GetStack()
    {
        Console.Clear();
        List<StackModel> lstAllStacks = DataAccess.GetAllStacks();
        TableVisualization.ShowTable(lstAllStacks, "Flashcard Menu");

        var stackName = ConsoleHelper.ValidateStackName(DataAccess);

        StackModel stack = DataAccess.GetStackByName(stackName);

        return stack;
    }

}
