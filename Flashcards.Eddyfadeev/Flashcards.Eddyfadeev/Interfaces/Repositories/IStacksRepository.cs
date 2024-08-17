using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories.Operations;

namespace Flashcards.Eddyfadeev.Interfaces.Repositories;

/// <summary>
/// Represents a repository for managing stacks.
/// </summary>
internal interface IStacksRepository : 
    IInsertIntoRepository<IStack>,
    IGetAllFromRepository<IStack>,
    ISelectableRepositoryEntry<IStack>,
    IDeleteFromRepository,
    IUpdateInRepository;