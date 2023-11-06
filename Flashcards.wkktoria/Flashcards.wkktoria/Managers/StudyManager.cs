using Flashcards.wkktoria.Controllers;
using Flashcards.wkktoria.Managers.Helpers;
using Flashcards.wkktoria.Models;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions;
using Flashcards.wkktoria.UserInteractions.Helpers;

namespace Flashcards.wkktoria.Managers;

internal class StudyManager
{
    private readonly StackController _stackController;
    private readonly StackService _stackService;
    private Stack? _studyStack;

    internal StudyManager(StackService stackService, CardService cardService, SessionService sessionService)
    {
        _stackService = stackService;
        _stackController = new StackController(stackService, cardService, sessionService);
    }

    internal void Run(Stack? stackToStudy = null)
    {
        Console.Clear();

        if (_stackService.GetAll().Any())
        {
            _studyStack = stackToStudy ?? StackHelper.Choose(_stackService);
            _stackController.Study(_studyStack!.Id);
        }
        else
        {
            UserOutput.InfoMessage("No stacks found.");
            ConsoleHelpers.PressToContinue();
        }
    }
}