namespace Flashcards.Eddyfadeev.Interfaces.Models;

/// <summary>
/// Represents a stack.
/// </summary>
internal interface IStack
{
    /// <summary>
    /// Gets or sets the ID of the property.
    /// </summary>
    /// <remarks>
    /// This property represents the unique identifier of the property.
    /// </remarks>
    internal int Id { get; }

    /// <summary>
    /// Represents the name property of a stack.
    /// </summary>
    internal string? Name { get; set; }
}