using Flashcards.MartinL_no.Controllers;

namespace Flashcards.MartinL_no.UserInterface;

internal class UserInput
{
    private readonly StackManager _stackManager;

	public UserInput(StackManager stackManager)
	{
        _stackManager = stackManager;
    }

    public void Menu()
    {
        while (true)
        {
            Console.Clear();
            Helpers.ShowLine();
            Console.WriteLine("""
                0 - exit
                S - Manage Stacks
                F - Manage Flashcards
                ST - Study
                V - View study session data
                """);
            Helpers.ShowLine();

            var op = Console.ReadLine();

            switch (op.ToUpper())
            {
                case "S":
                    _stackManager.ManageStacks();
                    break;
                case "F":
                    _stackManager.ManageFlashcards();
                    break;
                case "ST":
                    Study();
                    break;
                case "V":
                    ViewStudySessionData();
                    break;
                case "0":
                    Helpers.ShowMessage("Program ended");
                    return;
                default:
                    Helpers.ShowMessage("Invalid input, please try again");
                    break;
            }
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
}
