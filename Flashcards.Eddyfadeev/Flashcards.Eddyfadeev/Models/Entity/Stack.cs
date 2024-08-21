using Flashcards.Eddyfadeev.Extensions;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Models.Dto;

namespace Flashcards.Eddyfadeev.Models.Entity;

/// <summary>
/// Represents a stack.
/// </summary>
internal class Stack : IStack, IDbEntity<StackDto>
{
    public int Id { get; init; }
    public string? Name { get; set; }

    /// <summary>
    /// Maps an instance of <see cref="Stack"/> to an instance of <see cref="StackDto"/>.
    /// </summary>
    /// <param name="stack">The instance of <see cref="Stack"/> to convert.</param>
    /// <returns>An instance of <see cref="StackDto"/> representing the converted <see cref="Stack"/>.</returns>
    public StackDto MapToDto() => this.ToDto();
    public override string ToString() => Name ?? "Unnamed Stack";
}