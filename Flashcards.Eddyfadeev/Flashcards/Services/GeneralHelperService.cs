using Flashcards.Interfaces.Models;
using Flashcards.Interfaces.Repositories.Operations;
using Spectre.Console;

namespace Flashcards.Services;

/// <summary>
/// The GeneralHelperService class provides general helper methods for various operations.
/// </summary>
internal static class GeneralHelperService
{
    /// <summary>
    /// Asks the user for confirmation.
    /// </summary>
    /// <returns>A boolean value indicating the user's confirmation.</returns>
    internal static bool AskForConfirmation()
    {
        var userChoice = AnsiConsole.Confirm(Messages.Messages.DeleteConfirmationMessage);
        
        return userChoice;
    }

    /// <summary>
    /// Displays the "Press any key to continue..." message and waits for user input.
    /// </summary>
    internal static void ShowContinueMessage()
    {
        AnsiConsole.MarkupLine(Messages.Messages.AnyKeyToContinueMessage);
        Console.ReadKey();
    }

    /// <summary>
    /// Sets the stack ID in the specified repository.
    /// </summary>
    /// <param name="repository">The repository that implements the <see cref="IAssignableStackId"/> interface.</param>
    /// <param name="stack">The stack object from which the stack ID will be assigned.</param>
    internal static void SetStackIdInRepository(IAssignableStackId repository, IStack stack)
    {
        if (CheckForNull(stack))
        {
            return;
        }
        
        repository.StackId = stack.Id;
    }

    /// <summary>
    /// Sets the stack name in the repository.
    /// </summary>
    /// <param name="repository">The repository with an assignable stack name.</param>
    /// <param name="stack">The stack containing the name.</param>
    internal static void SetStackNameInRepository(IAssignableStackName repository, IStack stack)
    {
        if (CheckForNull(stack))
        {
            return;
        }
        
        repository.StackName = stack.Name;
    }

    /// <summary>
    /// Checks if the specified entity is null and displays a message if it is.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to check.</typeparam>
    /// <param name="entity">The entity to check.</param>
    /// <returns>True if the entity is null, false otherwise.</returns>
    internal static bool CheckForNull<TEntity>(TEntity? entity) where TEntity : class
    {
        if (entity is not null)
        {
            return false;
        }
        
        AnsiConsole.MarkupLine($"{ Messages.Messages.NullEntityMessage }: { entity }");
        ShowContinueMessage();
        
        return true;
    }
}