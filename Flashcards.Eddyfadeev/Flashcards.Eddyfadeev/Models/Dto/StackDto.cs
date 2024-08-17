using Flashcards.Interfaces.Models;

namespace Flashcards.Models.Dto;

/// <summary>
/// Represents a data transfer object for a stack.
/// </summary>
public record StackDto : IStack
{
    public int Id { get; init; }
    public string? Name { get; set; }
}