using Flashcards.MartinL_no.Controllers;

namespace Flashcards.MartinL_no.UserInterface;

internal class StackManagerApplication
{
    private readonly StackManagerController _stackManagerController;

    public StackManagerApplication(StackManagerController stackManagerController)
	{
        _stackManagerController = stackManagerController;
    }

    public void ManageStacks()
    {
        while (true)
        {

            Console.Clear();

            var stackNames = _stackManagerController.GetStackNames();
            TableVisualizationEngine.ShowTable(stackNames);

            Helpers.ShowLine();
            Console.WriteLine("""
                0 to exit
                C to create Stack
                D to delete Stack
                """);
            Helpers.ShowLine();

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
                    Helpers.ShowMessage("Invalid input, please try again");
                    break;
            }
        }
    }

    private void CreateStack()
    {
        while (true)
        {
            Console.Clear();
            var name = Helpers.Ask("Enter the name of the stack you would like to create (or 0 to exit): ");
            if (name == "0") return;

            if (String.IsNullOrWhiteSpace(name))
            {
                Helpers.ShowMessage("You must enter a name, please try again");
            }
            else if (_stackManagerController.CreateStack(name))
            {
                Helpers.ShowMessage("Stack added!");
            }
            else
            {
                Helpers.ShowMessage("Stack already exists, please try again");
            }
        }
    }

    private void DeleteStack()
    {
        while (true)
        {
            Console.Clear();
            var name = Helpers.Ask("Enter the name of the stack you would like to delete (or 0 to exit): ");
            if (name == "0") return;

            if (String.IsNullOrWhiteSpace(name))
            {
                Helpers.ShowMessage("You must enter a name, please try again");
            }
            else if (_stackManagerController.DeleteStack(name))
            {
                Helpers.ShowMessage("Stack deleted!");
                return;
            }
            else
            {
                Helpers.ShowMessage("Stack does not exist, please try again");
            }
        }
    }

    public void ManageFlashcards()
    {
        var stackNames = _stackManagerController.GetStackNames();

        while (true)
        {
            Console.Clear();

            TableVisualizationEngine.ShowTable(stackNames);

            Console.WriteLine("""
                Choose a stack of flashcards to interact with:

                """);

            Helpers.ShowLine();
            Console.WriteLine("""
                Input a current stack name
                or input 0 to exit input
                """);
            Helpers.ShowLine();

            var stackName = Console.ReadLine();
            if (stackName == "0") break;

            var isValidStackName = stackNames.Exists(s => s.ToLower() == stackName.ToLower());
            if (isValidStackName)
            {
                StackMenu(stackName);
                break;
            }
            else Helpers.ShowMessage("Invalid input, please try again");
        }
    }

    private void StackMenu(string stackName)
    {
        while (true)
        {
            Console.Clear();

            Helpers.ShowLine();
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
            Helpers.ShowLine();

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
                    var amountString = Helpers.Ask("How many Flashcards do you want to see: ");
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
                    Helpers.ShowMessage("Invalid input, please try again");
                    break;
            }
        }
    }

    private void ViewAllFlashCards(string stackName)
    {
        Console.Clear();

        var stack = _stackManagerController.GetStackByName(stackName);
        TableVisualizationEngine.ShowTable(stack);

        Helpers.ShowLine();
        Helpers.Ask("Press any key to return to menu: ");
        Helpers.ShowLine();

    }

    private void ViewFlashcards(string stackName, int amount)
    {
        Console.Clear();

        var stack = _stackManagerController.GetStackByName(stackName);
        var flashcards = stack.Flashcards.Take(amount).ToList();
        stack.Flashcards = flashcards;
        TableVisualizationEngine.ShowTable(stack);

        Helpers.ShowLine();
        Helpers.Ask("Press any key to return to menu: ");
        Helpers.ShowLine();
    }

    private void CreateFlashcard(string stackName)
    {
        while (true)
        {
            Console.Clear();
            var front = Helpers.Ask("What should the front be? ");
            var back = Helpers.Ask("What should the back be? ");

            var stack = _stackManagerController.GetStackByName(stackName);
            var isAdded = _stackManagerController.CreateFlashcard(front, back, stack.Id);

            if (isAdded)
            {
                Helpers.ShowMessage("Flashcard created!");
                break;
            }

            Helpers.ShowMessage("Invalid entry, please try again");
        }
    }

    private void EditFlashcard(string stackName)
    {
        while (true)
        {
            Console.Clear();

            var stack = _stackManagerController.GetStackByName(stackName);
            TableVisualizationEngine.ShowTable(stack);

            var viewIdString = Helpers.Ask("What is the id of the flashcard you want to edit: ");
            var front = Helpers.Ask("What should the front be: ");
            var back = Helpers.Ask("What should the back be: ");
            int viewId;

            if (Int32.TryParse(viewIdString, out viewId))
            {
                var id = stack.Flashcards[viewId - 1].Id;
                var isAdded = _stackManagerController.UpdateFlashcard(id, front, back, stack.Id);
                if (isAdded)
                {
                    Helpers.ShowMessage("Flashcard updated!");
                    return;
                }
            }

            else Helpers.ShowMessage("Invalid entry, please try again");
        }
    }

    private void DeleteFlashcard(string stackName)
    {
        while (true)
        {
            Console.Clear();

            var stack = _stackManagerController.GetStackByName(stackName);
            TableVisualizationEngine.ShowTable(stack);

            var viewIdString = Helpers.Ask("What is the id of the flashcard you want to delete: ");
            int viewId;

            if (Int32.TryParse(viewIdString, out viewId))
            {
                var id = stack.Flashcards[viewId - 1].Id;
                var isAdded = _stackManagerController.DeleteFlashcard(id);
                if (isAdded)
                {
                    Helpers.ShowMessage("Flashcard deleted!");
                    return;
                }
            }

            else Helpers.ShowMessage("Invalid entry, please try again");
        }
    }
}

