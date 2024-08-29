using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Exceptions;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;
using Flashcards.Eddyfadeev.Interfaces.View.Factory;
using Flashcards.Eddyfadeev.View.Commands.StacksMenu;

namespace Flashcards.Eddyfadeev.View.Factory.EntriesInitializers;

/// <summary>
/// Initializes the menu entries for the stack-related menu.
/// </summary>
internal class StacksMenuEntriesInitializer : IMenuEntriesInitializer<StackMenuEntries>
{
    private readonly IStacksRepository _stacksRepository;
    private readonly IEditableEntryHandler<IStack> _stackEntryHandler;

    public StacksMenuEntriesInitializer(
        IStacksRepository stacksRepository,
        IEditableEntryHandler<IStack> stackEntryHandler
        )
    {
        _stacksRepository = stacksRepository;
        _stackEntryHandler = stackEntryHandler;
    }

    public Dictionary<StackMenuEntries, Func<ICommand>> InitializeEntries() =>
        new()
        {
            { StackMenuEntries.AddStack, () => new AddStack(_stacksRepository) },
            { StackMenuEntries.DeleteStack, () => new DeleteStack(_stacksRepository, _stackEntryHandler) },
            { StackMenuEntries.EditStack, () => new EditStack(_stacksRepository, _stackEntryHandler) },
            { StackMenuEntries.ChooseStack, () => new ChooseStack(_stacksRepository, _stackEntryHandler) },
            { StackMenuEntries.ReturnToMainMenu, () => throw new ReturnToMainMenuException() }
        };
}