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

    public void Execute()
    {
        while (true)
        {
            ShowMainMenuOptions();
            var op = Ask("Your choice: ");

            switch (op.ToUpper())
            {
                case "S":
                    ManageStacks();
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
                    ShowMessage("Invalid option, please try again");
                    break;
            }
        }
    }

    private void ShowMainMenuOptions()
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
    }

    private void ManageStacks()
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
                StackMenu(stack);
                break;
            }
            else ShowMessage("Invalid input, please try again");
        }
    }

    private void StackMenu(FlashcardStackDTO stack)
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
