using Spectre.Console;

namespace Flashcards;

public class StacksView
{
    static Color foregroundColor = ViewStyles.foregroundColor;
    
    internal static string StacksMenu()
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(foregroundColor)
                .Title($"Select an [{foregroundColor}]option[/] from the menu:")
                .PageSize(4)
                .AddChoices(new[] {
                    "View Stacks", "Create Stack",
                    "Delete Stack", "Exit"
                }));

        return selected;
    }

    internal static string? SelectStackMenu(List<Stack> stackList, int count)
    {
        stackList.Add(new Stack{StackId = -1, StackName = "Exit", CardQuantity = 0 });

        // Sets page size to 3 if less than since Spectre requires at least 3 choices
        count = count + 1;
        if (count < 3)
        {
            count = 3;
        }

        var selectedStack = AnsiConsole.Prompt(
               new SelectionPrompt<Stack>()
                   .HighlightStyle(foregroundColor)
                   .Title($"Select a [{foregroundColor}]stack[/]:")
                   .PageSize(count) // Number of items to show in the dropdown list
                   .AddChoices(stackList) // Add the list of stacks
                   .UseConverter(stack => stack.StackName) // Display the StackName in the dropdown
           );

        string? parsedName = selectedStack.StackName;

        return parsedName;
    }
}