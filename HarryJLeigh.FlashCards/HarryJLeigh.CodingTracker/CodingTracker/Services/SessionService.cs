using CodingTracker.Controllers;
using CodingTracker.Data;
using CodingTracker.Helpers;
using CodingTracker.Models;
using CodingTracker.Views;
using Spectre.Console;

namespace CodingTracker.Services;

public class SessionService
{
    private readonly CrudController _sqlController = new CrudController(new DataBaseService());

    public void ViewAllSessions()
    {
        DisplayAllSessions();
    }

    public void ViewSessionsWithFilter()
    {
        var filterChoice = AnsiConsole.Prompt(
            new SelectionPrompt<FilterOptions>()
                .Title("Filter by:")
                .AddChoices(Enum.GetValues<FilterOptions>())
        );
        FilterSessions(filterChoice);
    }

    public void ViewSession(int id)
    {
        List<CodingSession> session = _sqlController.GetSession(id);
        AnsiConsole.MarkupLine($"[bold yellow]Current editing session:[/]");
        DisplaySessions(session);
    }

    public void InsertSession()
    {
        DateTime startTime = Utilities.GetDatetime("Start Time");
        DateTime endTime = Utilities.GetDatetime("End Time");

        while (!DateValidator.IsValidEndDate(startTime, endTime))
        {
            endTime = Utilities.GetDatetime("End Time");
        }

        if (!Utilities.PromptReturnToMenu()) return;
        string duration = Utilities.CalculateDuration(startTime, endTime);
        _sqlController.Insert(startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"),
            duration);
    }
    
    public void UpdateSession()
    {
        int id = Utilities.GetId(true);
        var updateChoice = AnsiConsole.Prompt(
            new SelectionPrompt<UpdateSessionOptions>()
                .Title("[yellow]What would you like to update[/]")
                .AddChoices(Enum.GetValues<UpdateSessionOptions>())
        );

        ViewSession(id);
        switch (updateChoice)
        {
            case UpdateSessionOptions.StartTime:
                UpdateStartTime(id);
                break;
            case UpdateSessionOptions.EndTime:
                UpdateEndTime(id);
                break;
            case UpdateSessionOptions.Both:
                UpdateBothTimes(id);
                break;
        }
    }

    public void DeleteSession()
    {
        List<CodingSession> sessions = _sqlController.GetAllSessions();
        if (!Utilities.CheckSessionsExist(sessions)) return;

        int id = Utilities.GetId(true);
        if (!Utilities.PromptReturnToMenu()) return;
        _sqlController.Delete(id);
        AnsiConsole.MarkupLine($"[bold yellow]Deleted session with id: {id}[/]");
        Utilities.AskUserToContinue();
    }
    
    public void GenerateReport()
    {
        DateTime? startDate = null;
        string? useFilter = null;
        string? orderDirection = null;
        FilterOptions? filterChoice = null;

        var reportChoice = AnsiConsole.Prompt(
            new SelectionPrompt<ReportOptions>()
                .Title("[yellow]Select a report to generate.[/]")
                .AddChoices(Enum.GetValues<ReportOptions>())
        );

        useFilter = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Would you like to filter the report? [/][cyan](yes/no):[/]")
                .PromptStyle("green")
                .Validate(input =>
                {
                    return input.ToLower() == "yes" || input.ToLower() == "no"
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Please enter 'yes' or 'no'.[/]");
                })
        );

        if (useFilter == "yes")
        {
            int filterValue;
            string prompt;
            filterChoice = AnsiConsole.Prompt(
                new SelectionPrompt<FilterOptions>()
                    .Title("Filter by:")
                    .AddChoices(Enum.GetValues<FilterOptions>())
            );

            switch (filterChoice)
            {
                case FilterOptions.Days:
                    prompt = "Enter an integer for amount of days to filter by";
                    filterValue = Utilities.AskForInt(prompt);
                    break;
                case FilterOptions.Weeks:
                    prompt = "Enter an integer for amount of weeks to filter by";
                    filterValue = Utilities.AskForInt(prompt) * 7; // Convert weeks to days
                    break;
                case FilterOptions.Years:
                    prompt = "Enter an integer for amount of years to filter by";
                    filterValue = Utilities.AskForInt(prompt) * 365; // Approximate years to days
                    break;
                default:
                    return; // Should not reach here
            }
            startDate = DateTime.Now.AddDays(-filterValue);
        }

