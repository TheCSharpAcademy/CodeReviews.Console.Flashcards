using Flashcards.Models;
using Flashcards.Models.Dtos;
using Spectre.Console;

namespace Flashcards.Views.utils;

public class UserInput
{
    public string GetText(string message)
    {
        var userInput = AnsiConsole.Prompt(
            new TextPrompt<string>(message));

        return userInput;
    }

    public string GetText(string message, string defaultValue)
    {
        var userInput = AnsiConsole.Prompt(
            new TextPrompt<string>(message).DefaultValue(defaultValue));

        return userInput;
    }

    public T GetExistingEntity<T>(List<T> entities, string message) where T : BaseEntity
    {
        var selectedIndex = GetNumber(message);
        while (selectedIndex < 0 || selectedIndex >= entities.Count)
        {
            AnsiConsole.MarkupLine("[red]Please enter a valid index[/]");
            selectedIndex = GetNumber(message);
        }

        return entities[selectedIndex];
    }

    private int GetNumber(string message)
    {
        int formattedInput;

        while (!Int32.TryParse(AnsiConsole.Prompt(new TextPrompt<string>(message)), out formattedInput))
        {
            AnsiConsole.MarkupLine("[red]Please enter integer[/]");
        }

        return formattedInput;
    }
}