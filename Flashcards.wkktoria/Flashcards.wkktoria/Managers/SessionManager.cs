using Flashcards.wkktoria.Controllers;
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
        Choose();
        Console.Clear();

        _sessionController.ShowAll(_currentStack!.Id);
    }

    private void Choose()
    {
        Console.Clear();

        var stacks = _stackService.GetAll();

        if (stacks.Any())
        {
            TableVisualisation.ShowStacksTable(stacks);

            do
            {
                var id = UserInput.GetNumberInput("Enter id of stack to see study sessions.");
                var stack = _stackService.GetByDtoId(id);
                _currentStack = stack;

                if (_currentStack == null) UserOutput.ErrorMessage("No stack found.");
            } while (_currentStack == null);
        }
        else
        {
            UserOutput.InfoMessage("No stacks found.");
            ConsoleHelpers.PressToContinue();
        }
    }
}