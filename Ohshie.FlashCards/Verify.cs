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

    public static bool SessionsExist(string where)
    {
        StudySessionRepository sessionRepository = new();
        var sessions = sessionRepository.FetchSessions();
        
        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("Oops, you got no [bold]studying sessions[/]. Looks like you need to study first\n" +
                                   $"Press enter to {where}");
            Console.ReadLine();

            return false;
        }

        return true;
    }

    public static bool EnteredAppropriateLength(string userInput, int appropriateLength)
    {
        var length = userInput.Length;

        if (length > appropriateLength)
        {
            AnsiConsole.MarkupLine($"Hm, it looks like {userInput} is a bit too long\n" +
                                   $"Try to keep that under {appropriateLength} characters\n" +
                                   $"Press enter to try again");
            Console.ReadLine();

            return false;
        }

        return true;
    }
}