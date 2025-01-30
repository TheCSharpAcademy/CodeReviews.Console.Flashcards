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

    public FlashcardApp(ILogger<FlashcardApp> log, IFlashcardScreen flashcardScreen, IStackScreen stackScreen)
    {
        _log = log;
        _flashcardScreen = flashcardScreen;
        _stackScreen = stackScreen;
    }
    public async Task RunApp()
    {
        await _stackScreen.RunStackView();
    }

}
