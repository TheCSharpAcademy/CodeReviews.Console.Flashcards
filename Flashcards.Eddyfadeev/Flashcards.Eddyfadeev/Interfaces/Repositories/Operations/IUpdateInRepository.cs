namespace Flashcards.Eddyfadeev.Interfaces.Repositories.Operations;

/// <summary>
/// Represents an interface for updating entities in a repository.
/// </summary>
internal interface IUpdateInRepository
{
    /// <summary>
    /// Updates the selected entry in the repository.
    /// </summary>
    /// <returns>The number of affected rows.</returns>
    internal int Update();
}