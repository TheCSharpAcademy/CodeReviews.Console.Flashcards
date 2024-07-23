using Flashcards.ConsoleApp.Models;
using Flashcards.ConsoleApp.Services;
using Flashcards.Enums;
using Flashcards.Models;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.ConsoleApp.Views;

/// <summary>
/// Page which allows users to confgure the study session report.
/// </summary>
internal class ConfigureStudySessionReportPage : BasePage
{
    #region Constants

    private const string PageTitle = "Configure Study Session Report";

    #endregion
    #region Properties

    internal static IEnumerable<UserChoice> StudySessionReportTypeChoices
    {
        get
        {
            return
            [
                new(1, "Total"),
                new(2, "Average"),
                new(0, "Close page")
            ];
        }
    }

    #endregion
    #region Methods - Internal

    internal static StudySessionReportConfiguration? Show()
    {
        AnsiConsole.Clear();

        WriteHeader(PageTitle);

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<UserChoice>()
            .Title(PromptTitle)
            .AddChoices(StudySessionReportTypeChoices)
            .UseConverter(c => c.Name!)
            );

        if (choice.Id == 0)
        {
            // Close page.
            return null;
        }

        var reportType = choice.Id switch
        {
            1 => StudySessionReportType.Total,
            2 => StudySessionReportType.Average,
            _ => throw new NotImplementedException("Invalid StudySessionReportType")
        };
        
        DateTime? reportDate = GetReportDate();
        if (reportDate == null)
        {
            return null;
        }

        return new StudySessionReportConfiguration
        {
            Type = reportType,
            Date = reportDate.Value
        };
    }

    #endregion
    #region Methods - Private

    private static DateTime? GetReportDate()
    {
        var format = "yyyy";
        DateTime? date = UserInputService.GetDateTime(
            $"Enter the report date, format [blue]{format}[/], or [blue]0[/] to return to main menu: ",
            format,
            input => UserInputValidationService.IsValidReportDate(input, format)
        );

        return date == null ? null : date;
    }

    #endregion
}
