using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Helper;
using Buutyful.FlashCards.Models;
using Buutyful.FlashCards.Repository;
using Spectre.Console;

namespace Buutyful.FlashCards.State;

public class CardsViewState: IState
{
    private readonly StateManager _stateManager;
    private readonly CardRepository _cardRepository = new();
    private readonly List<Commands> commands = new()
    {    
        Commands.UpdateCard,
        Commands.DeleteCard,
        Commands.Menu,
        Commands.Back,
        Commands.Forward,
        Commands.Clear,
        Commands.Quit
    };
    private List<FlashCard> Cards { get; set; } = new();

    public CardsViewState(StateManager manager)
    {
        _stateManager = manager;
        Cards = [.. _cardRepository.GetAll()];
    }
    public ICommand GetCommand()
    {

        var command = UiHelper.DisplayOptions(commands.CommandsToStrings()).ToLower();
        if (command == "updatecard" || command == "deletecard")
        {
            return new SwitchStateCommand(_stateManager, new SelectCardState(_stateManager, Cards, command));
        }
        return UiHelper.MenuSelector(command, _stateManager);
    }

    public void Render()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[gray]=====Cards=====[/]");

        var table = new Table();
        // Add some columns       
        table.AddColumn("Id");
        table.AddColumn("Front");
        table.AddColumn("Back");

        foreach (var card in Cards)
        {
            // Add some rows
            table.AddRow($"{card.Id}", $"{card.FrontQuestion}", $"{card.BackAnswer}");
        }
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[gray]========================[/]");
    }
}
