using Flashcards.Core;
using Flashcards.DataAccess;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class MoveFlashcardScreen
{
    public static Screen Get(IDataAccess dataAccess, int flashcardId)
    {
        var card = dataAccess.GetFlashcardByIdAsync(flashcardId).Result;
        var stack = dataAccess.GetStackListItemByFlashcardIdAsync(flashcardId).Result;
        string error = string.Empty;

        Screen screen = new(header: (_, _) => "Move Flashcard", body: (_, _) => $"{error}The card is currently in the \"{stack.ViewName}\" stack.\n\nEnter another stack's name: ", footer: (_, _) => "Press [Esc] to cancel.");
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);
        screen.SetPromptAction((userInput) =>
        {
            var otherStackName = StackManagement.CreateSortName(userInput);
            var otherStack = dataAccess.GetStackListItemBySortNameAsync(otherStackName).Result;
            if (string.IsNullOrEmpty(userInput))
            {
                error = "Enter a stack name.\n\n";
            }
            else if (otherStack == null)
            {
                error = "There is no stack with that name.\n\n";
            }
            else if (dataAccess.CardInStack(otherStack.Id, flashcardId).Result)
            {
                error = "The card is already in that stack.\n\n";
            }
            else
            {
                dataAccess.MoveFlashcardAsync(flashcardId, otherStack.Id).Wait();
                screen.ExitScreen();
            }
        });

        return screen;
    }
}
