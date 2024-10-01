using Flashcard_Application.DataServices;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.UI;

public class StackSelectionPrompt
{
    public static void StackSelectorPrompt()
    {
        List<CardStack> stacks = StackDatabaseServices.GetAllStacks();
        string[] stackNameArray = new string[stacks.Count];

        for (int i = 0; i < stackNameArray.Length; i++)
        {
            stackNameArray[i] = stacks[i].StackName;
        }

        var stackSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
        .Title("[green]\n\nPlease select a stack: [/]")
                .PageSize(10)
                .AddChoices(stackNameArray)
                );
    }
}
