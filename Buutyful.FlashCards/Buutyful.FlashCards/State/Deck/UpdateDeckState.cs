using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Models;
using Buutyful.FlashCards.Repository;
using Spectre.Console;

namespace Buutyful.FlashCards.State;

public class UpdateDeckState(StateManager manager, Deck deck) : IState
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
        AnsiConsole.MarkupLine($"[red]Updating[/]: {_deck.Id}, {_deck.Name}, {_deck.Category}.\n" +
            $"Continue? [yellow][[y]]/[[n]][/]");
        var inp = Console.ReadLine();
        if (inp != "y") return;
        var loop = true;
        bool exists = DeckExists(_deck.Id);
        if (!exists)
        {
            Console.WriteLine("Deck Not Found");
            return;
        }
        while (loop)
        {
            DeckCreateDto res = GetDeckParameters();
            var updatedDeck = new Deck() 
            {
                Id = _deck.Id,
                Name = res.Name,
                Category = res.Category
            };
            if (string.IsNullOrEmpty(res.Name) || string.IsNullOrEmpty(res.Category))
            {
                Console.WriteLine("Name and Category cant be null or empty");
                break;
            }
            AnsiConsole.MarkupLine($"Name: [green]{res.Name}[/]," +
                $" Category: [green]{res.Category}[/], Update? [yellow][[y]]/[[n]][/]");
            var input = Console.ReadLine()?.ToLower();
            if (input == "y")
            {
                try
                {
                    _deckRepository.Update(updatedDeck);
                    loop = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
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
            if(condition) Console.WriteLine("Name and Category cant be null or empty");
        }
        return (name, category);
    }

    private bool DeckExists(int id) => _deckRepository.Find(d => d.Id == id);
    private bool ContainsName(string name) =>
       _deckRepository.Find(x => name.Equals(x.Name));
}
