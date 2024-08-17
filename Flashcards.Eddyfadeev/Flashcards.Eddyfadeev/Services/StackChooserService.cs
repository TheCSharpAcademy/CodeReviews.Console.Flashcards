using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Interfaces.View.Factory;

namespace Flashcards.Eddyfadeev.Services;

/// <summary>
/// Represents a service for choosing a stack from a menu.
/// </summary>
internal abstract class StackChooserService
{
    /// <summary>
    /// Gets the selected stack from the stack repository.
    /// </summary>
    /// <param name="menuCommandFactory">The menu command factory.</param>
    /// <param name="stacksRepository">The stacks repository.</param>
    /// <returns>The selected stack from the stack repository.</returns>
    internal static IStack GetStacks(
        IMenuCommandFactory<StackMenuEntries> menuCommandFactory,
        IStacksRepository stacksRepository)
    {
        var chooseCommand = menuCommandFactory.Create(StackMenuEntries.ChooseStack);
        chooseCommand.Execute();

        return stacksRepository.SelectedEntry!;
    }
}