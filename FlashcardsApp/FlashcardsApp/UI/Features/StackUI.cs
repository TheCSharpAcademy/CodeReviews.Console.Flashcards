using FlashcardsApp.Models;
using FlashcardsApp.Services;
using FlashcardsApp.UI.Core;
using Spectre.Console;

namespace FlashcardsApp.UI.Features
{
    internal class StackUI
    {
        private readonly DatabaseService _databaseService;
        private readonly FlashcardUI _flashcardUI;
        private readonly InputValidator _validator;

        internal StackUI(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            _flashcardUI = new FlashcardUI(databaseService);
            _validator = new InputValidator();
        }

        public void StacksListMenu()
        {
            while (true)
            {
                Console.Clear();
                _databaseService.GetAllStacks();

                List<Stack> stacks = _databaseService.GetAllStacksAsList();
                if (!stacks.Any())
                {
                    Console.WriteLine("\nNo stacks found. Create a stack first!");
                    Console.WriteLine("\nPress Enter to return to main menu...");
                    Console.ReadLine();
                    return;
                }

                List<string> stackNames = stacks.Select(s => s.Name).ToList();
                stackNames.Add("Return to Main Menu");

                var selectedStack = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Select a stack to manage.")
                    .AddChoices(stackNames));

                if (selectedStack == "Return to Main Menu")
                {
                    return;
                }

                ManageStack(selectedStack);
            }
        }

        private void ManageStack(string selectedStack)
        {
            while (true)
            {
                Console.Clear();

                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title($"Managing stack: {selectedStack}")
                    .AddChoices(new[]
                    {
                        "View Cards",
                        "Add Card",
                        "Update Card",
                        "Delete Card",
                        "Delete this Stack",
                        "Return to Stack Menu",
                        "Return to Main Menu"
                    }));

                int stackId = GetStackId(selectedStack);
                if (stackId == -1)
                {
                    Console.WriteLine("\nError accessing stack. Returning to stack menu...");
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    return;
                }

                switch (action)
                {
                    case "View Cards":
                        _flashcardUI.ViewCards(stackId);
                        break;
                    case "Add Card":
                        _flashcardUI.CreateFlashcard(stackId);
                        break;
                    case "Update Card":
                        _flashcardUI.UpdateFlashcard(stackId);
                        break;
                    case "Delete Card":
                        _flashcardUI.DeleteFlashcard(stackId);
                        break;
                    case "Delete this Stack":
                        if (DeleteStack(stackId, selectedStack))
                            return;
                        break;
                    case "Return to Stack Menu":
                        return;
                    case "Return to Main Menu":
                        return;
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }

        public void CreateStack()
        {
            string name = _validator.GetStackName();
            if (string.IsNullOrEmpty(name))
                return;

            string description = _validator.GetStackDescription();

            Stack stack = new()
            {
                Name = name,
                Description = description,
                CreatedDate = DateTime.Now
            };

            try
            {
                _databaseService.Post(stack);
                Console.WriteLine("\nStack created successfully!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError creating stack: {ex.Message}");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }

        private bool DeleteStack(int stackId, string selectedStack)
        {
            if (_validator.GetConfirmation($"\nAre you sure you want to delete '{selectedStack}' stack?"))
            {
                try
                {
                    _databaseService.DeleteStack(stackId);
                    Console.WriteLine("\nStack deleted successfully!");
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError deleting stack: {ex.Message}");
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    return false;
                }
            }
            return false;
        }

        private int GetStackId(string selectedStack)
        {
            try
            {
                List<Stack> stacks = _databaseService.GetAllStacksAsList();
                Stack stack = stacks.First(s => s.Name == selectedStack);
                return stack.StackId;
            }
            catch (Exception)
            {
                Console.WriteLine("\nError finding stack.");
                return -1;
            }
        }
    }
}
