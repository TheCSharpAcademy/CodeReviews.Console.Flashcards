using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Helper;
using Buutyful.FlashCards.Repository;

namespace Buutyful.FlashCards.State;

public class CreateDeckState(StateManager manager) : IState
{
    private readonly StateManager _stateManager = manager;
    private readonly DeckRepository _deckRepository = new();
    private readonly List<Commands> commands = new()
    {
        Commands.Menu,
        Commands.Back,
        Commands.Quit
    };

    public ICommand GetCommand()
    {
        var command = UiHelper.DisplayOptions(commands.CommandsToStrings());
        return UiHelper.MenuSelector(command, _stateManager);
    }

    public void Render()
    {
        Console.WriteLine("Create Deck State");
        var res = GetDeckParameters();

    }
    private (string Name, string Category) GetDeckParameters()
    {
        var name = string.Empty;
        var category = string.Empty;
        bool condition = true;
        while (condition)
        {
            Console.WriteLine("Select Deck Name:");
            var input = Console.ReadLine();
            if (input is not null && !ContainsName(input))
            {
                name = input;
            }
            else
            {
                Console.WriteLine("Already a deck present with same name");
                continue;
            }
            Console.WriteLine("Select Deck Category:");
            category = Console.ReadLine();
            condition = (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(category));
        }
        return (name, category);
    }
    private bool ContainsName(string name) =>
        _deckRepository.Find(x => name.Equals(x.Name));


}
