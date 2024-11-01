using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Models;
using Buutyful.FlashCards.Repository;
using Buutyful.Coding_Tracker.Command;
using Spectre.Console;

namespace Buutyful.FlashCards.State;

public class CreateDeckState(StateManager manager) : IState
{
    private readonly StateManager _stateManager = manager;
    private readonly DeckRepository _deckRepository = new();


    public ICommand GetCommand()
    {
        return new SwitchStateCommand(_stateManager, new DecksViewState(_stateManager));
    }

    public void Render()
    {
        Console.WriteLine("Create Deck: ");
        bool loop = true;
        while (loop)
        {
            DeckCreateDto res = GetDeckParameters();
            if (string.IsNullOrEmpty(res.Name) || string.IsNullOrEmpty(res.Category)) break;
            AnsiConsole.MarkupLine($"Name: [green]{res.Name}[/]," +
                $" Category: [green]{res.Category}[/], Create? [yellow][[y]]/[[n]][/]");
            var input = Console.ReadLine()?.ToLower();
            if (input == "y")
            {
                _deckRepository.Add(DeckCreateDto.ToDeck(res));
                loop = false;
            }
            if (input == "break") break;
        }


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
            if (input?.ToLower() == "break") break;
            else if (input is not null && !ContainsName(input))
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
            if (category?.ToLower() == "break") break;
            condition = (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(category));
        }
        return (name, category);
    }
    private bool ContainsName(string name) =>
        _deckRepository.Find(x => name.Equals(x.Name));


}
