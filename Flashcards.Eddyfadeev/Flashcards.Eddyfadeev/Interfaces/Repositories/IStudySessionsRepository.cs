using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories.Operations;

namespace Flashcards.Eddyfadeev.Interfaces.Repositories;

/// <summary>
/// Represents a repository for study sessions.
/// </summary>
internal interface IStudySessionsRepository :
    IInsertIntoRepository<IStudySession>,
    IGetAllFromRepository<IStudySession>,
    ISelectableRepositoryEntry<IStudySession>,
    IAssignableStackId,
    IAssignableStackName;