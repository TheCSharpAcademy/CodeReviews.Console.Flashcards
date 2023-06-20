namespace Ohshie.FlashCards;

public static class Verify
{
    public static bool DeckExist()
    {
        DbOperations dbOperations = new();
        var stacks = dbOperations.FetchAllDecks();
        
        if (!stacks.Any())
        {
            AnsiConsole.MarkupLine("Oops, you got no [bold]Decks[/]. Please create one first\n" +
                                  "Press enter to go back");
            Console.ReadLine();

            return false;
        }

        return true;
    }
}