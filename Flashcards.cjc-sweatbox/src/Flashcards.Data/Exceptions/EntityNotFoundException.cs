namespace Flashcards.Data.Exceptions;

/// <summary>
/// Custom exception for when an entity that is expected to exist is not returned from the database.
/// </summary>
internal class EntityNotFoundException : Exception
{
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string? message) : base(message)
    {
    }

    public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
