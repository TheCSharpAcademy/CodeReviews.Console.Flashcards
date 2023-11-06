using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions;
using Flashcards.wkktoria.UserInteractions.Helpers;

namespace Flashcards.wkktoria.Controllers;

internal class SessionController
{
    private readonly SessionService _sessionService;

    internal SessionController(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    internal void ShowAll(int stackId)
    {
        Console.Clear();

        var cards = _sessionService.GetAll(stackId);

        if (cards.Any())
            TableVisualisation.ShowSessionsTable(cards);
        else
            UserOutput.InfoMessage("No sessions in stack.");

        ConsoleHelpers.PressToContinue();
    }
}