using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.Handlers;

/// <summary>
/// Represents a class that handles editable entries.
/// </summary>
/// <typeparam name="TEntry">The type of the entry.</typeparam>
internal class EditableEntryHandler<TEntry> : IEditableEntryHandler<TEntry> where TEntry : class
{
    /// <summary>
    /// Handles an editable entry by presenting a list of entries and prompting the user to choose one.
    /// </summary>
    /// <typeparam name="TEntry">The type of entry.</typeparam>
    /// <param name="entries">The list of entries to choose from.</param>
    /// <returns>The selected entry, or null if no entry is selected.</returns>
    public TEntry? HandleEditableEntry(List<TEntry> entries)
    {
        if (entries.Count == 0)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoEntriesFoundMessage);
            return default;
        }
        
        var entriesNames = entries.ConvertAll(entry => entry.ToString());
        
        var userChoice = AnsiConsole.Prompt(GetUserChoice(entriesNames!));

        return entries.Find(entry => entry.ToString() == userChoice);
    }


    private static SelectionPrompt<string> GetUserChoice(List<string> entriesNames) =>
        new SelectionPrompt<string>()
            .Title(Messages.Messages.ChooseEntryMessage)
            .AddChoices(entriesNames);
}