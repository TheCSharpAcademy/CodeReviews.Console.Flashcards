using Flashcards.Enums;
using Flashcards.Interfaces.Repositories;
using Flashcards.Interfaces.View.Commands;
using Flashcards.Interfaces.View.Factory;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.View.Commands.StacksMenu;

/// <summary>
/// Represents a command for deleting a stack.
/// </summary>
internal sealed class DeleteStack : ICommand
{
    private readonly IStacksRepository _stacksRepository;
    private readonly IMenuCommandFactory<StackMenuEntries> _menuCommandFactory;

    public DeleteStack(IStacksRepository stacksRepository, IMenuCommandFactory<StackMenuEntries> menuCommandFactory)
    {
        _stacksRepository = stacksRepository;
        _menuCommandFactory = menuCommandFactory;
    }

    public void Execute()
    {
        StackChooserService.GetStacks(_menuCommandFactory, _stacksRepository);

        var stack = _stacksRepository.SelectedEntry;

        if (GeneralHelperService.CheckForNull(stack))
        {
            return;
        }

        var confirmation = GeneralHelperService.AskForConfirmation();
        
        if (!confirmation)
        {
            return;
        }
        
        var result = _stacksRepository.Delete();

        AnsiConsole.MarkupLine(
            result > 0 ? 
                Messages.Messages.DeleteSuccessMessage : 
                Messages.Messages.DeleteFailedMessage
            );
        GeneralHelperService.ShowContinueMessage();
    }
}