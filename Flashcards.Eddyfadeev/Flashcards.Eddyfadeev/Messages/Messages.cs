namespace Flashcards.Eddyfadeev.Messages;

/// <summary>
/// Represents a class that contains messages used in the application.
/// </summary>
public static class Messages
{
    internal const string WelcomeMessage = "[white]Welcome to Flashcards![/]";
    internal const string NoStackChosenMessage = "[red]No stack was chosen.[/]";
    internal const string NullEntityMessage = "[red]Passed entity is null.[/]";
    internal const string NoFlashcardsFoundMessage = "[red]No flashcards found.[/]";
    internal const string DeleteSuccessMessage = "[green]Deleted successfully.[/]";
    internal const string DeleteFailedMessage = "[red]Error while deleting.[/]";
    internal const string DeleteConfirmationMessage = 
        "[white]Are you sure you want to delete this entry?[/] [red]This action cannot be undone![/]";
    internal const string AnyKeyToContinueMessage = "[white]Press any key to continue...[/]";
    internal const string EnterNameMessage = "[white]Please enter the name:[/]";
    internal const string EnterFlashcardQuestionMessage = "[white]Enter the question.[/]";
    internal const string EnterFlashcardAnswerMessage = "[white]Enter the answer.[/]";
    internal const string PromptArrow = "[white]> [/]";
    internal const string UpdateSuccessMessage = "[green]Update successful![/]";
    internal const string UpdateFailedMessage = "[red]Update failed![/]";
    internal const string AddSuccessMessage = "[green]Added successfully![/]";
    internal const string AddFailedMessage = "[red]Error while adding.[/]";
    internal const string NoEntriesFoundMessage = "[red]No entries found.[/]";
    internal const string ChooseEntryMessage = "[white]Choose an entry:[/]";
    internal const string WhatToDoMessage = "[white]What would you like to do?[/]";
    internal const string CorrectAnswerMessage = "[green]Correct![/]";
    internal const string IncorrectAnswerMessage = "[red]Incorrect![/]";
}