namespace Flashcards;

class StudySessionMenuView : BaseView
{
    private readonly StudySessionController controller;

    public StudySessionMenuView(StudySessionController controller)
    {
        this.controller = controller;
    }

    public override void Body()
    {
        Console.WriteLine("Study");
        Console.WriteLine("1 - Start New Session");
        Console.WriteLine("2 - View Session History");
        Console.WriteLine("0 - Return to Main Menu");
        Console.WriteLine("Enter one of the numbers above to select a menu option.");

        switch (Console.ReadKey().KeyChar.ToString())
        {
            case "1":
                controller.StartNewSession();
                break;
            case "2":
                controller.ShowSessionHistory();
                break;
            case "0":
                controller.BackToMainMenu();
                break;
            default:
                controller.ShowMenu();
                break;
        }
    }
}