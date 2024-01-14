using ConsoleTableExt;
using Flashcards.DataAccess;
using Flashcards.DataAccess.DTOs;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class StudyStackScreen
{
    internal static Screen Get(IDataAccess dataAccess, int stackId, string stackName)
    {
        string header = $"Studying Stack: {stackName}";
        var flashcards = dataAccess.GetFlashcardListAsync(stackId).Result ?? new();
        Random rnd = new();
        ExistingFlashcard? currentFlashcard = null;
        if (flashcards.Count > 0)
        {
            currentFlashcard = flashcards[rnd.Next(flashcards.Count)];
        }
        string? userAnswer = null;

        NewHistory sessionHistory = new() { StackId = stackId, StartedAt = DateTime.UtcNow };

        var screen = new Screen(header: (_, _) => header,
            body: (usableWidth, _) =>
            {
                int maxWidth = usableWidth - 2;
                if (currentFlashcard != null)
                {
                    var tableData = new List<List<object>>
                    {
                        // TODO: ConsoleTableExt doesn't support multiline cells.
                        new List<object>() { TCSAHelper.General.Utils.LimitWidth(currentFlashcard.Front.Replace("\\n", "\n"), maxWidth) }
                    };
                    string prompt;
                    if (userAnswer == null)
                    {
                        tableData.Add(new List<object>() { "???" });
                        prompt = "\n\nEnter your answer:\n  ";
                    }
                    else
                    {
                        // TODO: ConsoleTableExt doesn't support multiline cells.
                        tableData.Add(new List<object>() { TCSAHelper.General.Utils.LimitWidth(currentFlashcard.Back.Replace("\\n", "\n"), maxWidth) });
                        if (userAnswer == currentFlashcard.Back)
                        {
                            prompt = "\n\nYou were correct!";
                        }
                        else
                        {
                            prompt = "\n\nBetter luck next time!";
                        }
                    }
                    return ConsoleTableBuilder.From(tableData)
                        .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
                        .Export()
                        + prompt;
                }
                else
                {
                    return "There are no cards to study in this stack.";
                }
            },
            footer: (_, _) =>
            {
                if (userAnswer == null)
                {
                    return "Press [Esc] to go back to the stack.";
                }
                else
                {
                    return "Press [Esc] to go back to the stack,\nor any other key to go to the next question.";
                }
            });

        void CheckAnswer(string userInput)
        {
            // TODO: Option for case-sensitive answers?
            userAnswer = userInput.Trim().ToLower();
            if (currentFlashcard != null)
            {
                var solution = currentFlashcard.Back.Trim().ToLower();
                var result = new NewStudyResult() { FlashcardId = currentFlashcard.Id, AnsweredAt = DateTime.UtcNow, WasCorrect = userAnswer == solution };
                sessionHistory.Results.Add(result);
            }
            screen.SetPromptAction(null);
        }
        screen.SetPromptAction(CheckAnswer);
        screen.SetAnyKeyAction(() =>
        {
            if (userAnswer != null)
            {
                var nextFlashcard = flashcards[rnd.Next(flashcards.Count)];
                if (flashcards.Count > 1)
                {
                    while (nextFlashcard == currentFlashcard)
                    {
                        nextFlashcard = flashcards[rnd.Next(flashcards.Count)];
                    }
                }
                currentFlashcard = nextFlashcard;
                userAnswer = null;
                screen.SetPromptAction(CheckAnswer);
            }
        });

        screen.AddAction(ConsoleKey.Escape, () =>
        {
            if (sessionHistory.Results.Count > 0)
            {
                dataAccess.AddHistoryAsync(sessionHistory).Wait();
            }
            screen.ExitScreen();
        });

        return screen;
    }
}
