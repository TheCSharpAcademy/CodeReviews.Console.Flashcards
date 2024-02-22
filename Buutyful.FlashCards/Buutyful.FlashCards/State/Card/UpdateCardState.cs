using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Helper;
using Buutyful.FlashCards.Models;
using Buutyful.FlashCards.Repository;
using Spectre.Console;

namespace Buutyful.FlashCards.State;
public class UpdateCardState(StateManager manager, FlashCard card) : IState
{
    private readonly FlashCard _card = card;
    private readonly StateManager _manager = manager;
    private readonly CardRepository _cardRepository = new();  

    public ICommand GetCommand()
    {
        return new SwitchStateCommand(_manager, new CardsViewState(_manager));
    }

    public void Render()
    {
        AnsiConsole.MarkupLine($"[red]Updating[/]: {_card.Id}, {_card.FrontQuestion}," +
            $" {_card.BackAnswer}.\n" +
            $"Continue? [yellow][[y]]/[[n]][/]");

        var inp = Console.ReadLine();
        if (inp != "y") return;

        var loop = true;
        bool exists = CardExists(_card.Id);
        if (!exists)
        {
            Console.WriteLine("Card Not Found");
            return;
        }
        while (loop)
        {
            var (frontQuestion, backAnswer) = UiHelper.GetCardParameters();
            var updatedCard = new FlashCard()
            {
                Id = _card.Id,
                DeckId = _card.DeckId,
                FrontQuestion = frontQuestion,
                BackAnswer = backAnswer
            };
            if (string.IsNullOrEmpty(updatedCard.FrontQuestion) ||
                string.IsNullOrEmpty(updatedCard.BackAnswer))
            {
                Console.WriteLine("FrontQuestion and BackAnswer cant be null or empty");
                break;
            }
            AnsiConsole.MarkupLine($"Name: [green]{updatedCard.FrontQuestion}[/]," +
                $" Category: [green]{updatedCard.BackAnswer}[/], Update? [yellow][[y]]/[[n]][/]");
            var input = Console.ReadLine()?.ToLower();
            if (input == "y")
            {
                try
                {
                    _cardRepository.Update(updatedCard);
                    loop = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            if (input == "break") break;
        }

    }

    private bool CardExists(int id) => _cardRepository.Find(c => c.Id == id);
}