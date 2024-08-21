using Flashcards.Eddyfadeev.Extensions;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Models.Entity;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.Services;

/// <summary>
/// Represents a service for choosing a stack from a menu.
/// </summary>
internal abstract class StackChooserService
{
    /// <summary>
    /// Gets the selected stack from the stack repository.
    /// </summary>
    /// <param name="stackEntryHandler">Menu entry handler for stacks.</param>
    /// <param name="stacksRepository">The stacks repository.</param>
    /// <returns>The selected stack from the stack repository.</returns>
    internal static Stack GetStackFromUser(
        IStacksRepository stacksRepository,
        IEditableEntryHandler<IStack> stackEntryHandler)
    {
        var stacks = stacksRepository.GetStackNames().ToList();
        if (stacks.Count == 0)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoEntriesFoundMessage);
            GeneralHelperService.ShowContinueMessage();
            return new Stack
            {
                Name = "Error getting Stack"
            };
        }
        var userChoice = stackEntryHandler.HandleEditableEntry(stacks)?.ToEntity();
        
        if (userChoice is null)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoStackChosenMessage);
            GeneralHelperService.ShowContinueMessage();
            return new Stack
            {
                Name = "Error getting Stack"
            };
        }
        
        return userChoice;
    }
}