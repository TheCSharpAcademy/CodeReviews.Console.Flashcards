using Spectre.Console;
using Flashcards.empty_codes.Controllers;
using Flashcards.empty_codes.Models;

namespace Flashcards.empty_codes.Views
{
    internal class FlashcardMenu
    {
        public FlashcardsController FlashcardController { get; }
        
        public void GetFlashcardMenu()
        {
            var flashcardChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose an [green]option below[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal your choices)[/]")
                    .AddChoices(new[] {
                        "Add New Flashcard","Edit Flashcard",
                        "Delete Flashcard", "Return to Main Menu",
                    }));

            switch (flashcardChoice)
            {
                case "Add New Flashcard":
                    AddNewFlashcard(); 
                    break;
                case "Edit Flashcard":
                    UpdateFlashcard(); 
                    break;
                case "Delete Flashcard":
                    DeleteFlashcard(); 
                    break;
                case "Return to Main Menu":
                    return; 
                default:
                    AnsiConsole.WriteLine("Invalid selection. Please try again."); 
                    break;
            }

        }

        public void AddNewFlashcard()
        {

        }

        public void UpdateFlashcard()
        {

        }

        public void DeleteFlashcard()
        {

        }
    }
}
