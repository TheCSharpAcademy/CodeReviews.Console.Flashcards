namespace Flashcards;

public class UI
{
    bool validInt;
    bool closeApp;
    Validation validation = new Validation();
    UILogic uILogic = new UILogic();
    string selectedStack = "none";

    public void MainMenu()
    {
        int validIntInput = 0;

        while (closeApp == false)
        {
            do
            {
                Console.Clear();

                Console.WriteLine(@$"
Current selected stack: {selectedStack}

Main Menu

Choose an Option:
1 - Exit
2 - Manage Stacks
3 - Manage Flashcards
4 - Study
5 - View study session data");

                string mainMenuInput = Console.ReadLine();
                validIntInput = validation.GetValidInt(mainMenuInput, out validInt);
            } while (validInt == false);

            Console.WriteLine(validIntInput);
            switch (validIntInput)
            {
                case 1:
                    closeApp = true;
                    break;
                case 2:
                    ManageStacksMenu();
                    break;
                case 3:
                    ManageFlashcardsMenu();
                    break;
                case 4:
                    uILogic.StartStudy(selectedStack);
                    break;
                case 5:
                    uILogic.ShowStudySession();
                    break;
                default:
                    validInt = false; //Will keep the loop going until the user picks a number thats is in the switch
                    break;
            }
        }
    }

    void ManageStacksMenu() 
    {
        Console.Clear();

        validInt = false;
        int validIntInput = 0;

        while (validInt == false)
        {
            Console.WriteLine(@$"
Manage Stacks Menu

Current selected stack: {selectedStack}

Choose an Option:
1 - Back to Main Menu
2 - Create a stack
3 - Delete a stack
4 - Change current stack
");
            string stackMenuInput = Console.ReadLine();
            validIntInput = validation.GetValidInt(stackMenuInput, out validInt);
        }

        switch (validIntInput)
        {
            case 1:
                validInt = true; //breaks 2nd loop going back to 1st loop and showing main menu again.
                break;
            case 2:
                uILogic.CreateNewStack();
                break;
            case 3:
                uILogic.ShowStacksList();
                uILogic.DeleteStack(ref selectedStack);
                break;
            case 4:
                uILogic.ShowStacksList();
                uILogic.SelectStack(ref selectedStack);
                break;
            default:
                validInt = false; //Will keep the 2nd loop going until the user picks a number thats is in the switch
                break;
        }
    }

    void ManageFlashcardsMenu()
    {
        validInt = false;
        int validIntInput = 0;

        while (validInt == false)
        {
            Console.Clear();

            Console.WriteLine(@$"
Manage FlashCards Menu

Current selected stack: {selectedStack}

Choose an Option:
1 - Back to Main Menu
2 - Change current stack
3 - View all flashcards in stack
4 - Create a new flashcard in the current stack
5 - Edit a flashcard
6 - Delete a flashcard
");
            string flashcardMenuInput = Console.ReadLine();
            validIntInput = validation.GetValidInt(flashcardMenuInput, out validInt);
        }

        switch (validIntInput)
        {
            case 1:
                validInt = true; //breaks 2nd loop going back to 1st loop and showing main menu again.
                break;
            case 2:
                uILogic.ShowStacksList();
                uILogic.SelectStack(ref selectedStack);
                break;
            case 3:
                uILogic.ShowFilteredFlashcardsList(selectedStack);
                break;
            case 4:
                uILogic.CreateFlashcard(selectedStack);
                break;
            case 5:
                uILogic.ShowAllFlashCardsList();
                uILogic.UiUpdateFlashcard();
                break;
            case 6:
                uILogic.ShowAllFlashCardsList();
                uILogic.UiDeleteFlashcard();
                break;
            default:
                validInt = false; //Will keep the loop going until the user picks a number thats is in the switch
                break;
        }
    }
}
