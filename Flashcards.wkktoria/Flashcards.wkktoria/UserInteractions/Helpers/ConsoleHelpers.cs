namespace Flashcards.wkktoria.UserInteractions.Helpers;

internal static class ConsoleHelpers
{
    internal static void PressToContinue()
    {
        UserOutput.InfoMessage("Press any key to continue...");
        Console.ReadKey();
    }
}