using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Helper;
using Buutyful.FlashCards.Models;
using Buutyful.FlashCards.Repository;
using Spectre.Console;

namespace Buutyful.FlashCards.State;

public class CreateCardState(StateManager manager, Deck deck) : IState
{
    private readonly StateManager _manager = manager;
    private readonly CardRepository _cardRepository = new();
    private readonly Deck _deck = deck;
    public ICommand GetCommand()
    {
        return new SwitchStateCommand(_manager, new DeckCardsViewState(_manager, _deck));
    }

    public void Render()
    {
        Console.WriteLine("Create Card: ");
        bool loop = true;
        while (loop)
        {
            var (FrontQuestion, BackAnswer) = UiHelper.GetCardParameters();
            var res = FlashCardCreateDto.Create(_deck.Id,
                                                 FrontQuestion,
                                                 BackAnswer);
            if (string.IsNullOrEmpty(res.FrontQuestion) || 
                string.IsNullOrEmpty(res.BackAnswer)) break;
            AnsiConsole.MarkupLine($"FrontQuestion: [green]{res.FrontQuestion}[/]," +
                $" BackAnswer: [green]{res.BackAnswer}[/].\n Create? [yellow][[y]]/[[n]][/]");
            var input = Console.ReadLine()?.ToLower();
            if (input == "y")
            {
                _cardRepository.Add(FlashCardCreateDto.ToCard(res));
                loop = false;
            }
            if (input == "break") break;
        }


    }
   
}
