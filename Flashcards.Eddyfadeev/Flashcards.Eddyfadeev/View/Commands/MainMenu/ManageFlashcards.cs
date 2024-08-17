using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;

namespace Flashcards.Eddyfadeev.View.Commands.MainMenu;

/// <summary>
/// Represents a command to manage flashcards.
/// </summary>
internal sealed class ManageFlashcards : ICommand
{
    private readonly IMenuHandler<FlashcardEntries> _flashcardsMenuHandler;

    public ManageFlashcards(IMenuHandler<FlashcardEntries> flashcardsMenuHandler)
    {
        _flashcardsMenuHandler = flashcardsMenuHandler;
    }

    public void Execute()
    {
        _flashcardsMenuHandler.HandleMenu();
    }
}