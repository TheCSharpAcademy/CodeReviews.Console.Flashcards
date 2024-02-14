using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
using Spectre.Console;

namespace Buutyful.FlashCards.Helper;

public static class UiHelper
{
    public static Dictionary<Commands, string> MapCommands => new()
    {
            {Commands.Info, "Gets you all the infos u need" },
            {Commands.Menu, "Return to the main menu"},
            {Commands.View, "Display database records"},
            {Commands.Create, "Create new entry" },
            {Commands.Update, "Update record" },
            {Commands.Delete, "Delete record" },
            {Commands.Back, "Goes back to past state" },
            {Commands.Forward, "Goes to forward state" },
            {Commands.Clear, "Clear console"},
            {Commands.Break, "Breaks out of input loop"},
            {Commands.Quit, "Quit the application" }
    };
    public static ICommand MenuSelector(string? command, StateManager _manager)
    {
        return command switch
        {
            "info" => new InfoCommand(),
            "view" => new SwitchStateCommand(_manager, new ViewState(_manager)),
            "create" => new SwitchStateCommand(_manager, new CreateState(_manager)),
            "update" => new SwitchStateCommand(_manager, new UpdateState(_manager)),
            "delete" => new SwitchStateCommand(_manager, new DeleteState(_manager)),
            "back" => new SwitchStateCommand(_manager, _manager.PastState()),
            "forward" => new SwitchStateCommand(_manager, _manager.FutureState()),
            "clear" => new ClearCommand(),
            "quit" => new QuitCommand(),
            "menu" => new InvalidCommand(command, "You are already in the main menu"),
            _ => new InvalidCommand(command, "Please select [info] for navigation help"),
        };
    }
    public static string DisplayOptions() =>
      AnsiConsole.Prompt(
      new SelectionPrompt<string>()
      .Title("Main Menu please select command:")
      .PageSize(20)
      .MoreChoicesText("===================")
      .AddChoices(Enum.GetValues(typeof(Commands))
      .Cast<Commands>()
      .Select(o => o.ToString())
          ));
}
