using Flashcards.wkktoria.Models;
using Flashcards.wkktoria.Models.Dtos;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions;
using Flashcards.wkktoria.UserInteractions.Helpers;

namespace Flashcards.wkktoria.Managers;

internal class StudyManager
{
    private readonly CardService _cardService;
    private readonly SessionService _sessionService;
    private readonly StackService _stackService;
    private Stack? _studyStack;

    internal StudyManager(StackService stackService, CardService cardService, SessionService sessionService)
    {
        _stackService = stackService;
        _cardService = cardService;
        _sessionService = sessionService;
    }

    internal void Run()
    {
        SetUp();
        Console.Clear();

        var cards = _cardService.GetAll(_studyStack!.Id);
        var score = 0;

        if (cards.Any())
        {
            foreach (var card in cards)
            {
                Console.Clear();

                var userBack = UserInput.GetStringInput($"Enter back for '{card.Front.ToUpper()}'.");

                if (string.Equals(userBack, card.Back, StringComparison.CurrentCultureIgnoreCase))
                {
                    UserOutput.SuccessMessage("Correct!");
                    score++;
                }
                else
                {
                    UserOutput.ErrorMessage($"Wrong! Correct answer is: {card.Back.ToUpper()}.");
                }

                ConsoleHelpers.PressToContinue();
            }

            var session = new SessionDto
            {
                Date = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss"),
                Score = score
            };

            var created = _sessionService.Create(session, _studyStack.Id);

            if (created)
                UserOutput.InfoMessage("Session has been added to database.");
            else
                UserOutput.ErrorMessage("Failed to add session to database.");
        }
        else
        {
            UserOutput.ErrorMessage("Stack is empty and cannot be studied.");
        }

        ConsoleHelpers.PressToContinue();
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