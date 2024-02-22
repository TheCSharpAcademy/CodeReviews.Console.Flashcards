using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Models;
using Buutyful.FlashCards.Repository;
using Spectre.Console;

namespace Buutyful.FlashCards.State;

public class DeleteCardState(StateManager manager, FlashCard card) : IState
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
        AnsiConsole.MarkupLine($"[red]Deliting[/]: {_card.Id}, {_card.FrontQuestion}," +
            $" {_card.BackAnswer}.\nContinue? [yellow][[y]]/[[n]][/]");

        var input = Console.ReadLine();
        if (input?.ToLower() != "y") return;

        bool exists = CardExists(_card.Id);
        if (!exists)
        {
            Console.WriteLine("Card Not Found");
            return;
        }
        try
        {
            _cardRepository.Delete(_card);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadLine();
        }

    }
    private bool CardExists(int id) => _cardRepository.Find(c => c.Id == id);
}