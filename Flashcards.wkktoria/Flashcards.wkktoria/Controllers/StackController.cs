using Flashcards.wkktoria.Models.Dtos;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions;
using Flashcards.wkktoria.UserInteractions.Helpers;
using Flashcards.wkktoria.Validators;

namespace Flashcards.wkktoria.Controllers;

internal class StackController
{
    private readonly CardService _cardService;
    private readonly SessionService _sessionService;
    private readonly StackService _stackService;

    internal StackController(StackService stackService, CardService cardService, SessionService sessionService)
    {
        _stackService = stackService;
        _cardService = cardService;
        _sessionService = sessionService;
    }

    internal void ShowAll()
    {
        Console.Clear();

        var stacks = _stackService.GetAll();

        if (stacks.Any()) TableVisualisation.ShowStacksTable(stacks);
        else
            UserOutput.InfoMessage("No stacks found.");

        ConsoleHelpers.PressToContinue();
    }

    internal void Create()
    {
        Console.Clear();

        var name = UserInput.GetStringInput("Enter name for new stack.");

        while (!StackValidator.CheckName(name))
        {
            UserOutput.ErrorMessage($"'{name}' is invalid name.");
            name = UserInput.GetStringInput("Enter name for new stack.");
        }

        while (_stackService.CheckIfNameExists(name))
        {
            UserOutput.ErrorMessage($"Stack with name '{name}' already exists.");
            name = UserInput.GetStringInput("Enter name for new stack.");
        }

        var newStack = new StackDto
        {
            Name = name
        };

        var created = _stackService.Create(newStack);

        if (created)
            UserOutput.SuccessMessage($"Stack '{name}' has been created.");
        else
            UserOutput.ErrorMessage($"Failed to create stack with name '{name}'.");

        ConsoleHelpers.PressToContinue();
    }

    internal void Delete()
    {
        Console.Clear();

        var stacks = _stackService.GetAll();

        if (stacks.Any())
        {
            TableVisualisation.ShowStacksTable(stacks);

            var id = UserInput.GetNumberInput("Enter id of stack to delete.");
            var stackToDelete = _stackService.GetByDtoId(id);

            if (stackToDelete != null)
            {
                var deleted = _stackService.Delete(stackToDelete.Id);

                if (deleted)
                    UserOutput.SuccessMessage("Stack has been deleted.");
                else
                    UserOutput.ErrorMessage("Failed to delete stack.");
            }
            else
            {
                UserOutput.ErrorMessage("No stack found.");
            }
        }
        else
        {
            UserOutput.InfoMessage("No stacks to delete.");
        }

        ConsoleHelpers.PressToContinue();
    }

    internal void Study(int stackId)
    {
        var cards = _cardService.GetAll(stackId);
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

            Console.Clear();

            var session = new SessionDto
            {
                Date = DateTime.Now, // DateTime.Now.ToString("dd MMM yyyy HH:mm:ss"),
                Score = score
            };

            var created = _sessionService.Create(session, stackId);

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
}