﻿using Flashcards.Interfaces.Handlers;
using Flashcards.Interfaces.Models;
using Flashcards.Interfaces.Repositories;
using Flashcards.Interfaces.View.Commands;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.View.Commands.StacksMenu;

/// <summary>
/// Represents a command for choosing a stack.
/// </summary>
internal sealed class ChooseStack : ICommand
{
    private readonly IStacksRepository _stacksRepository;
    private readonly IEditableEntryHandler<IStack> _editableEntryHandler;

    public ChooseStack(IStacksRepository stacksRepository, IEditableEntryHandler<IStack> editableEntryHandler)
    {
        _stacksRepository = stacksRepository;
        _editableEntryHandler = editableEntryHandler;
    }

    public void Execute()
    {
        var entries = _stacksRepository.GetAll().ToList();
        
        if (entries.Count == 0)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoStackChosenMessage);
            GeneralHelperService.ShowContinueMessage();
            return;
        }
        
        var userChoice = _editableEntryHandler.HandleEditableEntry(entries);

        if (GeneralHelperService.CheckForNull(userChoice))
        {
            return;
        }
        
        _stacksRepository.SelectedEntry = userChoice;
    }
}