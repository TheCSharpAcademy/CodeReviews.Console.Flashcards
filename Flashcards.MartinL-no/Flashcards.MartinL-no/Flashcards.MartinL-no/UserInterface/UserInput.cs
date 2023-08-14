namespace Flashcards.MartinL_no.UserInterface;

internal class UserInput
{
    private readonly StackManagerApplication _stackManagerApp;
    private readonly StudySessionApplication _sessionApp;

    public UserInput(StackManagerApplication stackManagerApp, StudySessionApplication sessionApp)
	{
        _stackManagerApp = stackManagerApp;
        _sessionApp = sessionApp;
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
                    _sessionApp.Study();
                    break;
                case "V":
                    _sessionApp.ViewStudySessionData();
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
}
