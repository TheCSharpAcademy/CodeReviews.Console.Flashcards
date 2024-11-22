using CodingTracker.Controllers;
using CodingTracker.Data;
using CodingTracker.Helpers;
using CodingTracker.Models;
using CodingTracker.Views;
using Spectre.Console;

namespace CodingTracker.Services;

public class GoalsService
{
    private readonly GoalController _goalController = new GoalController(new DataBaseService());
    private readonly CrudController _sqlController = new CrudController(new DataBaseService());

    internal void InsertGoal()
    {
        AnsiConsole.MarkupLine("[yellow]Set your goal: [/]");
        DateTime goalEndDate = Utilities.GetDatetime("End Date");
        DateTime goalStartDate = DateTime.Now;
        while (!DateValidator.IsValidEndDate(goalStartDate, goalEndDate))
        {
            goalEndDate = Utilities.GetDatetime("End Date");
        }

        int goalHours = Utilities.GetHours();
        string goalHoursString = goalHours.ToString();
        AnsiConsole.MarkupLine($"[yellow]Goal End Date: {goalEndDate}[/]");
        AnsiConsole.MarkupLine($"[yellow]Goal Hours: {goalHoursString}[/]");

        Utilities.AskUserToContinue();
        _goalController.InsertGoal(goalStartDate.ToString("yyyy-MM-dd HH:mm"), goalEndDate.ToString("yyyy-MM-dd HH:mm"),
            goalHoursString);
    }

    internal void ViewGoals()
    {
        List<Goal> goals = _goalController.GetAllGoals();
        TableVisualisation.ShowGoalsTable(goals);
        Utilities.AskUserToContinue();
    }

    private void ViewGoal(int id)
    {
        List<Goal> goals = _goalController.GetGoal(id);
        TableVisualisation.ShowGoalsTable(goals);
    }

    internal void UpdateGoal()
    {
        int id = Utilities.GetId(false, true);
        var updateChoice = AnsiConsole.Prompt(
            new SelectionPrompt<UpdateGoalOptions>()
                .Title("[yellow]What would you like to update[/]")
                .AddChoices(Enum.GetValues<UpdateGoalOptions>())
        );

        ViewGoal(id);
        switch (updateChoice)
        {
            case UpdateGoalOptions.EndTime:
                UpdateEndTime(id);
                break;
            case UpdateGoalOptions.Hours:
                UpdateHours(id);
                break;
            case UpdateGoalOptions.Both:
                UpdateBoth(id);
                break;
        }
    }

    private void UpdateGoalInfo(int id, bool updateEndTime, bool updateHours)
    {
        DateTime goalFinishDate = GetPreviousEndTime(id);
        if (updateEndTime)
        {
            goalFinishDate = Utilities.GetDatetime("End Date");

            while (!DateValidator.IsValidEndDate(_goalController.GetGoal(id)[0].ParsedStartDate, goalFinishDate))
            {
                goalFinishDate = Utilities.GetDatetime("End Date");
            }
        }

        DateTime goalEndDate = updateEndTime ? goalFinishDate : GetPreviousEndTime(id);
        int goalHours = updateHours ? Utilities.GetHours() : GetPreviousHours(id);

        string formattedEndDate = goalEndDate.ToString("yyyy-MM-dd HH:mm");
        string formattedHours = goalHours.ToString();

        if (!Utilities.PromptReturnToMenu()) return;
        if (updateEndTime)
        {
            string previousEndDate = GetPreviousEndTime(id).ToString("yyyy-MM-dd HH:mm");
            _goalController.UpdateGoalEndDate(id, formattedEndDate);
            Utilities.DisplayUpdateInfo(id, formattedEndDate, previousEndDate, "End Date");
        }

        if (updateHours)
        {
            string previousHours = GetPreviousHours(id).ToString();
            _goalController.UpdateGoalHours(id, formattedHours);
            Utilities.DisplayUpdateInfo(id, formattedHours, previousHours, "Target Hours");
        }
    }

    private void UpdateHours(int id)
    {
        UpdateGoalInfo(id, false, true);
    }

    private void UpdateEndTime(int id)
    {
        UpdateGoalInfo(id, true, false);
    }

    private void UpdateBoth(int id)
    {
        UpdateGoalInfo(id, true, true);
    }

    internal void CheckGoalStatus()
    {
        int id = Utilities.GetId(false, true);
        List<Goal> goal = _goalController.GetGoal(id);
        DateTime goalStartDate = goal[0].ParsedStartDate;
        DateTime goalEndDate = goal[0].ParsedDateToComplete;
        int goalHours = Convert.ToInt32(goal[0].Hours);

        double? selectedGoalHoursComplete = _sqlController.GetGoalHoursComplete(goalStartDate);
        if (selectedGoalHoursComplete == -1)
        {
            AnsiConsole.MarkupLine(
                $"[green]Goal ID:[/] [cyan]{id}[/] [green]- Hours Remaining:[/] [cyan]{goalHours}[/]");
        }

        AnsiConsole.MarkupLine($"[green]Goal StartDate:[/] [cyan]{goalStartDate}[/]");
        AnsiConsole.MarkupLine($"[green]Goal EndDate:[/] [cyan]{goalEndDate}[/]");
        if (selectedGoalHoursComplete != -1)
        {
            AnsiConsole.MarkupLine(
                $"[green]Goal ID:[/] [cyan]{id}[/] [green]- Hours Remaining:[/] [cyan]{goalHours - selectedGoalHoursComplete}[/]");
        }
        
        TimeSpan daysDifference = goalEndDate - goalStartDate;
        double hoursPerDay = goalHours / daysDifference.TotalDays;
        AnsiConsole.MarkupLine(
            $"[green]Days Left to reach goal:[/] [cyan]{Math.Round(daysDifference.TotalDays, 0)}[/]");
        AnsiConsole.MarkupLine(
            $"[green]Hours per day need to complete goal:[/] [cyan]{Math.Round(hoursPerDay, 2)}[/]  ");
        Utilities.AskUserToContinue();
    }

    internal void DeleteGoal()
    {
        if (!Utilities.CheckGoalsExist(_goalController.GetAllGoals())) return;

        int id = Utilities.GetId(goals: true);
        if (!Utilities.PromptReturnToMenu()) return;

        _goalController.DeleteGoal(id);
        AnsiConsole.MarkupLine($"[bold yellow]Deleted goal with id: {id}[/]");
        Utilities.AskUserToContinue();
    }

    internal void DeleteAllGoals()
    {
        _goalController.DeleteAllGoals();
        AnsiConsole.MarkupLine("[bold yellow]All goals deleted.[/]");
        Utilities.AskUserToContinue();
    }

    internal List<int> GetAllGoalIds()
    {
        List<int> goalIds = new List<int>();
        List<Goal> goals = _goalController.GetAllGoals();

        foreach (Goal goal in goals)
        {
            goalIds.Add(goal.Id);
        }

        return goalIds;
    }

    private DateTime GetPreviousEndTime(int id)
    {
        List<Goal> goal = _goalController.GetGoal(id);
        return goal[0].ParsedDateToComplete;
    }

    private int GetPreviousHours(int id)
    {
        List<Goal> goal = _goalController.GetGoal(id);
        return Convert.ToInt32(goal[0].Hours);
    }
}