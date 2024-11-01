namespace Flashcards;

class ReportMenuView : BaseView
{
    private readonly ReportController controller;

    public ReportMenuView(ReportController controller)
    {
        this.controller = controller;
    }

    public override void Body()
    {
        Console.WriteLine("Reports");
        Console.WriteLine("1 - Number of sessions per month per stack");
        Console.WriteLine("2 - Average score per month per stack");
        Console.WriteLine("0 - Return to Main Menu");
        Console.WriteLine("Enter one of the numbers above to select a menu option.");

        switch (Console.ReadKey().KeyChar.ToString())
        {
            case "1":
                controller.ShowReport(ReportType.NumberOfSessionsPerMonthPerStack);
                break;
            case "2":
                controller.ShowReport(ReportType.AverageScorePerMonthPerStack);
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