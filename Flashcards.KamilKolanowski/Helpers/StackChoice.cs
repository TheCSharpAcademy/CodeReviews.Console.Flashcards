using Flashcards.KamilKolanowski.Data;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Helpers;

public static class StackChoice
{
    internal static int GetStackChoice(DatabaseManager databaseManager)
    {
        var stacks = databaseManager.ReadStacks().Select(s => (s.StackId, s.StackName)).ToList();

        var stackNames = stacks.Select(x => x.Item2).ToList();

        var stackChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose the Stack from the list")
                .AddChoices(stackNames)
        );

        return stacks.First(x => x.Item2 == stackChoice).Item1;
    }
}
