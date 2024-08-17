using Flashcards.Eddyfadeev.Interfaces.Models;

namespace Flashcards.Eddyfadeev.Models.Dto;

/// <summary>
/// Represents a data transfer object for a stack.
/// </summary>
public record StackDto : IStack
{
    public int Id { get; init; }
    public string? Name { get; set; }
}