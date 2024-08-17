using Flashcards.Interfaces.Models;
using Flashcards.Interfaces.Repositories.Operations;

namespace Flashcards.Interfaces.Repositories;

/// <summary>
/// Represents a repository for managing stacks.
/// </summary>
internal interface IStacksRepository : 
    IInsertIntoRepository<IStack>,
    IGetAllFromRepository<IStack>,
    ISelectableRepositoryEntry<IStack>,
    IDeleteFromRepository,
    IUpdateInRepository;