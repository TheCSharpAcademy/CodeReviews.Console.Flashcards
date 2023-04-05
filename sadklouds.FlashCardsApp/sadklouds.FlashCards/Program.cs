

using ConsoleTableExt;
using FlashCardsLibrary;
using FlashCardsLibrary.Tools;
using sadklouds.FlashCards;
using sadklouds.FlashCards.Helpers;
using System.Security.Cryptography;

SQLDataAccess db = new SQLDataAccess();
CRUD controller = new(db);
//Menus menus = new(controller);



MainMenu();





void MainMenu()
{
    bool exit = false;
    while (exit == false)
    {
        MenuHelper.MainMenu();
        string input = UserInputHelper.GetUserStringInput("");
        switch (input.ToLower())
        {
            case "m":

                controller.GetStacks();
                string stackName = UserInputHelper.GetUserStringInput("Choose a stack to manage: ");
                bool stackNameFound = controller.checkStackName(stackName);
                if (stackNameFound == true)
                {
                    ManageStackMenu(stackName);
                }
                else Console.WriteLine("Stack not found");
                break;
            case "s":
                //int stackId = db.GetStackId("spanish");
                //Console.WriteLine(stackId);
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
        Console.WriteLine("X) Change current stack");
        Console.WriteLine("V) view all flash cards");
        Console.WriteLine("C) Create Flash Card in stack");
        Console.WriteLine("E) Edit Flash Card");
        Console.WriteLine("D) Delete FlashCard");
        Console.WriteLine("0) return to main menu");
        Console.WriteLine("--------------------------------");
        Console.Write("\nPlease Select an option: ");

        var flashcards = db.LoadFlashCards(stackName);
        string input = UserInputHelper.GetUserStringInput("");
        switch (input.ToLower())
        {
            case "v":
                controller.GetFlashCards(flashcards);
                break;
            case "c":
                controller.CreateFlashCard(stackName);
                break;
            case "e":
                controller.GetFlashCards(flashcards);
                controller.UpdateFlashCard(flashcards);
                break;
            case "d":
                controller.GetFlashCards(flashcards);
                controller.DeleteFlashCard(flashcards);
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

