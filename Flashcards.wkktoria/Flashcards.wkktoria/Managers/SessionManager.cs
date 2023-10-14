using Flashcards.wkktoria.Models;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions;
using Flashcards.wkktoria.UserInteractions.Helpers;

namespace Flashcards.wkktoria.Managers;

internal class SessionManager
{
    private readonly SessionService _sessionService;
    private readonly StackService _stackService;
    private Stack? _currentStack;

    internal SessionManager(StackService stackService, SessionService sessionService)
    {
        _stackService = stackService;
        _sessionService = sessionService;
    }

    internal void Run()
    {
        Choose();
        Console.Clear();

        var sessions = _sessionService.GetAll(_currentStack!.Id);

        if (sessions.Any())
            TableVisualisation.ShowSessionsTable(sessions);
        else
            UserOutput.ErrorMessage("Stack is empty.");


        ConsoleHelpers.PressToContinue();
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