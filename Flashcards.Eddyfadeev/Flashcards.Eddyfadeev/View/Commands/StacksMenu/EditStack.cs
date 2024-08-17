using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;
using Flashcards.Eddyfadeev.Interfaces.View.Factory;
using Flashcards.Eddyfadeev.Services;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.View.Commands.StacksMenu;

/// <summary>
/// Represents a command to edit a stack.
/// </summary>
internal sealed class EditStack : ICommand
{
    private readonly IStacksRepository _stacksRepository;
    private readonly IMenuCommandFactory<StackMenuEntries> _menuCommandFactory;

    public EditStack(IStacksRepository stacksRepository, IMenuCommandFactory<StackMenuEntries> menuCommandFactory)
    {
        _stacksRepository = stacksRepository;
        _menuCommandFactory = menuCommandFactory;
    }

    public void Execute()
    {
        StackChooserService.GetStacks(_menuCommandFactory, _stacksRepository);

        var newStackName = AskNewStackName();
        
        _stacksRepository.SelectedEntry!.Name = newStackName;

        var result = _stacksRepository.Update();
        
        AnsiConsole.MarkupLine(
            result > 0 ? 
                Messages.Messages.UpdateSuccessMessage : 
                Messages.Messages.UpdateFailedMessage
        );
        GeneralHelperService.ShowContinueMessage();
    }
    
    private static string AskNewStackName()
    {
        var newStackName = AnsiConsole.Ask<string>(Messages.Messages.EnterNameMessage);

        return newStackName;
    }
}