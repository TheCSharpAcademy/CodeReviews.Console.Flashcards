namespace Flashcards.Views;

public class BaseView
{
    public void ShowMessage(string message)
    {
        AnsiConsole.MarkupLine(message);
        Console.ReadKey();
    }

    public void ShowSuccess(string message)
    {
        ShowMessage($"[green]{message}[/]");
    }

    public void ShowError(string message)
    {
        ShowMessage($"[red]{message}[/]");
    }

    public bool AskConfirm(string message)
    {
        return AnsiConsole.Confirm(message);
    }

    public T AskInput<T>(string prompt, T? defaultValue = default, Func<T, bool>? validator = null,
        string? errorMessage = null)
    {
        var textPrompt = new TextPrompt<T>(prompt);

        if (defaultValue != null)
            textPrompt.DefaultValue(defaultValue);
        if (validator != null)
            textPrompt.Validate(validator,
                string.IsNullOrEmpty(errorMessage) ? textPrompt.ValidationErrorMessage : errorMessage);

        return AnsiConsole.Prompt(textPrompt);
    }

    public T ShowMenu<T>(IEnumerable<T> menuOptions, string title = "Select a menu option:", int pageSize = 10,
        Func<T, string>? converter = null)
        where T : notnull
    {
        AnsiConsole.Clear();

        var prompt = new SelectionPrompt<T>
        {
            Title = title,
            PageSize = pageSize
        };
        prompt.AddChoices(menuOptions);

        if (converter != null) prompt.UseConverter(converter);

        return AnsiConsole.Prompt(prompt);
    }

    public void Test()
    {
    }
}