using Flashcards.Controllers;
using Flashcards.Data;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Views;

public class Menu
{
    internal void MainMenu()
    {
        var isMenuRunning = true;
        while (isMenuRunning)
        {
            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(
                        "Manage Categories",
                        "Manage Flashcards",
                        "Study",
                        "Reports",
                        "Quit"
                    ));
            switch (userChoice)
            {
                case "Manage Categories":
                    AnsiConsole.Clear();
                    CategoriesMenu();
                    break;
                case "Manage Flashcards":
                    AnsiConsole.Clear();
                    FlashcardsMenu();
                    break;
                case "Study":
                    AnsiConsole.Clear();
                    StudySessionsMenu();
                    break;
                case "Quit":
                    AnsiConsole.WriteLine("Goodbye");
                    isMenuRunning = false;
                    break;
                default:
                    AnsiConsole.WriteLine("Invalid choice. Please choose one of the above");
                    break;
            }
        }
    }

    internal void CategoriesMenu()
    {
        var isMenuRunning = true;
        while (isMenuRunning)
        {
            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(
                        "View All Categories",
                        "Add Category",
                        "Update Category",
                        "Delete Category",
                        "Return to Main Menu"
                    ));
            switch (userChoice)
            {
                case "View All Categories":
                    AnsiConsole.Clear();
                    DataConnection dataConnection = new DataConnection();
                    var categories = dataConnection.GetAllCategories();
                    CategoryController.ViewAllCategories(categories);
                    break;
                case "Add Category":
                    AnsiConsole.Clear();
                    CategoryController.AddCategory();
                    break;
                case "Update Category":
                    AnsiConsole.Clear();
                    CategoryController.UpdateCategory();
                    break;
                case "Delete Category":
                    AnsiConsole.Clear();
                    CategoryController.DeleteCategory();
                    break;
                case "Return to Main Menu":
                    isMenuRunning = false;
                    break;
                default:
                    AnsiConsole.WriteLine("Invalid choice. Please choose one of the above");
                    break;
            }
        }
    }

    internal void FlashcardsMenu()
    {
        var isMenuRunning = true;
        while (isMenuRunning)
        {
            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(
                        "View All Flashcards",
                        "Add Flashcard",
                        "Update Flashcard",
                        "Delete Flashcard",
                        "Return to Main Menu"
                    ));
            switch (userChoice)
            {
                case "View All Flashcards":
                    AnsiConsole.Clear();
                    DataConnection data = new DataConnection();
                    var sessions = data.GetAllFlashcards();
                    FlashcardController.ViewAllFlashcards(sessions);
                    break;
                case "Add Flashcard":
                    AnsiConsole.Clear();
                    FlashcardController.AddFlashcard();
                    break;
                case "Update Flashcard":
                    AnsiConsole.Clear();
                    FlashcardController.UpdateFlashcard();
                    break;
                case "Delete Flashcard":
                    AnsiConsole.Clear();
                    FlashcardController.DeleteFlashcard();
                    break;
                case "Return to Main Menu":
                    isMenuRunning = false;
                    break;
                default:
                    AnsiConsole.WriteLine("Invalid choice. Please choose one of the above");
                    break;
            }
        }
    }

    internal void StudySessionsMenu()
    {
        var isMenuRunning = true;
        while (isMenuRunning)
        {
            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(
                        "Start Study Session",
                        "Study Session History",
                        "Return to Main Menu"));
            switch (userChoice)
            {
                case "Start Study Session":
                    AnsiConsole.Clear();
                    StudySessionController.CreateStudySession();
                    break;
                case "Study Session History":
                    AnsiConsole.Clear();
                    StudySessionController.ViewStudyHistory();
                    break;
                case "Return to Main Menu":
                    isMenuRunning = false;
                    break;
                default:
                    AnsiConsole.WriteLine("Invalid choice. Please choose one of the above");
                    break;
            }
        }
    }
}