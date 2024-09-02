using FlashcardsLibrary.Models;
using FlashcardsLibrary.Views;

namespace FlashcardsLibrary.Controllers;

internal class HomeMenuController
{
    private readonly StackMenuController StackMenuHandler = new();
    internal void HandleHomeMenuSelection(HomeMenu selectedOption)
    {
        System.Console.Clear();
        switch (selectedOption)
        {
            case HomeMenu.StackManager:
                this.ManageStacks();
                break;
            
            case HomeMenu.FlashCardManager:
                this.ManageFlashcards();
                break;
            
            case HomeMenu.Study:
                this.Study();
                break;
            
            default: break;
        }
    }

    private void ManageStacks()
    {        
        DataViewer.DisplayHeader("Stack Manager");
        this.StackMenuHandler.HandleStack();
    }

    private void ManageFlashcards()
    {
        DataViewer.DisplayHeader("Flashcard Manager");
        if(string.IsNullOrEmpty(Utilities.currentStack))
        {
            System.Console.WriteLine("The current stack is empty, so there are no Flashcards to modify\n Please go to Manage Stacks\n -> Either Create a Stack or Enter the ID of one\n -> Select 'Exit' or any modification you want to make\n -> Come back here as the latest Stack will be selected");
            return;
        }
        FlashcardManager flashcardManager = new(Utilities.currentStack);
        StackSelections flashcardSelection;

        do 
        {
            flashcardSelection = flashcardManager.DisplayFlashcardMenu();
            flashcardManager.HandleFlashcardSelection(flashcardSelection);  
            Utilities.PressToContinue();
        }while(flashcardSelection != StackSelections.exit);
    }

    private void Study()
    {
        DataViewer.DisplayHeader("Study");
        StudyManager studyManager = new();
        studyManager.Study();
    }
}