using Flashcards.wkktoria.Controllers;
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

    internal void Run()
    {
        SetUp();
        Console.Clear();

        _stackController.Study(_studyStack!.Id);
    }

    private void SetUp()
    {
        Console.Clear();

        var stacks = _stackService.GetAll();

        if (stacks.Any())
        {
            TableVisualisation.ShowStacksTable(stacks);

            do
            {
                var id = UserInput.GetNumberInput("Enter id of stack to study.");
                var stack = _stackService.GetByDtoId(id);
                _studyStack = stack;

                if (_studyStack == null) UserOutput.ErrorMessage("No stack found.");
            } while (_studyStack == null);
        }
        else
        {
            UserOutput.InfoMessage("No stacks found.");
            ConsoleHelpers.PressToContinue();
        }
    }
}