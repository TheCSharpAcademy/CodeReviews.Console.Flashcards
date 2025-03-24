using Flashcards.Models.Dtos;
using Flashcards.shared;
using Flashcards.Views.utils;
using Spectre.Console;

namespace Flashcards.Views;

public class ManageStacksView
{
    public ManageStacksOption Show(List<StackDto> stacks)
    {
        
        DtosDisplay.DisplayStacks(stacks);
        return DisplayOptions(stacks.Count > 0);
    }

    private static ManageStacksOption DisplayOptions(bool hasExistingStacks)
    {
        var menuDict = hasExistingStacks
            ? new Dictionary<string, ManageStacksOption>
            {
                { "Back", ManageStacksOption.Back },
                { "Create new stack", ManageStacksOption.Create },
                { "Delete stack", ManageStacksOption.DeleteStack },
                { "Change stack name", ManageStacksOption.ChangeName },
            }
            : new Dictionary<string, ManageStacksOption>
            {
                { "Back", ManageStacksOption.Back },
                { "Create new stack", ManageStacksOption.Create },
            };

        AnsiConsole.Write(new Rule());

        var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .AddChoices(menuDict.Keys));

        return menuDict[selection];
    }
}