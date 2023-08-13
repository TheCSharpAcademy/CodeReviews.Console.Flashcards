using Flashcards.MartinL_no.Controllers;

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

    private void ManageStacks()
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

    private void ShowMainMenuOptions()
    {
        ShowLine();

        Console.WriteLine("""

            Select an option:
                0 - exit
                S - Manage Stacks
                F - Manage Flashcards
                ST - Study
                V - View study session data

            """);

        ShowLine();
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
