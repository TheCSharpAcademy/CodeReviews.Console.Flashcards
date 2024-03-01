using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.SamGannon.UI
{
    internal class MenuMessages
    {

        public static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Custom Flashcard Console App ===");
            Console.WriteLine("1. Stack Menu");
            Console.WriteLine("2. Flashcard Menu");
            Console.WriteLine("3. Start Study Session");
            Console.WriteLine("4. Exit");
            Console.WriteLine("");
            Console.WriteLine("H - History");
            Console.WriteLine("I - Instructions");
            Console.WriteLine("Enter your choice (1, 2, 3, 4, or I): ");
        }

        public static void ShowStackMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Stack Menu ===");
            Console.WriteLine("1. Create Stack");
            Console.WriteLine("2. Edit Stack Names");
            Console.WriteLine("3. Delete Stack");
            Console.WriteLine("4. Back to Main Menu");
            Console.WriteLine("Enter your choice (1, 2, 3, or 4): ");
        }

        public static void ShowFlashcardMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Flashcard Menu ===");
            Console.WriteLine("1. Create Flashcards");
            Console.WriteLine("2. Delete Flashcards");
            Console.WriteLine("3. Back to Main Menu");
            Console.WriteLine("Enter your choice (1, 2, or 3): ");
        }

        public static void FlashCardMessage()
        {
            Console.WriteLine();
            Console.WriteLine("* Each flashcard must belong to a stack");
            Console.WriteLine("* Please select a stack by its Id that you want to add too.");
            Console.WriteLine("* Please note: that if this stack is ever deleted, the flashcard will also be deleted.");
            Console.WriteLine("==================================================================================================================");
            Console.WriteLine("Please be aware: there is no built in validation for user input for the creation of flashcards");
            Console.WriteLine("For example: if creating flashcards for mathimatical operations, the program relies on the user for correct input");
            Console.WriteLine("See the following two examples for acceptable inputs but wrong answers");
            Console.WriteLine("==================================================================================================================");
            Console.WriteLine("Question: What is 2 + 2?");
            Console.WriteLine("Answer: 5");
            Console.WriteLine("==================================================================================================================");
            Console.WriteLine("How do you say hello in Spanish?");
            Console.WriteLine("Answer: Adios");
            Console.WriteLine("==================================================================================================================");
            Console.WriteLine("Follow the on screen prompts to continue");
            Console.WriteLine("Press a key to continue.");
            Console.ReadLine();
            Console.Clear();
        }

        public static void EditMessage()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("* Following the on screen prompts, you can edit individual flashcards in specified stacks.");
            Console.WriteLine("* Please select a stack by its Id that you want to manage");
            Console.WriteLine("* Please note: There a limited number of actions you can edit.");
            Console.WriteLine("     Edit the question");
            Console.WriteLine("     Edit the answer");
            Console.WriteLine("     Delete the flashcard");
            Console.WriteLine("======================================================================================================================");
            Console.WriteLine("Follow the on screen prompts to continue");
            Console.WriteLine("Press a key to continue.");
            Console.ReadLine();
            Console.Clear();
        }

        internal static void FlashCardMessageReminder()
        {
            Console.WriteLine("Select the stack you would like to add to by typing it's name.");
            Console.WriteLine("");
        }

        internal static void ShowInstructionsMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Instructions ===");
            Console.WriteLine("");
            Console.WriteLine("Welcome to the Custom Flashcard Console App!");
            Console.WriteLine("Navigate through the app using the following instructions:");
            Console.WriteLine("");
            Console.WriteLine("1. To access different menus, enter the corresponding number:");
            Console.WriteLine("   - Enter '1' for Flashcard Menu");
            Console.WriteLine("   - Enter '2' for Stack Menu");
            Console.WriteLine("   - Enter '3' to start a Study Session");
            Console.WriteLine("   - Enter '4' to exit the application");
            Console.WriteLine("   - Enter 'I' to view instructions (you're here now)");
            Console.WriteLine("");
            Console.WriteLine("2. In each menu, follow the on-screen prompts to perform actions:");
            Console.WriteLine("   - Create and manage flashcards in the Flashcard Menu");
            Console.WriteLine("   - Create and manage stacks in the Stack Menu");
            Console.WriteLine("   - Start a study session to review flashcards");
            Console.WriteLine("");
            // add more instructions as they become neccessary
            Console.WriteLine("3. Press 'E' to exit the instructions screen and return to the main menu.");
        }
    }
}