        switch (reportChoice)
        {
            case ReportOptions.TotalSessions:
                DisplayTotalSessionsInfo(filterChoice, startDate);
                break;
            case ReportOptions.AverageHours:
                DisplayAverageHoursInfo(filterChoice, startDate);
                break;
        }
    }

    private void DisplayTotalSessionsInfo(FilterOptions? filterChoice, DateTime? startDate)
    {
        int sessionsCount = _sqlController.GetTotalSessionReport(filterChoice, startDate);
        if (startDate == null)
        {
            AnsiConsole.MarkupLine($"[green]Total number of sessions:[/][cyan]{sessionsCount}[/]");
            Utilities.AskUserToContinue();
        }
        else
        {
            AnsiConsole.MarkupLine($"[green]Total number of sessions after {startDate}:[/] [cyan]{sessionsCount}[/]");
            Utilities.AskUserToContinue();
        }
    }

    private void DisplayAverageHoursInfo(FilterOptions? filterChoice, DateTime? startDate)
    {
        double averageHours = _sqlController.GetAverageHoursReport(filterChoice, startDate);
        string formattedAvgHours = averageHours.ToString("F2");
        if (startDate == null)
        {
            AnsiConsole.MarkupLine($"[green]Average hours of total sessions:[/] [cyan]{formattedAvgHours}[/]");
            Utilities.AskUserToContinue();
        }
        else
        {
            AnsiConsole.MarkupLine($"[green]Average hours per sessions after {startDate}:[/] [cyan]{formattedAvgHours}[/]");
            Utilities.AskUserToContinue();
        }
    }

    public void StartStopwatch()
    {
        string? input = null;
        DateTime stopwatchStartTime = StopwatchService.StartStopwatch();

        while (input == null || input != "0")
        {
            input = AnsiConsole.Ask<string>("[bold green]Type[/] [yellow]'0'[/] [green]to stop the stopwatch coding session.[/]");
        }

        DateTime stopwatchEndTime = StopwatchService.StopStopwatch();
        string stopwatchDuration = Utilities.CalculateDuration(stopwatchStartTime, stopwatchEndTime);
        List<string> stopwatchSession = new List<string>();
        stopwatchSession.Add(stopwatchStartTime.ToString("yyyy-MM-dd HH:mm"));
        stopwatchSession.Add(stopwatchEndTime.ToString("yyyy-MM-dd HH:mm"));
        stopwatchSession.Add(stopwatchDuration);
        InsertStopwatchSession(stopwatchSession);
    }

    private void InsertStopwatchSession(List<string> stopwatchSession)
    {
        if (!Utilities.PromptReturnToMenu()) return;
        _sqlController.Insert(stopwatchSession[0], stopwatchSession[1], stopwatchSession[2]);
        List<CodingSession> lastInsertedSession = _sqlController.GetLastSession();

        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]Inserted stopwatch session:[/]");
        DisplaySessions(lastInsertedSession);
        Utilities.AskUserToContinue();
    }

    private void UpdateSessionTime(int sessionId, bool updateStartTime, bool updateEndTime)
    {
        DateTime startTime = updateStartTime ? Utilities.GetDatetime("Start Time") : GetStartTime(sessionId);
        DateTime EndTimeValid = GetEndTime(sessionId);
        if (updateEndTime)
        {
            EndTimeValid = Utilities.GetDatetime("End Time");
            while (!DateValidator.IsValidEndDate(_sqlController.GetSession(sessionId)[0].ParsedStartedTime,
                       EndTimeValid))
            {
                EndTimeValid = Utilities.GetDatetime("End Time");
            } ;
        }
        
        DateTime endTime = updateEndTime ? EndTimeValid : GetEndTime(sessionId);
        if (!Utilities.PromptReturnToMenu()) return;
        if (updateStartTime)
        {
            string previousStartTime = GetStartTime(sessionId).ToString("yyyy-MM-dd HH:mm:ss");
            _sqlController.UpdateStartTime(sessionId, startTime.ToString("yyyy-MM-dd HH:mm:ss"));
            Utilities.DisplayUpdateInfo(sessionId, startTime.ToString("yyyy-MM-dd HH:mm:ss"), previousStartTime,
                "Start Time");
        }
        if (updateEndTime)
        {
            string previousEndTime = GetEndTime(sessionId).ToString("yyyy-MM-dd HH:mm:ss");
            _sqlController.UpdateEndTime(sessionId, endTime.ToString("yyyy-MM-dd HH:mm:ss"));
            Utilities.DisplayUpdateInfo(sessionId, endTime.ToString("yyyy-MM-dd HH:mm:ss"), previousEndTime,
                "End Time");
        }
        UpdateDuration(sessionId, startTime, endTime);
    }

    private void UpdateStartTime(int id)
    {
        UpdateSessionTime(id, true, false);
    }

    private void UpdateEndTime(int id)
    {
        UpdateSessionTime(id, false, true);
    }

    private void UpdateBothTimes(int id)
    {
        UpdateSessionTime(id, true, true);
    }

    private void UpdateDuration(int id, DateTime startTime, DateTime endTime)
    {
        string duration = Utilities.CalculateDuration(startTime, endTime);
        _sqlController.UpdateDuration(id, duration);
        AnsiConsole.MarkupLine($"[cyan]New Duration:[/] [cyan]{duration}[/]");
        Utilities.AskUserToContinue();
    }

    public List<int> GetAllSessionIds()
    {
        List<int> sessionIds = new List<int>();
        List<CodingSession> sessions = _sqlController.GetAllSessions();
        foreach (CodingSession session in sessions)
        {
            sessionIds.Add(session.Id);
        }
        return sessionIds;
    }

    private DateTime GetStartTime(int id)
    {
        List<CodingSession> session = _sqlController.GetSession(id);
        return session[0].ParsedStartedTime;
    }

    private DateTime GetEndTime(int id)
    {
        List<CodingSession> session = _sqlController.GetSession(id);
        return session[0].ParsedEndTime;
    }

    private void FilterSessions(FilterOptions filterOption)
    {
        int filterValue;
        string prompt, orderDirection;

        switch (filterOption)
        {
            case FilterOptions.Days:
                prompt = "Enter an integer for amount of days to filter by";
                filterValue = Utilities.AskForInt(prompt);
                orderDirection = Utilities.AskForOrderInput();
                break;
            case FilterOptions.Weeks:
                prompt = "Enter an integer for amount of weeks to filter by";
                filterValue = Utilities.AskForInt(prompt) * 7; // Convert weeks to days
                orderDirection = Utilities.AskForOrderInput();
                break;
            case FilterOptions.Years:
                prompt = "Enter an integer for amount of years to filter by";
                filterValue = Utilities.AskForInt(prompt) * 365; // Approximate years to days
                orderDirection = Utilities.AskForOrderInput();
                break;
            default:
                return; // Should not reach here
        }

        DateTime startDate = DateTime.Now.AddDays(-filterValue);
        List<CodingSession> sessions = _sqlController.GetSessionsByFilter(startDate, orderDirection);
        if (!Utilities.PromptReturnToMenu()) return;
        DisplaySessions(sessions);
        Utilities.AskUserToContinue();
    }

    private void DisplayAllSessions()
    {
        List<CodingSession> sessions = _sqlController.GetAllSessions();
        DisplaySessions(sessions);
    }

    private void DisplaySessions(List<CodingSession> sessions)
    {
        TableVisualisation.ShowCodingTable(sessions);
    }
}