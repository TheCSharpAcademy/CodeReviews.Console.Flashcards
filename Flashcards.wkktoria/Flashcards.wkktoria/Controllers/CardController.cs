using Flashcards.wkktoria.Models.Dtos;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.Validators;

namespace Flashcards.wkktoria.Controllers;

internal class CardController
{
    private readonly CardService _cardService;

    internal CardController(CardService cardService)
    {
        _cardService = cardService;
    }

    internal void ShowCards(int stackId)
    {
        Console.Clear();

        var cards = _cardService.GetAll(stackId);

        if (cards.Any())
            TableVisualisation.ShowCardsTable(cards);
        else
            Console.WriteLine("No cards in stack.");

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    internal void ShowXCards(int stackId)
    {
        Console.Clear();

        var cards = _cardService.GetAll(stackId);

        if (cards.Any())
        {
            var cardsAmount = cards.Count;
            var limit = UserInput.GetNumberInput($"Enter how many cards to show (max: {cardsAmount}).");

            while (limit > cardsAmount)
            {
                Console.WriteLine($"Max amount is {cardsAmount}.");
                limit = UserInput.GetNumberInput($"Enter how many cards to show (max: {cardsAmount}).");
            }

            var cardsToShow = cards.Take(limit).ToList();
            TableVisualisation.ShowCardsTable(cardsToShow);
        }
        else
        {
            Console.WriteLine("No cards in stack.");
        }


        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
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

            if (!CardValidator.Check(newCard)) Console.WriteLine("Card is not valid.");
        } while (!CardValidator.Check(newCard));

        var created = _cardService.Create(newCard, stackId);

        Console.WriteLine(created
            ? "Card has been created."
            : "Failed to create card.");

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    internal void Delete(int stackId)
    {
        Console.Clear();

        var cards = _cardService.GetAll(stackId);

        if (cards.Any())
        {
            TableVisualisation.ShowCardsTable(cards);

            var front = UserInput.GetStringInput("Enter front of card to delete.");
            var cardToDelete = _cardService.GetByFront(front, stackId);

            if (cardToDelete.Id == 0)
            {
                Console.WriteLine($"No card with front '{front}'.");
            }
            else
            {
                var deleted = _cardService.Delete(front, stackId);

                Console.WriteLine(deleted
                    ? $"Card with front '{front}' has been deleted."
                    : $"Failed to delete card with front '{front}'.");
            }
        }
        else
        {
            Console.WriteLine("No cards to delete.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    internal void Update(int stackId)
    {
        Console.Clear();

        var cards = _cardService.GetAll(stackId);

        if (cards.Any())
        {
            TableVisualisation.ShowCardsTable(cards);

            var front = UserInput.GetStringInput(
                "Enter front of card to update.");

            var cardToUpdate = _cardService.GetByFront(front, stackId);

            if (cardToUpdate.Id == 0)
            {
                Console.WriteLine($"No card with front '{front}'.");
            }
            else
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

                    if (!CardValidator.Check(updatedCard)) Console.WriteLine("Card is not valid.");
                } while (!CardValidator.Check(updatedCard));

                var updated = _cardService.Update(cardToUpdate, updatedCard, stackId);

                Console.WriteLine(updated
                    ? "Card has been updated."
                    : "Failed to update card.");
            }
        }
        else
        {
            Console.WriteLine("No cards to update.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}