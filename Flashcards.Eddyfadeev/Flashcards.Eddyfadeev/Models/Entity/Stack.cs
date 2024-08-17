using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Extensions;

namespace Flashcards.Eddyfadeev.Models.Entity;

/// <summary>
/// Represents a stack.
/// </summary>
internal class Stack : IStack, IDbEntity<IStack>
{
    public int Id { get; init; }
    public string? Name { get; set; }

    /// <summary>
    /// Maps an instance of <see cref="Stack"/> to an instance of <see cref="StackDto"/>.
    /// </summary>
    /// <param name="stack">The instance of <see cref="Stack"/> to convert.</param>
    /// <returns>An instance of <see cref="StackDto"/> representing the converted <see cref="Stack"/>.</returns>
    public IStack MapToDto() => this.ToDto();
    public override string ToString() => Name;
}