using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Models.Entity;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.Services;

/// <summary>
/// The StudySessionsHelperService class provides helper methods related to study sessions.
/// </summary>
internal static class StudySessionsHelperService
{
    /// <summary>
    /// Sets the stack ID in the flashcards repository and study sessions repository.
    /// </summary>
    /// <param name="stack">The stack.</param>
    /// <param name="flashcardsRepository">The flashcards repository.</param>
    /// <param name="studySessionsRepository">The study sessions repository.</param>
    internal static void SetStackIdsInRepositories(IStack stack, IFlashcardsRepository flashcardsRepository, IStudySessionsRepository studySessionsRepository)
    {
        GeneralHelperService.SetStackIdInRepository(flashcardsRepository, stack);
        GeneralHelperService.SetStackIdInRepository(studySessionsRepository, stack);
    }

    /// <summary>
    /// Creates a new study session.
    /// </summary>
    /// <param name="flashcards">The list of flashcards to study.</param>
    /// <param name="stack">The stack to associate the study session with.</param>
    /// <returns>The newly created study session.</returns>
    internal static StudySession CreateStudySession(List<IFlashcard> flashcards, IStack stack)
    {
        return new StudySession
        {
            Questions = flashcards.Count,
            StackId = stack.Id,
            Date = DateTime.Now
        };
    }

    /// <summary>
    /// Gets the number of correct answers from a list of flashcards.
    /// </summary>
    /// <param name="flashcards">The list of flashcards to evaluate.</param>
    /// <returns>The number of correct answers.</returns>
    internal static int GetCorrectAnswers(List<IFlashcard> flashcards)
    {
        var correctAnswers = 0;
        
        foreach (var flashcard in flashcards)
        {
            Console.Clear();
            var answer = AnsiConsole.Ask<string>($"{ flashcard.Question }: ");

            while (string.IsNullOrEmpty(answer))
            {
                answer = AnsiConsole.Ask<string>($"Answer cannot be empty. { flashcard.Question }: ");
            }

            if (string.Equals(answer.Trim(), flashcard.Answer, StringComparison.OrdinalIgnoreCase))
            {
                correctAnswers++;
                AnsiConsole.MarkupLine($"{ Messages.Messages.CorrectAnswerMessage }\n");
                GeneralHelperService.ShowContinueMessage();
            }
            else
            {
                AnsiConsole.MarkupLine($"{ Messages.Messages.IncorrectAnswerMessage } Correct answer is { flashcard.Answer }\n");
                GeneralHelperService.ShowContinueMessage();
            }
        }
        AnsiConsole.MarkupLine($"[white]You have { correctAnswers } out of { flashcards.Count }.[/]");
        
        GeneralHelperService.ShowContinueMessage();
        
        return correctAnswers;
    }
}