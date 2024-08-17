using Flashcards.Eddyfadeev.Interfaces.Repositories.Operations;

namespace Flashcards.Eddyfadeev.Interfaces.Models;

/// <summary>
/// Represents a flashcard with a question and an answer.
/// </summary>
internal interface IFlashcard : IAssignableStackId
{
    /// <summary>
    /// Gets or sets the unique identifier of the flashcard.
    /// </summary>
    internal int Id { get; set; }

    /// <summary>
    /// Represents a flashcard.
    /// </summary>
    internal string? Question { get; set; }

    /// <summary>
    /// Represents the answer property of a flashcard.
    /// </summary>
    /// <remarks>
    /// The answer property holds the answer to the question on a flashcard.
    /// </remarks>
    internal string? Answer { get; set; }
}