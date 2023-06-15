using Ohshie.FlashCards.StacksManager;

namespace Ohshie.FlashCards.Menus;

public class EditDecksMenu
{
    private DecksService _decksService = new();

    public void Initialize()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("Settings"));
        AnsiConsole.Write(DecksTable());

        var test = Selector();
    }

    private int Selector()
    {
       return AnsiConsole.Prompt
       (
           new TextPrompt<int>("Select Deck id to edit it")
               .PromptStyle("blue")
               .ValidationErrorMessage("this id does not exist")
               .Validate(id =>
               {
                   if (!_decksService.DeckExist(id)) return ValidationResult.Error("You've entered deck id that does not exist");
                   return ValidationResult.Success();
               })
           );
    }

    private Table DecksTable()
    {
        var table = new Table();

        table.AddColumn("Deck Id");
        table.AddColumn("Name");
        table.AddColumn("Description");
        table.AddColumn("Amount of flashcards");

        var deckList = _decksService.FetchAllDecksFromDb();

        foreach (var deck in deckList)
        {
            table.AddRow($"{deck.Id}",$"{deck.Name}",$"{deck.Description}", $"{deck.FlashCards.Count}");
        }

        return table;
    }
}