using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;

namespace Flashcards.Eddyfadeev.View.Commands.MainMenu;

/// <summary>
/// Represents a command for managing stacks.
/// </summary>
internal sealed class ManageStacks : ICommand
{
    private readonly IMenuHandler<StackMenuEntries> _stacksMenuHandler;

    public ManageStacks(IMenuHandler<StackMenuEntries> stacksMenuHandler)
    {
        _stacksMenuHandler = stacksMenuHandler;
    }

    public void Execute()
    {
        _stacksMenuHandler.HandleMenu();
    }
}