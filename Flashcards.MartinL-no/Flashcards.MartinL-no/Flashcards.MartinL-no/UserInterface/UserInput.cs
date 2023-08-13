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
            var stacks = _controller.GetStacks();

            Console.Clear();
            // Table output to be added

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
        throw new NotImplementedException();
    }

    private void DeleteStack()
    {
        throw new NotImplementedException();
    }

    private void ManageFlashcards()
    {
        var stacks = _controller.GetStacks();

        while (true)
        {
            Console.Clear();
            // Table output to be added

            Console.WriteLine("Choose a sack of flashcards to interact with: ");

            ShowLine();
            Console.WriteLine("""
                Input a current stack name
                or input 0 to exit input
                """);
            ShowLine();

            var stackName = Console.ReadLine();
            if (stackName == "0") break;

            var stack = stacks.FirstOrDefault(s => s.Name.ToLower() == stackName.ToLower());
            if (stack != null)
            {
                StackMenu(stack.Name);
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
            var stack = _controller.GetStackByName(stackName);

            Console.Clear();

            ShowLine();
            Console.WriteLine($"""
                Current working stack: {stack.Name}

                0 to return to main menu
                X to change current stack
                V to view all Flashcards in sack
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
                    ViewAllFlashCards(stack);
                    break;
                case "A":
                    var amountString = Ask("How many Flashcards do you want to see?");
                    int amount;
                    if (!Int32.TryParse(amountString, out amount)) goto default;
                    ViewFlashcards(stack, amount);
                    break;
                case "C":
                    CreateFlashcard(stack);
                    break;
                case "E":
                    EditFlashcard(stack);
                    break;
                case "D":
                    DeleteFlashcard(stack);
                    break;
                default:
                    ShowMessage("Invalid input, please try again");
                    break;
            }
        }
    }

    private void ViewAllFlashCards(FlashcardStackDTO stack)
    {
        throw new NotImplementedException();
    }

    private void ViewFlashcards(FlashcardStackDTO stack, int amount)
    {
        throw new NotImplementedException();
    }

    private void CreateFlashcard(FlashcardStackDTO stack)
    {
        while (true)
        {
            Console.Clear();
            var front = Ask("What should the front be? ");
            var back = Ask("What should the back be? ");

            var isAdded = _controller.CreateFlashcard(front, back, stack.Id);

            if (isAdded) break;

            ShowMessage("Invalid entry, please try again");
        }
    }

    private void EditFlashcard(FlashcardStackDTO stack)
    {
        throw new NotImplementedException();
    }

    private void DeleteFlashcard(FlashcardStackDTO stack)
    {
        throw new NotImplementedException();
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
