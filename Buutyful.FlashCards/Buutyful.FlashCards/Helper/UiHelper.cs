using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.State;
using Spectre.Console;

namespace Buutyful.FlashCards.Helper;

public static class UiHelper
{
    public static Dictionary<Commands, string> MapCommands => new()
    {
            {Commands.Info, "Gets you all the infos u need" },
            {Commands.Menu, "Return to the main menu"},
            {Commands.View, "Display menu"},               
            {Commands.Back, "Goes back to past state" },
            {Commands.Forward, "Goes to forward state" },
            {Commands.Clear, "Clear console"},
            {Commands.Break, "Breaks out of input loop"},
            {Commands.Quit, "Quit the application" }
    };
    public static ICommand MenuSelector(string? command, StateManager _manager)
    {
        return command?.ToLower() switch
        {
            "info" => new InfoCommand(),
            "menu" => new SwitchStateCommand(_manager, new MainMenuState(_manager)),
            "view" => new SwitchStateCommand(_manager, new ViewState(_manager)),
            "decks" => new SwitchStateCommand(_manager, new DeckViewState(_manager)),            
            "create" => new SwitchStateCommand(_manager, new CreateState(_manager)),
            "back" => new SwitchStateCommand(_manager, _manager.PastState()),
            "forward" => new SwitchStateCommand(_manager, _manager.FutureState()),
            "clear" => new ClearCommand(),
            "quit" => new QuitCommand(),
            _ => new InvalidCommand(command, "Please select [info] for navigation help"),
        };
    }
    public static string DisplayOptions(IEnumerable<string> options) =>
      AnsiConsole.Prompt(
      new SelectionPrompt<string>()     
      .PageSize(10)
      .MoreChoicesText("===================")
      .AddChoices(options));
    public static IEnumerable<string> CommandsToStrings(this IEnumerable<Commands> commands) =>
        commands.Select(c => c.ToString());
}

public enum Commands
{
    Info,
    Menu,
    View,
    Decks,
    DeckCards,
    Cards,  //todo
    Sessions, //todo
    Create,
    CreateDeck, //todo
    CreateCard,//todo    
    UpdateDeck, //todo
    UpdateCard, //todo
    DeleteDeck, //todo
    DeleteCard, //todo
    Back,
    Forward,
    Clear,
    Break,
    Quit
}