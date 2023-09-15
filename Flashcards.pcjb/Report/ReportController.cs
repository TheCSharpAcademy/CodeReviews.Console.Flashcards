namespace Flashcards;

class ReportController
{
    private readonly Database database;
    private MainMenuController? mainMenuController;

    public ReportController(Database database)
    {
        this.database = database;
    }

    public void SetMainMenuController(MainMenuController controller)
    {
        mainMenuController = controller;
    }

    public void ShowMenu()
    {
        var view = new ReportMenuView(this);
        view.Show();
    }

    public void ShowReport(ReportType reportType)
    {
        ShowReport(reportType, null);
    }

    public void ShowReport(ReportType reportType, int? year)
    {
        if (year == null)
        {
            ShowReportConfig(reportType);
            return;
        }
        var reportData = database.Report(reportType, year);
        var view = new ReportView(this, reportType, year, reportData);
        view.Show();
    }

    public void ShowReportConfig(ReportType reportType)
    {
        var view = new ReportConfigView(this, reportType);
        view.Show();
        return;
    }

    public void BackToMainMenu()
    {
        BackToMainMenu(null);
    }

    public void BackToMainMenu(string? message)
    {
        if (mainMenuController == null)
        {
            throw new InvalidOperationException("Required MainMenuController missing.");
        }
        mainMenuController.ShowMainMenu(message);
    }
}