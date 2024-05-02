using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

namespace FlashCards.obitom67
{
    public class Display
    {

        public static bool CloseApplication { get; set; }

        public static void GetUserInput()
        {
            CloseApplication = false;
            string currentStack;
            string[] selectionChoices =
            {
                "Exit",
                "Manage Stacks",
                "Create Stack",
                "Study",
                "View Study Session Data"
            };
            SelectionPrompt<string> menuPrompt = new SelectionPrompt<string>();
            menuPrompt.AddChoices(selectionChoices);
            menuPrompt.Title = "Please select an option:";
            var menuSelection = AnsiConsole.Prompt<string>(menuPrompt);

            switch (menuSelection)
            {
                case "Exit":
                    CloseApplication = true;
                    AnsiConsole.WriteLine("Thank you for using The FlashCard App (TM)");
                    break;
                case "Manage Stacks":
                    ManageStacks();
                    break;
                case "Create Stack":
                    Stack.CreateStack();
                    break;
                case "Study":
                    StudySession.StartSession();
                    break;
                case "View Study Session Data":
                    StudySession.ShowRecords();
                    break;
            }

            
        }

        public static void ManageStacks()
        {
            Stack currentStack = new Stack();
            string[] stackSelections =
                    {
                        "Main Menu",
                        "Change Stack",
                        "View Flashcards",
                        "Create Flashcard",
                        "Update Flashcard",
                        "Delete Flashcard",
                        "Delete Stack",
                        "Change Stack Name"
                    };
            currentStack = Stack.DisplayStacks();
            
            SelectionPrompt<string> stackPrompt = new SelectionPrompt<string>();
            stackPrompt.AddChoices(stackSelections);
            stackPrompt.Title = "Please make a selection:";
            var stackSelection = AnsiConsole.Prompt(stackPrompt);
            switch (stackSelection)
            {
                case "Main Menu":
                    AnsiConsole.Clear();
                    GetUserInput();
                    break;
                case "Change Stack":
                    AnsiConsole.Clear();
                    ManageStacks();
                    break;
                case "View Flashcards":
                    AnsiConsole.Clear();
                    Stack.ReadStack(currentStack);
                    break;
                case "Create Flashcard":
                    AnsiConsole.Clear();
                    FlashCard.CreateFlashCard(currentStack);
                    break;
                case "Update Flashcard":
                    AnsiConsole.Clear();
                    FlashCard.UpdateFlashCards(currentStack);
                    break;
                case "Delete Flashcard":
                    AnsiConsole.Clear();
                    FlashCard.DeleteFlashCard(currentStack);
                    break;
                case "Delete Stack":
                    AnsiConsole.Clear();
                    Stack.DeleteStack(currentStack);
                    break;
            }
        }
    }
}
