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
                    ManageStacksMenu();
                    break;
                case "F":
                    ManageFlashCards();
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

    private void ManageStacksMenu()
    {
        var stacks = _controller.GetStacks();

        while (true)
        {
            Console.Clear();
            // Table output to be added

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
                    ManageStacksMenu();
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

    private void DeleteFlashcard(FlashcardStackDTO stack)
    {
        throw new NotImplementedException();
    }

    private void EditFlashcard(FlashcardStackDTO stack)
    {
        throw new NotImplementedException();
    }

    private void CreateFlashcard(FlashcardStackDTO stack)
    {
        throw new NotImplementedException();
    }

    private void ViewFlashcards(FlashcardStackDTO stack, int amount)
    {
        throw new NotImplementedException();
    }

    private void ViewAllFlashCards(FlashcardStackDTO stack)
    {
        throw new NotImplementedException();
    }

    private void ManageFlashCards()
    {
        throw new NotImplementedException();
    }

    private void Study()
    {
        throw new NotImplementedException();
    }

    private void ViewStudySessionData()
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
