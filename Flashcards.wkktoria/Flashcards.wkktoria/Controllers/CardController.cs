using Flashcards.wkktoria.Models.Dtos;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions;
using Flashcards.wkktoria.UserInteractions.Helpers;
using Flashcards.wkktoria.Validators;

namespace Flashcards.wkktoria.Controllers;

internal class CardController
{
    private readonly CardService _cardService;

    internal CardController(CardService cardService)
    {
        _cardService = cardService;
    }

    internal void ShowAll(int stackId)
    {
        Console.Clear();

        var cards = _cardService.GetAll(stackId);

        if (cards.Any())
            TableVisualisation.ShowCardsTable(cards);
        else
            UserOutput.InfoMessage("No cards in stack.");

        ConsoleHelpers.PressToContinue();
    }

    internal void ShowX(int stackId)
    {
        Console.Clear();

        var cards = _cardService.GetAll(stackId);

        if (cards.Any())
        {
            var cardsAmount = cards.Count;
            var limit = UserInput.GetNumberInput($"Enter how many cards to show (max: {cardsAmount}).");

            while (limit > cardsAmount)
            {
                UserOutput.ErrorMessage($"Max amount is {cardsAmount}.");
                limit = UserInput.GetNumberInput($"Enter how many cards to show (max: {cardsAmount}).");
            }

            var cardsToShow = cards.Take(limit).ToList();
            TableVisualisation.ShowCardsTable(cardsToShow);
        }
        else
        {
            UserOutput.InfoMessage("No cards in stack.");
        }

        ConsoleHelpers.PressToContinue();
    }

    internal void Create(int stackId)
    {
        Console.Clear();
        CardDto newCard;

        do
        {
            var front = UserInput.GetStringInput("Enter front of new card.");
            var back = UserInput.GetStringInput("Enter back of new card.");

            newCard = new CardDto
            {
                Front = front,
                Back = back
            };

            if (!CardValidator.Check(newCard)) UserOutput.ErrorMessage("Card is not valid.");
        } while (!CardValidator.Check(newCard));

        var created = _cardService.Create(newCard, stackId);

        if (created)
            UserOutput.SuccessMessage("Card has been created.");
        else
            UserOutput.ErrorMessage("Failed to create card");

        ConsoleHelpers.PressToContinue();
    }

    internal void Delete(int stackId)
    {
        Console.Clear();

        var cards = _cardService.GetAll(stackId);

        if (cards.Any())
        {
            TableVisualisation.ShowCardsTable(cards);

            var id = UserInput.GetNumberInput("Enter id of card to delete.");
            var cardToDelete = _cardService.GetByDtoId(id, stackId);

            if (cardToDelete != null)
            {
                var deleted = _cardService.Delete(cardToDelete.Id, stackId);

                if (deleted)
                    UserOutput.SuccessMessage("Card has been deleted.");
                else
                    UserOutput.ErrorMessage("Failed to delete card.");
            }
            else
            {
                UserOutput.ErrorMessage("No card found.");
            }
        }
        else
        {
            UserOutput.InfoMessage("No cards to delete.");
        }

        ConsoleHelpers.PressToContinue();
    }

    internal void Update(int stackId)
    {
        Console.Clear();

        var cards = _cardService.GetAll(stackId);

        if (cards.Any())
        {
            TableVisualisation.ShowCardsTable(cards);

            var id = UserInput.GetNumberInput(
                "Enter id of card to update.");

            var cardToUpdate = _cardService.GetByDtoId(id, stackId);

            if (cardToUpdate != null)
            {
                CardDto updatedCard;

                do
                {
                    var newFront =
                        UserInput.GetStringInput("Enter new front for card (or press enter to use current front).");
                    if (newFront == "") newFront = cardToUpdate.Front;
                    var newBack =
                        UserInput.GetStringInput("Enter new back for card (or press enter to use current back).");
                    if (newBack == "") newBack = cardToUpdate.Back;

                    updatedCard = new CardDto
                    {
                        Front = newFront,
                        Back = newBack
                    };

                    if (!CardValidator.Check(updatedCard)) UserOutput.ErrorMessage("Card is not valid.");
                } while (!CardValidator.Check(updatedCard));

                var updated = _cardService.Update(cardToUpdate, updatedCard, stackId);

                if (updated)
                    UserOutput.SuccessMessage("Card has been updated.");
                else
                    UserOutput.ErrorMessage("Failed to update card.");
            }
            else
            {
                UserOutput.ErrorMessage("No card found.");
            }
        }
        else
        {
            UserOutput.InfoMessage("No cards to update.");
        }

        ConsoleHelpers.PressToContinue();
    }
}