namespace Flashcards.Exceptions;

/// <summary>
/// Represents an exception thrown to return to the main menu.
/// </summary>
public class ReturnToMainMenuException : Exception
{
    public ReturnToMainMenuException() : base("Returning to the main menu...")
    {
    }
    
    public ReturnToMainMenuException(string message) : base(message)
    {
    }
    
    public ReturnToMainMenuException(string message, Exception innerException) : base(message, innerException)
    {
    }
}