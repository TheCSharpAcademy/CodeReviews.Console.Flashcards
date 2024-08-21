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
    /// Converts an instance of <see cref="IStack"/> to an instance of <see cref="StackDto"/>.
    /// </summary>
    /// <param name="stack">The <see cref="IStack"/> to convert.</param>
    /// <returns>An instance of <see cref="StackDto"/> representing the converted <see cref="IStack"/>.</returns>
    public static StackDto ToDto(this Stack stack) =>
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
    /// Converts an object that implements the IYear interface to a YearDto object.
    /// </summary>
    /// <param name="year">The year object to convert.</param>
    /// <returns>A YearDto object converted from the IYear object.</returns>
    public static YearDto ToDto(this IYear year) =>
        new()
        {
            ChosenYear = year.ChosenYear
        };
    
    /// <summary>
    /// Converts an object that implements the IStackMonthlySessions interface to a StackMonthlySessionsDto object.
    /// </summary>
    /// <param name="stackMonthlySessions">The monthly sessions object to convert.</param>
    /// <returns>A StackMonthlySessionsDto object converted from the IStackMonthlySessions object.</returns>
    public static StackMonthlySessionsDto ToDto(this IStackMonthlySessions stackMonthlySessions) =>
        new()
        {
            StackName = stackMonthlySessions.StackName,
            January = stackMonthlySessions.January,
            February = stackMonthlySessions.February,
            March = stackMonthlySessions.March,
            April = stackMonthlySessions.April,
            May = stackMonthlySessions.May,
            June = stackMonthlySessions.June,
            July = stackMonthlySessions.July,
            August = stackMonthlySessions.August,
            September = stackMonthlySessions.September,
            October = stackMonthlySessions.October,
            November = stackMonthlySessions.November,
            December = stackMonthlySessions.December,
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

    /// <summary>
    /// Converts an object that implements the IStack interface to an instance of the Stack entity.
    /// </summary>
    /// <param name="stack"> The <see cref="IStack"/> instance to convert </param>
    /// <returns> An instance of <see cref="Stack"/>.</returns>
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
    
    /// <summary>
    /// Converts an instance of <see cref="IYear"/> to an instance of <see cref="Year"/> entity.
    /// </summary>
    /// <param name="year">The year to convert.</param>
    /// <returns>The converted <see cref="Year"/> entity.</returns>
    public static Year ToEntity(this IYear year) =>
        new()
        {
            ChosenYear = year.ChosenYear
        };
    
    /// <summary>
    /// Converts an instance of <see cref="IStackMonthlySessions"/> to an instance of <see cref="StackMonthlySessions"/> entity.
    /// </summary>
    /// <param name="stackMonthlySessions">The month to convert.</param>
    /// <returns>The converted <see cref="StackMonthlySessions"/> entity.</returns>
    public static StackMonthlySessions ToEntity(this IStackMonthlySessions stackMonthlySessions) =>
        new()
        {
            StackName = stackMonthlySessions.StackName,
            January = stackMonthlySessions.January,
            February = stackMonthlySessions.February,
            March = stackMonthlySessions.March,
            April = stackMonthlySessions.April,
            May = stackMonthlySessions.May,
            June = stackMonthlySessions.June,
            July = stackMonthlySessions.July,
            August = stackMonthlySessions.August,
            September = stackMonthlySessions.September,
            October = stackMonthlySessions.October,
            November = stackMonthlySessions.November,
            December = stackMonthlySessions.December,
        };
}