using Flashcards.Eddyfadeev.Interfaces.Repositories.Operations;

namespace Flashcards.Eddyfadeev.Interfaces.Models;

/// <summary>
/// Represents a study session.
/// </summary>
internal interface IStudySession : IAssignableStackId, IAssignableStackName
{
    internal int Id { get; set; }

    /// <summary>
    /// Represents a date associated with a study session.
    /// </summary>
    internal DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the number of questions in the study session.
    /// </summary>
    internal int Questions { get; set; }

    /// <summary>
    /// Gets or sets the number of correct answers in a study session.
    /// </summary>
    internal int CorrectAnswers { get; set; }

    /// <summary>
    /// Represents the percentage of correct answers in a study session.
    /// </summary>
    internal int Percentage { get; set; }

    /// <summary>
    /// Represents a study session.
    /// </summary>
    internal TimeSpan Time { get; set; }
}