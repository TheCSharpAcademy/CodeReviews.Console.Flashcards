using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
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
            var tuple = GetCardParameters();
            var res = FlashCardCreateDto.Create(_deck.Id,
                                                 tuple.FrontQuestion,
                                                 tuple.BackAnswer);
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
    private (string FrontQuestion, string BackAnswer) GetCardParameters()
    {
        var question = string.Empty;
        var answer = string.Empty;
        bool condition = true;
        while (condition)
        {
            Console.WriteLine("Select Card Question:");

            var input = Console.ReadLine();
            if (input?.ToLower() == "break") break;
            if (input is not null) question = input;

            Console.WriteLine("Select Card Answer:");

            answer = Console.ReadLine();
            if (answer?.ToLower() == "break") break;

            condition = (string.IsNullOrEmpty(question) || string.IsNullOrEmpty(answer));
        }
        return (question, answer);
    }
}
