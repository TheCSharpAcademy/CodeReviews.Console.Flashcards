using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Helper;
using Buutyful.FlashCards.Models;


namespace Buutyful.FlashCards.State;

internal class SelectCardState(StateManager manager, List<FlashCard> cards, string command) : IState
{
    private readonly StateManager _stateManager = manager;
    private readonly List<FlashCard> _cards = cards;
    private readonly string _command = command;

    public ICommand GetCommand()
    {
        var card = SelectCard();
        return CardViewSelector(_command, card);
    }

    public void Render()
    {
        Console.WriteLine("Select Card:");
    }
    private ICommand CardViewSelector(string cmd, FlashCard card) => cmd?.ToLower() switch
    {
        "updatecard" => new SwitchStateCommand(_stateManager, new UpdateCardState(_stateManager, card)),
        "deletecard" => new SwitchStateCommand(_stateManager, new DeleteCardState(_stateManager, card)),
        _ => new InvalidCommand(cmd, "deckviewselector")
    };
    private FlashCard SelectCard()
    {
        var cardId = UiHelper.DisplayOptions(_cards.Select(c => c.Id.ToString()));
        Console.WriteLine($"Selected card Id: {cardId}");
        return _cards.First(c => int.Parse(cardId) == c.Id);
    }
}


