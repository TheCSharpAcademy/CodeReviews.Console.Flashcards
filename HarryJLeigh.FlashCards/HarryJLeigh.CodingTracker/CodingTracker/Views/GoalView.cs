using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.Views;

public static class GoalView
{
    private static readonly GoalsService _goalService = new GoalsService();

    internal static void ShowMenu()
    {
        bool endApp = false;
        while (!endApp)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"You selected: [bold yellow]Goal[/]");
            var goalAction = AnsiConsole.Prompt(
                new SelectionPrompt<GoalOptions>()
                    .Title("[yellow]What would you like to do?[/]")
                    .AddChoices(Enum.GetValues<GoalOptions>())
            );
            Console.Clear();
            AnsiConsole.MarkupLine($"You selected: [bold yellow]{goalAction}[/]");

            switch (goalAction)
            {
                case GoalOptions.ShowGoals:
                    _goalService.ViewGoals();
                    break;
                case GoalOptions.SetGoal:
                    _goalService.InsertGoal();
                    break;
                case GoalOptions.UpdateGoal:
                    _goalService.UpdateGoal();
                    break;
                case GoalOptions.CheckGoalStatus:
                    _goalService.CheckGoalStatus();
                    break;
                case GoalOptions.DeleteGoal:
                    _goalService.DeleteGoal();
                    break;
                case GoalOptions.DeleteAllGoals:
                    _goalService.DeleteAllGoals();
                    break;
                case GoalOptions.Exit:
                    endApp = true;
                    break;
            }
        }
    }
}