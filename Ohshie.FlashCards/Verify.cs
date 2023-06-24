using Ohshie.FlashCards.DataAccess;

namespace Ohshie.FlashCards;

public static class Verify
{
    public static bool DeckExist(string where)
    {
        DecksRepository decksRepository = new();
        var stacks = decksRepository.FetchAllDecks();
        
        if (!stacks.Any())
        {
            AnsiConsole.MarkupLine("Oops, you got no [bold]Decks[/]. Please create one first\n" +
                                  $"Press enter to {where}");
            Console.ReadLine();

            return false;
        }

        return true;
    }
}