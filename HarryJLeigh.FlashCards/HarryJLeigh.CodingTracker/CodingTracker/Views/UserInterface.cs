using CodingTracker.Helpers;
using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.Views;

public class UserInterface
{
    private readonly SessionService _sessionService = new SessionService();

    public void Run()
    {
        bool endApp = false;
        while (!endApp)
        {
            Console.Clear();
            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Enums>()
                    .Title("[green]What would you like to do?[/]")
                    .AddChoices(Enum.GetValues<Enums>().ToList())
            );
            AnsiConsole.MarkupLine($"You selected: [bold yellow]{menuChoice}[/]");

            switch (menuChoice)
            {
                case Enums.ViewAllSessions:
                    _sessionService.ViewAllSessions();
                    Utilities.AskUserToContinue();
                    break;
                case Enums.ViewWithFilter:
                    _sessionService.ViewSessionsWithFilter();
                    break;
                case Enums.Insert:
                    _sessionService.InsertSession();
                    break;
                case Enums.Update:
                    _sessionService.UpdateSession();
                    break;
                case Enums.Delete:
                    _sessionService.DeleteSession();
                    break;
                case Enums.Report:
                    _sessionService.GenerateReport();
                    break;
                case Enums.Stopwatch:
                    _sessionService.StartStopwatch();
                    break;
                case Enums.Goal:
                    GoalView.ShowMenu();
                    break;
                case Enums.Exit:
                    endApp = true;
                    break;
            }
        }
    }
}