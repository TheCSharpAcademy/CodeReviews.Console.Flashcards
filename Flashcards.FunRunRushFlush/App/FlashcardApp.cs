using Flashcards.FunRunRushFlush.App.Interfaces;
using Flashcards.FunRunRushFlush.Controller.Interfaces;
using Flashcards.FunRunRushFlush.Data.Model;
using Flashcards.FunRunRushFlush.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Net.NetworkInformation;

namespace Flashcards.FunRunRushFlush.App;

public class FlashcardApp
{
    private readonly ILogger<FlashcardApp> _log;
    private readonly IFlashcardScreen _flashcardScreen;
    private readonly IStackScreen _stackScreen;
    private readonly IStudySessionScreen _studySession;

    public FlashcardApp(ILogger<FlashcardApp> log, IFlashcardScreen flashcardScreen, IStackScreen stackScreen, IStudySessionScreen studySession)
    {
        _log = log;
        _flashcardScreen = flashcardScreen;
        _stackScreen = stackScreen;
        _studySession = studySession;
    }

    private const string Manage_Stack = "Manage Stack";
    private const string Study_Session = "Study Session";
    private const string Exit = "Exit";


    public async Task RunApp()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AppHelperFunctions.AppHeader("FlashcardsApp", true);

            var specialChoices = new[] { Manage_Stack, Study_Session, Exit };

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select an option: [/]")
                    .PageSize(10)
                    .AddChoices(specialChoices)
            );
            if (selection == Manage_Stack)
            {
                await _stackScreen.RunStackView();
            }
            if (selection == Study_Session)
            {
                await _studySession.RunStudySessionView();
            }

            if (selection == Exit)
            {
                if(AppHelperFunctions.CloseApp())
                {
                    break;
                }
            }
        }
    }
}

