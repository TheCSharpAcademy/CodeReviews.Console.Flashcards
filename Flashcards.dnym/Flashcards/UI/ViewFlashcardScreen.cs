using Flashcards.DataAccess;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class ViewFlashcardScreen
{
    public static Screen Get(IDataAccess dataAccess, int flashcardId)
    {
        var card = dataAccess.GetFlashcardByIdAsync(flashcardId).Result ?? throw new ArgumentException($"No flashcard with ID {flashcardId} exists.");
        var stack = dataAccess.GetStackListItemByFlashcardIdAsync(flashcardId).Result;

        Screen screen = new(header: (_, _) => $"Managing Flashcard in Stack: {stack.ViewName}", body: (_, _) => $"Front side question:\n  {card.Front}\n\nBack side answer:\n  {card.Back}", footer: (_, _) =>
        @"Press [E] to edit the flashcard, [D] to delete,
[M] to move it to a different stack,
or [Esc] to go back to the stack.");

        screen.AddAction(ConsoleKey.E, () =>
        {
            CreateOrEditFlashcard.Get(dataAccess, stack.Id, card.Id).Show();
            card = dataAccess.GetFlashcardByIdAsync(flashcardId).Result ?? throw new ArgumentException($"No flashcard with ID {flashcardId} exists.");
        });
        screen.AddAction(ConsoleKey.D, () =>
        {
            dataAccess.DeleteFlashcardAsync(card.Id).Wait();
            screen.ExitScreen();
        });
        screen.AddAction(ConsoleKey.M, () =>
        {
            MoveFlashcardScreen.Get(dataAccess, card.Id).Show();
            stack = dataAccess.GetStackListItemByFlashcardIdAsync(flashcardId).Result;
        });
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);

        return screen;
    }
}
