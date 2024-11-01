using Flashcards.wkktoria.Controllers;
using Flashcards.wkktoria.Managers.Helpers;
using Flashcards.wkktoria.Models;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions;
using Flashcards.wkktoria.UserInteractions.Helpers;

namespace Flashcards.wkktoria.Managers;

internal class SessionManager
{
    private readonly SessionController _sessionController;
    private readonly StackService _stackService;
    private Stack? _currentStack;

    internal SessionManager(StackService stackService, SessionService sessionService)
    {
        _stackService = stackService;
        _sessionController = new SessionController(sessionService);
    }

    internal void Run()
    {
        Console.Clear();

        if (_stackService.GetAll().Any())
        {
            _currentStack = StackHelper.Choose(_stackService);
            _sessionController.ShowAll(_currentStack!.Id);
        }
        else
        {
            UserOutput.InfoMessage("No stacks found.");
            ConsoleHelpers.PressToContinue();
        }
    }
}