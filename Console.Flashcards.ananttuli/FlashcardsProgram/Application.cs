using FlashcardsProgram.Database;
using FlashcardsProgram.Flashcards;
using FlashcardsProgram.Reports;
using FlashcardsProgram.Stacks;
using FlashcardsProgram.StudySession;
using Spectre.Console;

namespace FlashcardsProgram;

public class Application(
    StacksController stacksController,
    StudySessionsController sessionsController,
    ReportsController reportsController
)
{
    public void Start()
    {
        bool isAppRunning = true;

        ShowWelcomeMessage();
        do
        {
            isAppRunning = ShowMainMenu();
            Console.Clear();
        } while (isAppRunning);

        ShowExitMessage();
    }

    public bool ShowMainMenu()
    {
        bool keepAppRunning = true;
        bool waitForKeyInputToClear = true;

        var selected = Menu.ShowMainMenu();

        AnsiConsole.MarkupLine(selected);

        switch (selected)
        {
            case Menu.CREATE_SESSION:
                sessionsController.Study();
                break;
            case Menu.VIEW_SESSIONS:
                sessionsController.ShowSessionsList();
                break;
            case Menu.MANAGE_STACKS_CARDS:
                var didPressBack = stacksController.ManageStacksCards();
                waitForKeyInputToClear = !didPressBack;
                break;
            case Menu.CREATE_STACK:
                stacksController.CreateOrUpdateStack();
                break;
            case Menu.VIEW_REPORT:
                reportsController.AverageScorePerMonthPerStack();
                break;
            case Menu.EXIT:
                keepAppRunning = false;
                waitForKeyInputToClear = false;
                break;
        }

        if (keepAppRunning && waitForKeyInputToClear)
        {
            Utils.ConsoleUtil.PressAnyKeyToClear();
        }

        return keepAppRunning;
    }

    public static void ShowExitMessage()
    {
        AnsiConsole.MarkupLine("[green]Thanks[/] for using Flashcards app!");
    }

    public static void ShowWelcomeMessage()
    {
        AnsiConsole.Write(
            new FigletText("Flashcards")
                .Centered()
                .Color(Color.Yellow)
        );

        AnsiConsole.MarkupLine("[green]Welcome![/]");

    }
}