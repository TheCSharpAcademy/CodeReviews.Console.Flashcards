namespace Flashcards;

class ReportConfigView : BaseView
{
    private readonly ReportController controller;
    private readonly ReportType reportType;

    public ReportConfigView(ReportController controller, ReportType reportType)
    {
        this.controller = controller;
        this.reportType = reportType;
    }

    public override void Body()
    {
        Console.WriteLine("Report Configuration");
        Console.Write("Report Year: ");
        var rawInput = Console.ReadLine();
        if (String.IsNullOrEmpty(rawInput))
        {
            controller.ShowMenu();
        }
        else if (int.TryParse(rawInput, out int year))
        {
            if (year < 1000 || year > (int) DateTime.Now.Year)
            {
                controller.ShowReport(reportType);
            }
            else
            {
                controller.ShowReport(reportType, year);
            }
        }
        else
        {
            controller.ShowMenu();
        }
    }
}