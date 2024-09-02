using FlashcardsLibrary.Models;
using FlashcardsLibrary.Views;

namespace FlashcardsLibrary.Controllers;

internal class FlashcardManager
{
    private readonly string topic;
    public FlashcardManager(string topic)
    {
        this.topic = topic;
    }

    internal void ViewAllFlashCardsInStack()
    {
        DataViewer.DisplayListAsTable<Flashcard>
        (
            Flashcard.Headers, 
            Utilities.databaseManager.GetAllDataWithTopic<Flashcard>(Utilities.FlashcardTableName,this.topic)
        );
    }

    internal void ViewXFlashCardsInStack()
    {
        List<Flashcard> data = Utilities.databaseManager.GetAllDataWithTopic<Flashcard>(Utilities.FlashcardTableName,this.topic);
        int amountShown = Utilities.GetIntegerInput
                            (
                                message: "Enter the amount of Flashcards you wish to be shown\n> ",
                                errorMessage: $"Please choose a valid number [0 -> {data.Count}]\n> ",
                                lowerRange: 0,
                                maxRange: data.Count
                            );
        
        data.RemoveRange(amountShown > 0 ? amountShown - 1 : 0, data.Count - amountShown);
        DataViewer.DisplayListAsTable<Flashcard>
        (
            Flashcard.Headers, 
            data
        );
    }

    internal void CreateFlashCardInStack()
    {
        string front = 
            Utilities.GetStringInput
            (
                message: "Enter the Front of the card\n> ",
                errorMessage: "Cannot be null or emtpy\n> "
            );
        
        string back = 
            Utilities.GetStringInput
            (
                message: "Enter the Back of the card\n> ",
                errorMessage: "Cannot be null or emtpy\n> "
            );

        Utilities.databaseManager.CreateFlashcardInStack(front, back, this.topic);
    }

    internal void EditFlashCard()
    {
        if(Utilities.databaseManager.GetNumberOfEntriesFromFlashcards() == 0)
        {
            System.Console.WriteLine("There are no entries to edit");
            return;
        }
        
        DataViewer.DisplayHeader("Current Flashcards", "left");
        this.ViewAllFlashCardsInStack();
        
        int id = 
            Utilities.GetIntegerInput
            (
                message: "Enter the id of the flashcard\n> ",
                errorMessage: "Invalid ID\n> ",
                Condition: Utilities.databaseManager.IDExistsFlashcard
            );

        string front = 
            Utilities.GetStringInput
            (
                message: "Enter the Updated Front of the card\n> ",
                errorMessage: "Cannot be null or emtpy\n> "
            );
        
        string back = 
            Utilities.GetStringInput
            (
                message: "Enter the Updated Back of the card\n> ",
                errorMessage: "Cannot be null or emtpy\n> "
            );

        Utilities.databaseManager.UpdateFlashCard(id, front, back, this.topic);
    }

    internal void DeleteFlashCard()
    {
        if(Utilities.databaseManager.GetNumberOfEntriesFromFlashcards() == 0)
        {
            System.Console.WriteLine("There are no entries to edit");
            return;
        }

        DataViewer.DisplayHeader("Current Flashcards", "left");
        this.ViewAllFlashCardsInStack();
        
        int id = 
            Utilities.GetIntegerInput
            (
                message: "Enter the id of the flashcard\n> ",
                errorMessage: "Invalid ID\n> ",
                Condition: Utilities.databaseManager.IDExistsFlashcard
            );
        
        Utilities.databaseManager.DeleteFlashcard(id);
    }

    internal StackSelections DisplayFlashcardMenu()
    {
        System.Console.Clear();
        DataViewer.DisplayHeader("Flashcard Modification");
        System.Console.WriteLine("\nCurrent Stack: {0}\n", Utilities.currentStack);
        return  
            Utilities.GetSelection<StackSelections>
            (
                enumerationValues: [StackSelections.exit, StackSelections.ViewAllFlashCardsInStack, 
                                    StackSelections.ViewXFlashCardsInStack, StackSelections.CreateFlashCardInStack,
                                    StackSelections.EditFlashCard, StackSelections.DeleteFlashCard],
                title: "[green]Select Your Option[/]",
                alternateNames: item => item switch 
                {
                    StackSelections.DeleteFlashCard => "Delete a FlashCard",
                    StackSelections.ViewXFlashCardsInStack => "View X FlashCards in stack",
                    StackSelections.EditFlashCard => "Edit a FlashCard",
                    StackSelections.ViewAllFlashCardsInStack => "View all Flashcards in stack",
                    StackSelections.CreateFlashCardInStack => "Create a FlashCard in current Stack",
                    _ => item.ToString()
            });
    }

    internal void HandleFlashcardSelection(StackSelections flashcardSelection)
    {
        switch (flashcardSelection)
        {
            case StackSelections.ViewAllFlashCardsInStack:
                this.ViewAllFlashCardsInStack();
                break;
            
            case StackSelections.ViewXFlashCardsInStack:
                this.ViewXFlashCardsInStack();
                break;
            
            case StackSelections.CreateFlashCardInStack:
                this.CreateFlashCardInStack();
                break;
            
            case StackSelections.EditFlashCard:
                this.EditFlashCard();
                break;
            
            case StackSelections.DeleteFlashCard:
                this.DeleteFlashCard();
                break;
            
            default:
                break;
        }
    }
}