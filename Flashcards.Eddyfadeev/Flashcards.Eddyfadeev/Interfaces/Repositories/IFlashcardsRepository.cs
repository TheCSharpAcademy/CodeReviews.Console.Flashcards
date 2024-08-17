﻿using Flashcards.Interfaces.Models;
using Flashcards.Interfaces.Repositories.Operations;

namespace Flashcards.Interfaces.Repositories;

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