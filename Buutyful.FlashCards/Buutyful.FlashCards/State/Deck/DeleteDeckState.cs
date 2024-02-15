using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Models;
using Buutyful.FlashCards.Repository;
using Spectre.Console;

namespace Buutyful.FlashCards.State;

public class DeleteDeckState(StateManager manager, Deck deck) : IState
{
    private readonly StateManager _stateManager = manager;
    private readonly DeckRepository _deckRepository = new();
    private readonly Deck _deck = deck;
    public ICommand GetCommand()
    {
        return new SwitchStateCommand(_stateManager, new DecksViewState(_stateManager));
    }

    public void Render()
    {
        AnsiConsole.MarkupLine($"[red]Deliting[/]: {_deck.Id}, {_deck.Name}, {_deck.Category}.\n" +
            $"[red](Warning, all deck relative cards will be also eliminated!)[/]\n" +
            $"Continue? [yellow][[y]]/[[n]][/]");
        var input = Console.ReadLine();
        if (input?.ToLower() == "break" || input?.ToLower() == "n") return;
        else if (input?.ToLower() == "y")
        {
            bool exists = DeckExists(_deck.Id);
            if (!exists)
            {
                Console.WriteLine("Deck Not Found");
                return;
            }
            try
            {
                _deckRepository.Delete(_deck);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }       
    }
    private bool DeckExists(int id) => _deckRepository.Find(d => d.Id == id);
}
