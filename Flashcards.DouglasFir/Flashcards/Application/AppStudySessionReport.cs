using Flashcards.Application.Helpers;
using Flashcards.DAO;
using Flashcards.Database;
using Flashcards.Enums;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.Application;

public class AppStudySessionReport
{
    private readonly StackDao _stackDao;
    private readonly FlashCardDao _flashCardDao;
    private readonly StudySessionDao _studySessionDao;
    private readonly InputHandler _inputHandler;
    private readonly ManageStacksHelper _manageStacksHelper;
    private readonly string _pageHeader = "Study Session";
    private bool _running;

    public AppStudySessionReport(DatabaseContext databaseContext, InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
        _stackDao = new StackDao(databaseContext);
        _flashCardDao = new FlashCardDao(databaseContext);
        _studySessionDao = new StudySessionDao(databaseContext);
        _manageStacksHelper = new ManageStacksHelper(_stackDao, _flashCardDao, _inputHandler);

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
            AnsiConsole.WriteLine("No data available for the selected year.");
            _inputHandler.PauseForContinueInput();
            return;
        }

        DisplayStudySessionReport(data);
    }

    private void DisplayStudySessionReport(IEnumerable<dynamic> data)
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
            string stackName = row.StackName;
            string jan = row.Jan?.ToString() ?? "0";
            string feb = row.Feb?.ToString() ?? "0";
            string mar = row.Mar?.ToString() ?? "0";
            string apr = row.Apr?.ToString() ?? "0";
            string may = row.May?.ToString() ?? "0";
            string jun = row.Jun?.ToString() ?? "0";
            string jul = row.Jul?.ToString() ?? "0";
            string aug = row.Aug?.ToString() ?? "0";
            string sep = row.Sep?.ToString() ?? "0";
            string oct = row.Oct?.ToString() ?? "0";
            string nov = row.Nov?.ToString() ?? "0";
            string dec = row.Dec?.ToString() ?? "0";

            table.AddRow(
                stackName,
                jan,
                feb,
                mar,
                apr,
                may,
                jun,
                jul,
                aug,
                sep,
                oct,
                nov,
                dec
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
            AnsiConsole.WriteLine("No data available for the selected year.");
            _inputHandler.PauseForContinueInput();
            return;
        }

        DisplayAverageScoreReport(data);

    }

    public void DisplayAverageScoreReport(IEnumerable<dynamic> data)
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
            string stackName = row.StackName;
            string jan = row.Jan?.ToString("N2") ?? "-";
            string feb = row.Feb?.ToString("N2") ?? "-";
            string mar = row.Mar?.ToString("N2") ?? "-";
            string apr = row.Apr?.ToString("N2") ?? "-";
            string may = row.May?.ToString("N2") ?? "-";
            string jun = row.Jun?.ToString("N2") ?? "-";
            string jul = row.Jul?.ToString("N2") ?? "-";
            string aug = row.Aug?.ToString("N2") ?? "-";
            string sep = row.Sep?.ToString("N2") ?? "-";
            string oct = row.Oct?.ToString("N2") ?? "-";
            string nov = row.Nov?.ToString("N2") ?? "-";
            string dec = row.Dec?.ToString("N2") ?? "-";

            table.AddRow(
               stackName,
                jan,
                feb,
                mar,
                apr,
                may,
                jun,
                jul,
                aug,
                sep,
                oct,
                nov,
                dec
            );
        }

        AnsiConsole.Write(table);
        Utilities.PrintNewLines(2);
        _inputHandler.PauseForContinueInput();
    }

}
