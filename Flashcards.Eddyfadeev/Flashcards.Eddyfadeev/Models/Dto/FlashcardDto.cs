﻿using Flashcards.Interfaces.Models;

namespace Flashcards.Models.Dto;

/// <summary>
/// Represents a data transfer object for a flashcard.
/// </summary>
public record FlashcardDto : IFlashcard
{
    public int Id { get; set; }
    public string? Question { get; set; }
    public string? Answer { get; set; }
    public int? StackId { get; set; }
}