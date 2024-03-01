using DataAccess;
using Flashcards.SamGannon.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.SamGannon.UI
{
    public class MainMenu : IMenu
    {
        private readonly IDataAccess _dataAccess;

        public MainMenu(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IDataAccess DataAccess => _dataAccess;

        public void ShowMenu()
        {
            MenuMessages.ShowMainMenu();
            string choice = ConsoleHelper.ReadValidInput(new[] { "1", "2", "3", "4", "I" });

            switch (choice)
            {
                case "1":
                    new StackMenu(this).ShowMenu();
                    ShowMenu();
                    break;
                case "2":
                    new FlashcardMenu(this).ShowMenu();
                    ShowMenu();
                    break;
                case "3":
                    new StudySession(this).StartStudySession();
                    ShowMenu();
                    break;
                case "4":
                    Console.WriteLine("Exiting the Custom Flashcard Console App. Goodbye!");
                    Environment.Exit(0);
                    break;
                case "I":
                    ShowInstructions();
                    ShowMenu();
                    break;
                default:
                    break;
            }
        }

        public void ShowInstructions()
        {
            MenuMessages.ShowInstructionsMenu();

            string choice = Console.ReadLine()?.Trim().ToUpper();
            int exitAttempts = 0;

            while (choice != "E" && exitAttempts < 3)
            {
                Console.Clear();
                Console.WriteLine("Invalid command. Press 'E' to exit.");
                choice = Console.ReadLine()?.Trim().ToUpper();
                exitAttempts++;
            }

            if (choice == "E")
            {
                ShowMenu();
            }
            else
            {
                // i may log that the user failed to exit the instructions menu successfully.
                Environment.Exit(1);
            }
        }
    }
}
 

