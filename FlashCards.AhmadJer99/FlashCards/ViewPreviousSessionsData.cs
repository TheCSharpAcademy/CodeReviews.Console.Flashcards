using FlashCards.Data;
using FlashCards.Dtos;
using FlashCards.Mappers;
using FlashCards.Managers;
using Spectre.Console;
using FlashCards.Models;
using System.Globalization;

namespace FlashCards;
public class ViewPreviousSessionsData
{
    public enum ReportTypes
    {
        ViewAllData,
        YearlyViewStackMonthlyData
    }

    internal static void ChooseReport()
    {
        var reportType = AnsiConsole.Prompt(
            new SelectionPrompt<ReportTypes>()
            .Title("Choose the type of report you want to see: ")
            .AddChoices(Enum.GetValues<ReportTypes>()));

        switch (reportType)
        {
            case ReportTypes.ViewAllData:
                ViewAllData();
                AnsiConsole.MarkupLine("(Press Any Key To Continue)");
                Console.ReadKey();
                break;
            case ReportTypes.YearlyViewStackMonthlyData:
                var userYearInput = ValidYearInput();

                ViewYearlyStackData(userYearInput);
                AnsiConsole.MarkupLine("(Press Any Key To Continue)");
                Console.ReadKey();
                break;
        }
    }

    private static string ValidYearInput()
    {
        bool validYearEntry = false;
        CultureInfo machineCulture = CultureInfo.InvariantCulture;
        string userYearInput;
        string yearFormat = "yyyy";
        do
        {
            userYearInput = AnsiConsole.Ask<string>("Enter a year to for the report in this format YYYY: ");
            if (DateTime.TryParseExact(userYearInput, yearFormat,  machineCulture,DateTimeStyles.None , out DateTime _))
            { 
                validYearEntry = true;
                continue;
            }
            AnsiConsole.MarkupLine("[red]Please make sure to enter the year in the correct format (YYYY)[/]");

        }
        while (!validYearEntry);

        return userYearInput;
    }

    internal static void ViewAllData()
    {
        StudyDBController studyDBController = new();
        var studySessions = studyDBController.ReadAllRows();
        var studySessionsDtos = studySessions.Select(
            s => s.ToStudySessionDto())
            .ToList();

        List<string> columnNames = ["session_date", "score"];

        TableVisualisationEngine<StudySessionDto>.ViewAsTable(studySessionsDtos, ConsoleTableExt.TableAligntment.Left, columnNames);

    }
    internal static void ViewYearlyStackData(string year)
    {
        StacksManager stacksManager = new StacksManager();
        int stackId = stacksManager.ChooseStackMenu();
        if (stackId == -1)
            return;
        StudyDBController studyDBController = new();
        var yearlyReports = studyDBController.ReadYearlyStackData(stackId, year);

        Console.Clear();
        List<string> columnNames = ["Stack Name", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December",];

        TableVisualisationEngine<YearlyReport>.ViewAsTable(yearlyReports, ConsoleTableExt.TableAligntment.Left, columnNames, $"Avg score per month for : {year}");
    }

}