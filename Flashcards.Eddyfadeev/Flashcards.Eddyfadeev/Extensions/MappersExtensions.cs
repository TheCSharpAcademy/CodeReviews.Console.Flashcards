using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Models.Dto;
using Flashcards.Eddyfadeev.Models.Entity;

namespace Flashcards.Eddyfadeev.Extensions;

/// <summary>
/// Contains extension methods for mapping between DTOs and entities.
/// </summary>
internal static class MappersExtensions
{
    /// <summary>
    /// Converts an instance of <see cref="IFlashcard"/> to an instance of <see cref="FlashcardDto"/>.
    /// </summary>
    /// <param name="flashcard">The <see cref="IFlashcard"/> instance to convert.</param>
    /// <returns>An instance of <see cref="FlashcardDto"/> representing the converted <see cref="IFlashcard"/>.</returns>
    public static FlashcardDto ToDto(this IFlashcard flashcard) =>
        new()
        {
            Id = flashcard.Id,
            Question = flashcard.Question,
            Answer = flashcard.Answer,
            StackId = flashcard.StackId
        };

    /// <summary>
    /// Converts an instance of <see cref="IFlashcard"/> to an instance of <see cref="FlashcardDto"/>.
    /// </summary>
    /// <param name="flashcard">The <see cref="IFlashcard"/> to convert.</param>
    /// <returns>An instance of <see cref="FlashcardDto"/> representing the converted <see cref="IFlashcard"/>.</returns>
    public static StackDto ToDto(this IStack stack) =>
        new()
        {
            Id = stack.Id,
            Name = stack.Name
        };

    /// <summary>
    /// Converts an object that implements the IStudySession interface to a StudySessionDto object.
    /// </summary>
    /// <param name="studySession">The study session object to convert.</param>
    /// <returns>A StudySessionDto object converted from the IStudySession object.</returns>
    public static StudySessionDto ToDto(this IStudySession studySession) =>
        new()
        {
            Id = studySession.Id,
            StackId = studySession.StackId,
            Date = studySession.Date,
            Questions = studySession.Questions,
            CorrectAnswers = studySession.CorrectAnswers,
            Percentage = studySession.Percentage,
            Time = studySession.Time,
            StackName = studySession.StackName
        };

    /// <summary>
    /// Converts an instance of <see cref="IFlashcard"/> to an instance of <see cref="Flashcard"/>.
    /// </summary>
    /// <param name="flashcard">The <see cref="IFlashcard"/> instance to convert.</param>
    /// <returns>An instance of <see cref="Flashcard"/>.</returns>
    public static Flashcard ToEntity(this IFlashcard flashcard) =>
        new()
        {
            Id = flashcard.Id,
            Question = flashcard.Question,
            Answer = flashcard.Answer,
            StackId = flashcard.StackId
        };

    /// Converts an object that implements the IStack interface to an instance of the Stack entity.
    /// @param stack The object that implements the IStack interface.
    /// @returns An instance of the Stack entity.
    /// /
    public static Stack ToEntity(this IStack stack) =>
        new()
        {
            Id = stack.Id,
            Name = stack.Name
        };

    /// <summary>
    /// Converts an instance of <see cref="IStudySession"/> to an instance of <see cref="StudySession"/> entity.
    /// </summary>
    /// <param name="studySession">The study session to convert.</param>
    /// <returns>The converted <see cref="StudySession"/> entity.</returns>
    public static StudySession ToEntity(this IStudySession studySession) =>
        new()
        {
            Id = studySession.Id,
            StackId = studySession.StackId,
            Date = studySession.Date,
            Questions = studySession.Questions,
            CorrectAnswers = studySession.CorrectAnswers,
            Percentage = studySession.Percentage,
            Time = studySession.Time,
            StackName = studySession.StackName
        };
}