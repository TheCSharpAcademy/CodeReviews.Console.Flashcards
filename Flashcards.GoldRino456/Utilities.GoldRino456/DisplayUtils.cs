using Spectre.Console;

namespace Utilities.GoldRino456;
public static class DisplayUtils
{
    /// <summary>
    /// Prompts the user for a selection from a dictionary of choices that correspond to a specific integer value.
    /// </summary>
    /// <param name="promptText"></param>
    /// <param name="choices"> Best to think of this as Dictionary<string, (int)enum> </param>
    /// <returns>integer value. </returns>
    public static int PromptUserForIndexSelection(string promptText, Dictionary<string, int> choices)
    {
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title(promptText)
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to see additional options)[/]")
            .AddChoices(choices.Keys.ToArray()));

        return choices[selection];
    }

    public static void DisplayListAsTable(string[] columns, List<string[]> rows)
    {
        var table = new Table();

        table.AddColumns(columns);

        foreach (var row in rows)
        {
            table.AddRow(row);
        }

        AnsiConsole.Write(table);
    }

    public static string PromptUserForStringInput(string promptText)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>(promptText)
            .Validate(n =>
            {
                if (string.IsNullOrEmpty(n))
                {
                    return ValidationResult.Error();
                }
                else
                {
                    return ValidationResult.Success();
                }
            }));

        return input;
    }

    public static int PromptUserForIntegerInput(string prompt, int min, int max)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<int>(prompt)
            .Validate(n =>
            {
                if (n >= max || n < min) { return ValidationResult.Error("Invalid input entered."); }
                else { return ValidationResult.Success(); }
            }));

        return input;
    }
}
