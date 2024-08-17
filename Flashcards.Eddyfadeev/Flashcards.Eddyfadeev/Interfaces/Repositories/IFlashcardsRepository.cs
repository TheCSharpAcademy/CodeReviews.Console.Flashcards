using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories.Operations;

namespace Flashcards.Eddyfadeev.Interfaces.Repositories;

/// <summary>
/// Represents an interface for a flashcard repository.
/// </summary>
internal interface IFlashcardsRepository : 
    IInsertIntoRepository<IFlashcard>,
    IGetAllFromRepository<IFlashcard>,
    IDeleteFromRepository,
    IUpdateInRepository,
    ISelectableRepositoryEntry<IFlashcard>,
    IAssignableStackId,
    IAssignableStackName;