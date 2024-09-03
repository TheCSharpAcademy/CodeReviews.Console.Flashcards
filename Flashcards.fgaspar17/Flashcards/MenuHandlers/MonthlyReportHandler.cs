using Flashcards.MenuEnums;
using FlashcardsLibrary;

namespace Flashcards;
public class MonthlyReportHandler
{
    public void Display()
    {
        MenuPresentation.MenuDisplayer<MonthlyReportOptions>(() => "[purple]Monthly Report[/]", HandleMenuOptions);
    }

    private bool HandleMenuOptions(MonthlyReportOptions option)
    {
        int? userYear;
        switch (option)
        {
            case MonthlyReportOptions.Back:
                return false;
            case MonthlyReportOptions.SessionsPerMonth:
                userYear = GetYearFromUser();
                if (userYear.HasValue)
                    ShowMonthlyReportSessions(userYear.Value);
                break;
            case MonthlyReportOptions.AveragePerMonth:
                userYear = GetYearFromUser();
                if (userYear.HasValue)
                    ShowMonthlyReportAverageScore(userYear.Value);
                break;
            default:
                Console.WriteLine($"Option: {option} not valid");
                break;
        }

        return true;
    }

    private void ShowMonthlyReportSessions(int year)
    {
        var report = MonthlyReportController.GetSessionsPerMonthReport(year);
        OutputRenderer.ShowTable(report, "Sessions Per Month");

        Prompter.PressKeyToContinuePrompt();
    }

    private void ShowMonthlyReportAverageScore(int year)
    {
        var report = MonthlyReportController.GetAverageScorePerMonthReport(year);
        OutputRenderer.ShowTable(report, "Average Score Per Month");

        Prompter.PressKeyToContinuePrompt();
    }

    private int? GetYearFromUser()
    {
        string format = "yyyy";
        string message = $"Introduce a year({format})";
        IValidator[] validators = { new DateTimeValidator { Format = "yyyy" } };

        string userYear = Prompter.ValidatedTextPrompt(message, DateTime.Today.Year.ToString(), validators);
        if (CancelSetup.IsCancelled(userYear)) return null;

        return Convert.ToInt32(userYear);
    }
}