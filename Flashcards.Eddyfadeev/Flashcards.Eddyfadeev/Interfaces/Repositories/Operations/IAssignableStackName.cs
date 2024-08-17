namespace Flashcards.Eddyfadeev.Interfaces.Repositories.Operations;

/// <summary>
/// Represents an interface for objects that can have an assignable stack name.
/// </summary>
internal interface IAssignableStackName
{
    /// <summary>
    /// Gets or sets the stack name.
    /// </summary>
    /// <value>
    /// The stack name.
    /// </value>
    internal string? StackName { get; set; }
}