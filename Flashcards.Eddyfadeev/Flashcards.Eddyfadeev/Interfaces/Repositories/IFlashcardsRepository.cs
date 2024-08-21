using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories.Operations;

namespace Flashcards.Eddyfadeev.Interfaces.Repositories;

/// <summary>
/// Represents an interface for a flashcard repository.
/// </summary>
internal interface IFlashcardsRepository :
    IInsertIntoRepository<IFlashcard>,
    IDeleteFromRepository<IFlashcard>,
    IUpdateInRepository<IFlashcard>
{
    /// <summary>
    /// Retrieves a list of flashcards associated with a given stack from the flashcard repository.
    /// </summary>
    /// <param name="stack">The stack for which to retrieve the flashcards.</param>
    /// <returns>A collection of flashcards.</returns>
    internal IEnumerable<IFlashcard> GetFlashcards(IDbEntity<IStack> stack);
}