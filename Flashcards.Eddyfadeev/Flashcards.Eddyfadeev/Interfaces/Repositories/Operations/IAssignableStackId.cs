namespace Flashcards.Interfaces.Repositories.Operations;

/// <summary>
/// Represents an object that can be assigned a stack ID.
/// </summary>
internal interface IAssignableStackId
{
    /// <summary>
    /// Represents the ID of a stack.
    /// </summary>
    /// <value>
    /// Stack ID.
    /// </value>
    internal int? StackId { get; set; }
}