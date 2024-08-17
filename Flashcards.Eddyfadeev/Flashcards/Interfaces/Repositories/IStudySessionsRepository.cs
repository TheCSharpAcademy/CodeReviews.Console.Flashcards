using Flashcards.Interfaces.Models;
using Flashcards.Interfaces.Repositories.Operations;

namespace Flashcards.Interfaces.Repositories;

/// <summary>
/// Represents a repository for study sessions.
/// </summary>
internal interface IStudySessionsRepository :
    IInsertIntoRepository<IStudySession>,
    IGetAllFromRepository<IStudySession>,
    ISelectableRepositoryEntry<IStudySession>,
    IAssignableStackId,
    IAssignableStackName;