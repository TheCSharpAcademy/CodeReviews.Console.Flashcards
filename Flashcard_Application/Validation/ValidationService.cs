using Flashcard_Application.DataServices;
using Flashcard_Application.UI;
using Flashcards.Models;
using Flashcards.UI;

namespace Flashcards.Validation;

public static class ValidationService
{
    public static void MainMenuInputValidation(string userSelection)
    {
        switch (userSelection)
        {
            case "Go to Study Area": StudyAreaMenu.StudyAreaPrompt(); break;
            case "Stack & Card Management": StackAndCardManagementMenu.StackAndCardManagementPrompt(); break;
            case "Close the Application": Environment.Exit(0); break;
        }
    }

    public static void StackCardManagementInputValidation(string userSelection)
    {
        switch (userSelection)
        {
            case "Create a Stack": CreateStack.CreateStackPrompt(); break;
            case "Delete a Stack": DeleteStack.DeleteStackPrompt(); break;
            case "Create a Card": CreateCard.CreateCardPrompt(); break;
            case "Delete a Card": DeleteCard.DeleteCardPrompt(); break;
            case "Return to Main Menu": MainMenu.MainMenuPrompt(); break;
        }
    }

    public static void StudyAreaInputValidation(string userSelection)
    {
        switch (userSelection)
        {
            case "Choose a Stack to study": StudySessionCards.SelectStackToStudy(); break;
            case "View study History": StudySessionHistory.ShowStudySessionHistory(); break;
            case "Return to Main Menu": MainMenu.MainMenuPrompt(); break;
        }
    }

    public static bool UniqueStackNameCheck(string stackName)
    {
        List<CardStack> stacksList = StackDatabaseServices.GetAllStacks();
        foreach (CardStack stack in stacksList)
        {
            if (stackName == stack.StackName)
            {
                return false;
            }
        }
        return true;
    }
}
