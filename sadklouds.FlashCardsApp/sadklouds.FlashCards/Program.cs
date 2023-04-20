using FlashCardsLibrary;
using sadklouds.FlashCards.Controllers;
using sadklouds.FlashCards.Helpers;

SQLDataAccess db = new SQLDataAccess();
ControllerStudySession studySession = new();
ControllerFlashCards controllerCards = new(db);
ControllerStacks controllerStacks = new();


MainMenu();


void MainMenu()
{
    bool exit = false;
    while (exit == false)
    {
        Console.WriteLine("---------Main Menu--------");
        Console.WriteLine("C) Create New Stack");
        Console.WriteLine("M) Manage Stacks");
        Console.WriteLine("S) Study");
        Console.WriteLine("V) View Study Data");
        Console.WriteLine("0) Exit");
        Console.WriteLine("----------------------");
        Console.Write("\nPlease Select an option: ");
        string input = UserInputHelper.GetUserStringInput("");
        switch (input.ToLower())
        {
            case "c":
                controllerStacks.GetStacks();
                controllerStacks.AddStack();
                break;
            case "m":

                controllerStacks.GetStacks();
                string stackName = UserInputHelper.GetUserStringInput("Choose a stack to manage: ");
                bool stackNameFound = controllerStacks.checkStackName(stackName);
                if (stackNameFound == true)
                {
                    ManageStackMenu(stackName);
                }
                else Console.WriteLine("Stack not found");
                break;
            case "s":
                controllerStacks.GetStacks();
                studySession.Study();
                break;
            case "v":
                studySession.ViewStudySessions();
                break;
            case "0":
                exit = true;
                break;
            default:
                Console.WriteLine("Unknown command");
                break;
        }
    }
}

void ManageStackMenu(string stackName)
{
    bool exit = false;
    while (exit == false)
    {
        Console.WriteLine($"\n---------{stackName}---------");
        Console.WriteLine("V) view all flash cards");
        Console.WriteLine("C) Create Flash Card in stack");
        Console.WriteLine("E) Edit Flash Card");
        Console.WriteLine("D) Delete FlashCard");
        Console.WriteLine("R) Remove Stack");
        Console.WriteLine("0) return to main menu");
        Console.WriteLine("--------------------------------");
        Console.Write("\nPlease Select an option: ");

        var flashcards = db.LoadFlashCards(stackName);
        string input = UserInputHelper.GetUserStringInput("");
        switch (input.ToLower())
        {
            case "v":
                controllerCards.GetFlashCards(flashcards);
                break;
            case "c":
                controllerCards.CreateFlashCard(stackName);
                break;
            case "e":
                controllerCards.GetFlashCards(flashcards);
                controllerCards.UpdateFlashCard(flashcards);
                break;
            case "d":
                controllerCards.GetFlashCards(flashcards);
                controllerCards.DeleteFlashCard(flashcards);
                break;
            case "r":
                string prompt = UserInputHelper.GetUserStringInput("Are you sure you want to delete stack (y/N) ");
                if (prompt == "y")
                {
                    controllerStacks.RemoveStack(stackName);
                    return;
                }
                break;
            case "0":
                exit = true;
                break;
            default:
                Console.WriteLine("Unknown command");
                break;
        }
    }

}

