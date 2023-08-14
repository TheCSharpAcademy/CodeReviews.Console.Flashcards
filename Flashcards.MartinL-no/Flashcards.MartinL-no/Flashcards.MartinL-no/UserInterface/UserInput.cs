using Flashcards.MartinL_no.Controllers;

namespace Flashcards.MartinL_no.UserInterface;

internal class UserInput
{
    private readonly StackManagerApplication _stackManagerApp;

	public UserInput(StackManagerApplication stackManagerApp)
	{
        _stackManagerApp = stackManagerApp;
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
                    _stackManagerApp.ManageStacks();
                    break;
                case "F":
                    _stackManagerApp.ManageFlashcards();
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
