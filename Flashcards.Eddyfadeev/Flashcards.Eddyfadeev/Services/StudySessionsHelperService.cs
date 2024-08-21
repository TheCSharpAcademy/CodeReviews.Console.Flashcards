using Flashcards.Eddyfadeev.Extensions;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
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
            }
            else
            {
                AnsiConsole.MarkupLine($"{ Messages.Messages.IncorrectAnswerMessage } Correct answer is { flashcard.Answer }\n");
            }

            GeneralHelperService.ShowContinueMessage();
        }
        AnsiConsole.MarkupLine($"[white]You have { correctAnswers } out of { flashcards.Count }.[/]");
        
        GeneralHelperService.ShowContinueMessage();
        
        return correctAnswers;
    }

    /// <summary>
    /// Retrieves the chosen year from the user.
    /// </summary>
    /// <param name="studySessionsRepository">The study sessions repository.</param>
    /// <param name="yearEntryHandler">The handler for user input of year.</param>
    /// <returns>The chosen year as an instance of IYear.</returns>
    internal static IYear GetYearFromUser(
        IStudySessionsRepository studySessionsRepository, 
        IEditableEntryHandler<IYear> yearEntryHandler)
    {
        var years = studySessionsRepository.GetYears().ToList();
        
        if (years.Count == 0)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoEntriesFoundMessage);
            GeneralHelperService.ShowContinueMessage();
            return new Year();
        }
        
        var userChoice = yearEntryHandler.HandleEditableEntry(years)?.ToEntity();
        
        if (userChoice is null)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoYearsFoundMessage);
            GeneralHelperService.ShowContinueMessage();
            return new Year();
        }
        
        return userChoice;
    }
}