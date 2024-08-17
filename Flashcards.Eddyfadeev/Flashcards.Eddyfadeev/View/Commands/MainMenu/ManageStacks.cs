using Flashcards.Enums;
using Flashcards.Interfaces.Handlers;
using Flashcards.Interfaces.View.Commands;

namespace Flashcards.View.Commands.MainMenu;

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