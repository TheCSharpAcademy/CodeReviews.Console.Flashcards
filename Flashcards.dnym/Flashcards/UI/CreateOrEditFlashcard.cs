using Flashcards.DataAccess;
using Flashcards.DataAccess.DTOs;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class CreateOrEditFlashcard
{
    public static Screen Get(IDataAccess dataAccess, int stackId, int? flashcardId = null)
    {
        string? front = null;
        string? back = null;
        var stack = dataAccess.GetStackListItemByIdAsync(stackId).Result;
        ExistingFlashcard? card = null;
        if (flashcardId != null)
        {
            card = dataAccess.GetFlashcardByIdAsync((int)flashcardId).Result;
        }
        int cardsCreated = 0;

        Screen screen = new(header: (_, _) =>
        {
            if (card == null)
            {
                return $"Creating Flashcards in Stack: {stack.ViewName}";
            }
            else
            {
                return $"Editing Flashcard in Stack: {stack.ViewName}";
            }
        }, body: (_, _) =>
        {
            if (string.IsNullOrEmpty(front))
            {
                return "Enter a front side question: ";
            }
            else if (string.IsNullOrEmpty(back))
            {
                return $"Enter a front side question: {front}\n\nEnter a back side answer: ";
            }
            else
            {
                return "Card updated.";
            }
        }, footer: (_, _) =>
        {
            if (flashcardId == null)
            {
                return $"{cardsCreated} cards created.\nPress [Esc] to go back to the stack.";
            }
            else if (front == null || back == null)
            {
                return "Press [Esc] to go back.";
            }
            else
            {
                return "Press any key to go back.";
            }
        });
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);

        screen.SetDefaultUserInput(card?.Front);
        screen.SetPromptAction((userInput) =>
        {
            if (!string.IsNullOrEmpty(userInput))
            {
                if (string.IsNullOrEmpty(front))
                {
                    front = userInput;
                    screen.SetDefaultUserInput(card?.Back);
                }
                else if (string.IsNullOrEmpty(back))
                {
                    back = userInput;
                }

                if (front != null && back != null)
                {
                    if (card == null)
                    {
                        var newCard = new NewFlashcard { StackId = stackId, Front = front, Back = back };
                        dataAccess.CreateFlashcardAsync(newCard).Wait();
                        cardsCreated++;
                        front = null;
                        back = null;
                    }
                    else
                    {
                        card.Front = front;
                        card.Back = back;
                        dataAccess.UpdateFlashcardAsync(card).Wait();
                        screen.SetPromptAction(null);
                        screen.SetAnyKeyAction(screen.ExitScreen);
                    }
                }
            }
            else
            {
                Console.Beep();
            }
        });

        return screen;
    }
}
