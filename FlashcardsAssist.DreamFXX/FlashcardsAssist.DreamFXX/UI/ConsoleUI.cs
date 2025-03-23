using FlashcardsAssist.DreamFXX.Services;
using Spectre.Console;

namespace FlashcardsAssist.DreamFXX.UI;
public class ConsoleUI
{
    private readonly StacksService _stacksService;
    private readonly FlashcardsService _flashcardsService;
    private readonly StudyEngine _studyEngine;
    private readonly SessionsService _sessionsService;
    private readonly ReportingService _reportingService;
    private bool _running = true;

    public ConsoleUI(
        StacksService stacksService, 
        FlashcardsService flashcardsService,
        StudyEngine studyEngine,
        SessionsService sessionsService,
        ReportingService reportingService)
    {
        _stacksService = stacksService;
        _flashcardsService = flashcardsService;
        _studyEngine = studyEngine;
        _sessionsService = sessionsService;
        _reportingService = reportingService;
    }

    public async Task RunAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Flashcards App")
                .Centered()
                .Color(Color.Yellow));

        while (_running)
        {
            await DisplayMainMenuAsync();
        }
    }

    private async Task DisplayMainMenuAsync()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow on bold]Flashcard manager // by DreamFXX[/]")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Manage Stacks",
                    "Manage Flashcards",
                    "Study",
                    "View Study History",
                    "Reports",
                    "Exit"
                }));

        switch (choice)
        {
            case "Manage Stacks":
                await DisplayStacksMenuAsync();
                break;
            case "Manage Flashcards":
                await DisplayFlashcardsMenuAsync();
                break;
            case "Study":
                await _studyEngine.StartStudySessionAsync();
                PressEnterToContinue();
                break;
            case "View Study History":
                await _sessionsService.ViewStudyHistoryAsync();
                PressEnterToContinue();
                break;
            case "Reports":
                await DisplayReportsMenuAsync();
                break;
            case "Exit":
                _running = false;
                AnsiConsole.MarkupLine("[green]Goodbye![/]");
                break;
        }
    }

    private async Task DisplayReportsMenuAsync()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Reports Menu[/]")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Sessions per Month",
                    "Average Score per Month",
                    "Back to Main Menu"
                }));

        switch (choice)
        {
            case "Sessions per Month":
                await _reportingService.ViewSessionsPerMonthReportAsync();
                PressEnterToContinue();
                break;
            case "Average Score per Month":
                await _reportingService.ViewAverageScorePerMonthReportAsync();
                PressEnterToContinue();
                break;
            case "Back to Main Menu":
                return;
        }
    }

    private async Task DisplayStacksMenuAsync()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Stacks Menu[/]")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Create New Stack",
                    "View All Stacks",
                    "Delete Stack",
                    "Back to Main Menu"
                }));

        switch (choice)
        {
            case "Create New Stack":
                await _stacksService.CreateStackAsync();
                PressEnterToContinue();
                break;
            case "View All Stacks":
                await _stacksService.ViewAllStacksAsync();
                PressEnterToContinue();
                break;
            case "Delete Stack":
                await _stacksService.DeleteStackAsync();
                PressEnterToContinue();
                break;
            case "Back to Main Menu":
                return;
        }
    }

    private async Task DisplayFlashcardsMenuAsync()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Flashcards Menu[/]")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Add New Flashcard",
                    "View Flashcards in Stack",
                    "Back to Main Menu"
                }));

        switch (choice)
        {
            case "Add New Flashcard":
                await _flashcardsService.AddFlashcardAsync();
                PressEnterToContinue();
                break;
            case "View Flashcards in Stack":
                await _flashcardsService.ViewFlashcardsAsync();
                PressEnterToContinue();
                break;
            case "Back to Main Menu":
                return;
        }
    }

    private void PressEnterToContinue()
    {
        AnsiConsole.MarkupLine("\n[grey]Press Enter to continue...[/]");
        Console.ReadLine();
    }
}
