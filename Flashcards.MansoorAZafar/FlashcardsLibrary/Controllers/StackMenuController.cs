using FlashcardsLibrary.Views;
using FlashcardsLibrary.Models;
using Microsoft.EntityFrameworkCore.Update;

namespace FlashcardsLibrary.Controllers;

internal class StackMenuController
{    
    private FlashcardManager? flashcardManager;
    internal void HandleStack()
    {
        bool continueHandlingStack = true;
        while(continueHandlingStack)
        {
            System.Console.Clear();
            if(!Utilities.EmptyStack)
            {
                DataViewer.DisplayHeader("Current Items in Stack", "left");
                List<Stack> currentStackItems = Utilities.databaseManager.GetAllData<Stack>(Utilities.StackTableName);
                DataViewer.DisplayListAsTable(Stack.Headers, currentStackItems);
            }
            else 
            {
                System.Console.WriteLine("The stack is currently empty\n");
            }
            if(!string.IsNullOrEmpty(Utilities.CurrentStack))
            {
                System.Console.WriteLine($"Current Stack: {Utilities.CurrentStack}\n\n");
            }

            StackHomeSelections stackHomeSelection =
                Utilities.GetSelection<StackHomeSelections>
                (
                    enumerationValues: Enum.GetValues<StackHomeSelections>(),
                    title: "[green]Select Your Option[/]",
                    alternateNames: item => item switch 
                    {
                        StackHomeSelections.CreateStack => "Create A Stack",
                        StackHomeSelections.InputStack => "Enter a Stack",
                        StackHomeSelections.DeleteStack => "Delete a Stack",
                        _ => item.ToString()
                    }
                );
            
            switch (stackHomeSelection)
            {
                case StackHomeSelections.CreateStack:
                    this.CreateStack();
                    break;
                
                case StackHomeSelections.InputStack:
                    this.EnterStack();
                    break;
                
                case StackHomeSelections.DeleteStack:
                    this.DeleteStack();
                    Utilities.CurrentStack = null;
                    break;
                
                default:
                    continueHandlingStack = false;
                    break;
            }
        }
    }

    private void CreateStack()
    {
        System.Console.Clear();
        DataViewer.DisplayHeader("Creating a Stack");
        string topic = 
            Utilities.GetStringInput
            (
                message: "Enter the Topic of the Stack\n> ",
                errorMessage: "Name must be unique and cannot be null or empty\n> ",
                Utilities.databaseManager.TopicFree
            );
        Utilities.databaseManager.CreateStack(topic);
        Utilities.EmptyStack = false;
    }

    private void EnterStack()
    {
        if(Utilities.EmptyStack)
        {
            System.Console.WriteLine("There Are no Items in the Stack to Enter!\n Please create one and try again\n");
            return;
        }

        int id = 
            Utilities.GetIntegerInput
            (
                message: "Enter the Stack ID\n> ",
                errorMessage: "Enter a valid/Existing Stack ID\n> ",
                Condition: Utilities.databaseManager.IDExistsStack
            );
        
        string topic = Utilities.databaseManager.GetTopicFromStackID(id);
        Utilities.CurrentStack = topic;

        StackSelections modificationSelection;
        do 
        {
            modificationSelection = this.DisplayStackModificationOptions();
            this.HandleStackModification(topic, modificationSelection);   
        }while(modificationSelection != StackSelections.exit && modificationSelection != StackSelections.ChangeCurrentStack);
    }

    private StackSelections DisplayStackModificationOptions()
    {
        System.Console.Clear();
        DataViewer.DisplayHeader("Stack Modification");
        return  
            Utilities.GetSelection<StackSelections>
            (
                enumerationValues: Enum.GetValues<StackSelections>(),
                title: "[green]Select Your Option[/]",
                alternateNames: item => item switch 
                {
                    StackSelections.DeleteFlashCard => "Delete a FlashCard",
                    StackSelections.ViewXFlashCardsInStack => "View X FlashCards in stack",
                    StackSelections.EditFlashCard => "Edit a FlashCard",
                    StackSelections.ViewAllFlashCardsInStack => "View all Flashcards in stack",
                    StackSelections.CreateFlashCardInStack => "Create a FlashCard in current Stack",
                    StackSelections.ChangeCurrentStack => "Change Current Stack",
                    _ => item.ToString()
            });
    }

    private void HandleStackModification(string topic, StackSelections selection)
    {
        this.flashcardManager = new(topic);

        switch (selection)
        {
            case StackSelections.exit: 
                break;
            
            case StackSelections.ChangeCurrentStack:
                break;
            
            default:
                flashcardManager.HandleFlashcardSelection(selection);
                break;
        }
        Utilities.PressToContinue();
    }

    private void DeleteStack()
    {
        if(Utilities.EmptyStack)
        {
            System.Console.WriteLine("There Are no Items in the Stack to Enter!\n Please create one and try again\n");
            return;
        }

        int id = 
            Utilities.GetIntegerInput
            (
                message: "Enter the Stack ID\n> ",
                errorMessage: "Enter a valid/Existing Stack ID\n> ",
                Condition: Utilities.databaseManager.IDExistsStack
            );
        
        string topic = Utilities.databaseManager.GetTopicFromStackID(id);
        Utilities.CurrentStack = topic;

        Utilities.databaseManager.DeleteStack(id, topic);
    }
}