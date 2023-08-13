using Flashcards.MartinL_no.Controllers;
using Flashcards.MartinL_no.Models;

namespace Flashcards.MartinL_no.UserInterface;

internal class UserInput
{
    private readonly FlashcardController _controller;

	public UserInput(FlashcardController controller)
	{
        _controller = controller;
    }

    public void Menu()
    {
        while (true)
        {
            Console.Clear();
            ShowLine();
            Console.WriteLine("""
                0 - exit
                S - Manage Stacks
                F - Manage Flashcards
                ST - Study
                V - View study session data
                """);
            ShowLine();

            var op = Console.ReadLine();

            switch (op.ToUpper())
            {
                case "S":
                    ManageStacks();
                    break;
                case "F":
                    ManageFlashcards();
                    break;
                case "ST":
                    Study();
                    break;
                case "V":
                    ViewStudySessionData();
                    break;
                case "0":
                    ShowMessage("Program ended");
                    return;
                default:
                    ShowMessage("Invalid input, please try again");
                    break;
            }
        }
    }

    private void ManageStacks()
    {
        while (true)
        {

            Console.Clear();

            var stackNames = _controller.GetStackNames();
            TableVisualizationEngine.ShowTable(stackNames);

            ShowLine();
            Console.WriteLine("""
                0 to exit
                C to create Stack
                D to delete Stack
                """);
            ShowLine();

            var op = Console.ReadLine();

            switch (op.ToUpper())
            {
                case "0":
                    return;
                case "C":
                    CreateStack();
                    break;
                case "D":
                    DeleteStack();
                    break;
                default:
                    ShowMessage("Invalid input, please try again");
                    break;
            }
        }
    }

    private void CreateStack()
    {
        while (true)
        {
            Console.Clear();
            var name = Ask("Enter the name of the stack you would like to create (or 0 to exit): ");
            if (name == "0") return;

            if (String.IsNullOrWhiteSpace(name))
            {
                ShowMessage("You must enter a name, please try again");
            }
            else if (_controller.CreateStack(name))
            {
                ShowMessage("Stack added!");
            }
            else
            {
                ShowMessage("Stack already exists, please try again");
            }
        }
    }

    private void DeleteStack()
    {
        while (true)
        {
            Console.Clear();
            var name = Ask("Enter the name of the stack you would like to delete (or 0 to exit): ");
            if (name == "0") return;

            if (String.IsNullOrWhiteSpace(name))
            {
                ShowMessage("You must enter a name, please try again");
            }
            else if (_controller.DeleteStack(name))
            {
                ShowMessage("Stack deleted!");
                return;
            }
            else
            {
                ShowMessage("Stack does not exist, please try again");
            }
        }
    }

    private void ManageFlashcards()
    {
        var stackNames = _controller.GetStackNames();

        while (true)
        {
            Console.Clear();

            TableVisualizationEngine.ShowTable(stackNames);

            Console.WriteLine("""
                
                Choose a stack of flashcards to interact with:

                """);

            ShowLine();
            Console.WriteLine("""
                Input a current stack name
                or input 0 to exit input
                """);
            ShowLine();

            var stackName = Console.ReadLine();
            if (stackName == "0") break;

            var isValidStackName = stackNames.Exists(s => s.ToLower() == stackName.ToLower());
            if (isValidStackName)
            {
                StackMenu(stackName);
                break;
            }
            else ShowMessage("Invalid input, please try again");
        }
    }

    private void Study()
    {
        throw new NotImplementedException();
    }

    private void ViewStudySessionData()
    {
        throw new NotImplementedException();
    }

    private void StackMenu(string stackName)
    {
        while (true)
        {
            Console.Clear();

            ShowLine();
            Console.WriteLine($"""
                Current working stack: {stackName}

                0 to return to main menu
                X to change current stack
                V to view all Flashcards in stack
                A to view X amount of cards in stack
                C to create a Flashcard in current stack
                E to edit a Flashcard
                D to delete a Flashcard
                """);
            ShowLine();

            var op = Console.ReadLine();

            switch (op.ToUpper())
            {
                case "0":
                    return;
                case "X":
                    ManageFlashcards();
                    break;
                case "V":
                    ViewAllFlashCards(stackName);
                    break;
                case "A":
                    var amountString = Ask("How many Flashcards do you want to see: ");
                    int amount;
                    if (!Int32.TryParse(amountString, out amount)) goto default;
                    ViewFlashcards(stackName, amount);
                    break;
                case "C":
                    CreateFlashcard(stackName);
                    break;
                case "E":
                    EditFlashcard(stackName);
                    break;
                case "D":
                    DeleteFlashcard(stackName);
                    break;
                default:
                    ShowMessage("Invalid input, please try again");
                    break;
            }
        }
    }

    private void ViewAllFlashCards(string stackName)
    {
        Console.Clear();

        var stack = _controller.GetStackByName(stackName);
        TableVisualizationEngine.ShowTable(stack);

        Console.WriteLine();
        ShowLine();
        Ask("Press any key to return to menu: ");
        ShowLine();

    }

    private void ViewFlashcards(string stackName, int amount)
    {
        Console.Clear();

        var stack = _controller.GetStackByName(stackName);
        var flashcards = stack.Flashcards.Take(amount).ToList();
        stack.Flashcards = flashcards;
        TableVisualizationEngine.ShowTable(stack);

        Console.WriteLine();
        ShowLine();
        Ask("Press any key to return to menu: ");
        ShowLine();
    }

    private void CreateFlashcard(string stackName)
    {
        while (true)
        {
            Console.Clear();
            var front = Ask("What should the front be? ");
            var back = Ask("What should the back be? ");

            var stack = _controller.GetStackByName(stackName);
            var isAdded = _controller.CreateFlashcard(front, back, stack.Id);

            if (isAdded)
            {
                ShowMessage("Flashcard created!");
                break;
            }

            ShowMessage("Invalid entry, please try again");
        }
    }

    private void EditFlashcard(string stackName)
    {
        while (true)
        {
            Console.Clear();

            var stack = _controller.GetStackByName(stackName);
            TableVisualizationEngine.ShowTable(stack);

            var viewIdString = Ask("What is the id of the flashcard you want to edit: ");
            var front = Ask("What should the front be: ");
            var back = Ask("What should the back be: ");
            int viewId;

            if (Int32.TryParse(viewIdString, out viewId))
            {
                var id = stack.Flashcards[viewId - 1].Id;
                var isAdded = _controller.UpdateFlashcard(id, front, back, stack.Id);
                if (isAdded)
                {
                    ShowMessage("Flashcard updated!");
                    return;
                }
            }

            else ShowMessage("Invalid entry, please try again");
        }
    }

    private void DeleteFlashcard(string stackName)
    {
        while (true)
        {
            Console.Clear();

            var stack = _controller.GetStackByName(stackName);
            TableVisualizationEngine.ShowTable(stack);

            var viewIdString = Ask("What is the id of the flashcard you want to delete: ");
            int viewId;

            if (Int32.TryParse(viewIdString, out viewId))
            {
                var id = stack.Flashcards[viewId - 1].Id;
                var isAdded = _controller.DeleteFlashcard(id);
                if (isAdded)
                {
                    ShowMessage("Flashcard deleted!");
                    //return;
                }
            }

            else ShowMessage("Invalid entry, please try again");
        }
    }

    private void ShowMessage(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        Thread.Sleep(2500);
    }

    private void ShowLine()
    {
        Console.WriteLine("---------------------------------");
    }

    private string Ask(string message)
    {
        Console.Write(message);
        return Console.ReadLine();
    }
}
