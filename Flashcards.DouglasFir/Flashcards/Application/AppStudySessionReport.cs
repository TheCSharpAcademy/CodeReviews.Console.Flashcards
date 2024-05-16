
using Flashcards.DAO;
using Flashcards.Database;
using Flashcards.Enums;
using Flashcards.Models;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.Application;

public class AppStudySessionReport
{
    private readonly StudySessionDao _studySessionDao;
    private readonly InputHandler _inputHandler;
    private readonly string _pageHeader = "Study Session";
    private bool _running;

    public AppStudySessionReport(DatabaseContext databaseContext, InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
        _studySessionDao = new StudySessionDao(databaseContext);
        _running = true;
    }

    public void Run()
    {
        while (_running)
        {
            AnsiConsole.Clear();
            Utilities.DisplayPageHeader(_pageHeader);
            PromptForSessionAction();
        }
    }

    private void PromptForSessionAction()
    {
        ViewStudySessionDataMenuOption selectedOption = _inputHandler.PromptMenuSelection<ViewStudySessionDataMenuOption>();
        ExecuteSelectedOption(selectedOption);
    }

    private void ExecuteSelectedOption(ViewStudySessionDataMenuOption option)
    {
        switch (option)
        {
            case ViewStudySessionDataMenuOption.Cancel:
                CloseSession();
                break;
            case ViewStudySessionDataMenuOption.ViewNumberOfSessionsReport:
                HandleNumberOfSessionsReport();
                break;
            case ViewStudySessionDataMenuOption.ViewAverageScoreReport:
                HandleAverageScoreReport();
                break;
        }
    }

    private void CloseSession()
    {
        _running = false;
    }

    private void HandleNumberOfSessionsReport()
    {
        AnsiConsole.Clear();
        int year = _inputHandler.PromptForPositiveInteger("Please enter a year to view study sessions for:");
        var data = _studySessionDao.GetStudySessionReportData(year);
        if (data == null || !data.Any())
        {
            Utilities.DisplayInformationConsoleMessage("No data available for the selected year.");
            _inputHandler.PauseForContinueInput();
            return;
        }

        DisplayStudySessionReport(data);
    }

    private void DisplayStudySessionReport(IEnumerable<ReportMonthlySessionCount> data)
    {
        AnsiConsole.Clear();
        Utilities.DisplayPageHeader("Study Session Report");
        var table = new Table();
        table.AddColumn("Stack Name");
        table.AddColumn("Jan");
        table.AddColumn("Feb");
        table.AddColumn("Mar");
        table.AddColumn("Apr");
        table.AddColumn("May");
        table.AddColumn("Jun");
        table.AddColumn("Jul");
        table.AddColumn("Aug");
        table.AddColumn("Sep");
        table.AddColumn("Oct");
        table.AddColumn("Nov");
        table.AddColumn("Dec");

        foreach (var row in data)
        {
            table.AddRow(
                row.StackName!,
                row.Jan.ToString(),
                row.Feb.ToString(),
                row.Mar.ToString(),
                row.Apr.ToString(),
                row.May.ToString(),
                row.Jun.ToString(),
                row.Jul.ToString(),
                row.Aug.ToString(),
                row.Sep.ToString(),
                row.Oct.ToString(),
                row.Nov.ToString(),
                row.Dec.ToString()
            );
        }

        AnsiConsole.Write(table);
        Utilities.PrintNewLines(2);
        _inputHandler.PauseForContinueInput();
    }

    private void HandleAverageScoreReport()
    {
        AnsiConsole.Clear();
        int year = _inputHandler.PromptForPositiveInteger("Please enter a year to view study sessions for:");
        var data = _studySessionDao.GetAverageScoreReportData(year);
        if (data == null || !data.Any())
        {
            Utilities.DisplayInformationConsoleMessage("No data available for the selected year.");
            _inputHandler.PauseForContinueInput();
            return;
        }

        DisplayAverageScoreReport(data);

    }

    public void DisplayAverageScoreReport(IEnumerable<ReportAverageSessionScore> data)
    {
        var table = new Table();
        table.AddColumn("Stack Name");
        table.AddColumn("Jan");
        table.AddColumn("Feb");
        table.AddColumn("Mar");
        table.AddColumn("Apr");
        table.AddColumn("May");
        table.AddColumn("Jun");
        table.AddColumn("Jul");
        table.AddColumn("Aug");
        table.AddColumn("Sep");
        table.AddColumn("Oct");
        table.AddColumn("Nov");
        table.AddColumn("Dec");

        foreach (var row in data)
        {
            table.AddRow(
                row.StackName!,
                row.Jan.ToString("N2"),
                row.Feb.ToString("N2"),
                row.Mar.ToString("N2"),
                row.Apr.ToString("N2"),
                row.May.ToString("N2"),
                row.Jun.ToString("N2"),
                row.Jul.ToString("N2"),
                row.Aug.ToString("N2"),
                row.Sep.ToString("N2"),
                row.Oct.ToString("N2"),
                row.Nov.ToString("N2"),
                row.Dec.ToString("N2")
            );
        }

        AnsiConsole.Write(table);
        Utilities.PrintNewLines(2);
        _inputHandler.PauseForContinueInput();
    }

}
