using Flashcard_Application.DataServices;
using Flashcards.UI;
using Spectre.Console;

namespace Flashcard_Application.UI;

public class DeleteStack
{
    public static void DeleteStackPrompt()
    {
        Console.Clear();

        var stacks = StackDatabaseServices.GetAllStacks();
        string[] stackNameArray = new string[stacks.Count];

        for (int i = 0; i < stackNameArray.Length; i++)
        {
            stackNameArray[i] = stacks[i].StackName;
        }

        var stackSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Please select the stack you wish to delete from the list below: [/]")
                .PageSize(10)
                .AddChoices(stackNameArray)
                );

        StackDatabaseServices.DeleteStack(stackSelection);

        MainMenu.MainMenuPrompt();
    }
}
