using Spectre.Console;

namespace FlashardsUI;
internal class UserInput
{
    internal static string StringPrompt(string prompt)
    {
        return AnsiConsole.Ask<string>(prompt);
    }

    internal static string StringPromptAllowEmpty(string prompt)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
            .AllowEmpty());
    }

    internal static T EnumPrompt<T>(string title) where T : notnull
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<T>()
                .Title(title)
                .PageSize(10)
                .AddChoices(Enum.GetValues(typeof(T)) as T[] ?? throw new InvalidOperationException("Failed to retrieve enum values!")));
    }
}