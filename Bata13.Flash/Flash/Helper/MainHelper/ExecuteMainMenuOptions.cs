using Flash.Launching;
using System;

namespace Flash.Helper.MainHelper;
internal class ExecuteMainMenuOptions
{
    internal static void GetExecuteMainMenuOptions(string command, bool closeApp)
    {
        switch (command)
        {
            case "0":
                Console.WriteLine("\nGoodbye!\n");
                closeApp = true;
                Environment.Exit(0);
                break;
            case "1":
                Console.WriteLine("Manage Stacks");
                ManageStacks.GetManageStacks();
                break;
            case "2":
                Console.WriteLine("Manage Flashcards");
                ManageFlashcards.GetManageFlashcards();
                break;
            case "3":
                Console.WriteLine("Study");
                Study.GetStudy();
                break;
            case "4":
                Console.WriteLine("View Study History");
                StudyHistory.GetStudyHistory();
                break;
            case "5":
                Console.WriteLine("Delete a Stack");
                DeleteStacks.GetDeleteStacks();
                break;

            default:
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                break;
        }
    }
}
