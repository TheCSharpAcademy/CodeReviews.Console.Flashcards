

using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Helper;
using Buutyful.FlashCards.Models;
using Buutyful.FlashCards.Repository;
using Spectre.Console;

namespace Buutyful.FlashCards.State.Session;

public class ViewSessionsState : IState
{
    private readonly StateManager _stateManager;
    private readonly SessionRepository _sessionRepository = new();
    private readonly List<Commands> commands = new()
    {   
        Commands.StartNewSession,
        Commands.Menu,
        Commands.Back,       
        Commands.Quit
    };
    private List<StudySession> Sessions { get; set; } = new();

    public ViewSessionsState(StateManager manager)
    {
        _stateManager = manager;
        Sessions = [.. _sessionRepository.GetAll()];
    }
    public ICommand GetCommand()
    {

        var command = UiHelper.DisplayOptions(commands.CommandsToStrings()).ToLower();       
        return UiHelper.MenuSelector(command, _stateManager);
    }

    public void Render()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[gray]=====Sessions=====[/]");

        var table = new Table();
        // Add some columns       
        table.AddColumn("DeckName");
        table.AddColumn("CreatedAt");
        table.AddColumn("Score");

        foreach (var session in Sessions)
        {
            // Add some rows
            table.AddRow($"{session.Deck.Name}", $"{session.CreatedAt}" , $"{session.Score}");
        }
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[gray]========================[/]");
    }


}
