using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;
using Flashcards.Eddyfadeev.Models.Entity;
using Flashcards.Eddyfadeev.Services;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.View.Commands.StacksMenu;

/// <summary>
/// Represents a command for adding a new stack.
/// </summary>
internal sealed class AddStack : ICommand
{
    private readonly IStacksRepository _stacksRepository;

    public AddStack(IStacksRepository stacksRepository)
    {
        _stacksRepository = stacksRepository;
    }

    public void Execute()
    {
        var stack = new Stack();
        var stackName = AnsiConsole.Ask<string>(Messages.Messages.EnterNameMessage);

        stack.Name = stackName;

        var result = _stacksRepository.Insert(stack);

        AnsiConsole.MarkupLine(
            result > 0 ? 
                Messages.Messages.AddSuccessMessage : 
                Messages.Messages.AddFailedMessage
        );
        
        GeneralHelperService.ShowContinueMessage();
    }
}