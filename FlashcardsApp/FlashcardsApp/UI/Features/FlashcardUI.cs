using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsApp.DTOs;
using FlashcardsApp.Models;
using FlashcardsApp.Services;
using FlashcardsApp.UI.Core;
using Spectre.Console;

namespace FlashcardsApp.UI.Features
{
    internal class FlashcardUI
    {
        private readonly DatabaseService _databaseService;
        private readonly InputValidator _validator;

        internal FlashcardUI(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            _validator = new InputValidator();
        }

        public void ViewCards(int stackId)
        {
            _databaseService.GetFlashcardsByID(stackId);
        }

        public void CreateFlashcard(int stackId)
        {
            Flashcard flashcard = new();

            string front = GetFrontContent();
            if (string.IsNullOrEmpty(front)) return;

            string back = GetBackContent();
            if (string.IsNullOrEmpty(back)) return;

            flashcard.StackId = stackId;
            flashcard.Front = front;
            flashcard.Back = back;
            flashcard.CreatedDate = DateTime.Now;

            _databaseService.PostFlashcard(flashcard);
            Console.WriteLine("\nFlashcard created successfully!");
        }

        public void UpdateFlashcard(int stackId)
        {
            List<FlashcardDTO> flashcards = _databaseService.GetFlashcardsByID(stackId);

            if (!flashcards.Any())
            {
                Console.WriteLine("\nNo flashcards in this stack!");
                return;
            }

            Dictionary<string, int> cardMapping = flashcards.ToDictionary(
                f => $"Front: {f.Front}\t\t| Back: {f.Back}",
                f => f.FlashcardId);

            var choices = cardMapping.Keys.ToList();
            choices.Add("Return to Stack Menu");

            var selectedCard = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a card to update.")
                .AddChoices(choices));

            if (selectedCard == "Return to Stack Menu")
            {
                return;
            }

            int flashcardId = cardMapping[selectedCard];

            Flashcard? flashcard = _databaseService.GetFlashcardByFlashcardId(flashcardId, stackId);
            if (flashcard == null)
            {
                return;
            }

            bool finished = false;
            while (!finished)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Select what to update, save, or exit.")
                    .AddChoices(new[]
                    {
                        "Front",
                        "Back",
                        "Save Changes",
                        "Return to Stack Menu"
                    }));

                switch (choice)
                {
                    case "Front":
                        string newFront = GetFrontContent();
                        if (!string.IsNullOrEmpty(newFront))
                            flashcard.Front = newFront;
                        break;
                    case "Back":
                        string newBack = GetBackContent();
                        if (!string.IsNullOrEmpty(newBack))
                            flashcard.Back = newBack;
                        break;
                    case "Save Changes":
                        flashcard.CreatedDate = DateTime.Now;
                        _databaseService.UpdateFlashcard(stackId, flashcardId, flashcard);
                        finished = true;
                        break;
                    case "Return to Stack Menu":
                        return;
                }
            }
        }

        public void DeleteFlashcard(int stackId)
        {
            List<FlashcardDTO> flashcards = _databaseService.GetFlashcardsByID(stackId);

            Dictionary<string, int> cardMapping = flashcards.ToDictionary(
                f => $"Front: {f.Front}\t\t| Back: {f.Back}",
                f => f.FlashcardId);

            var choices = cardMapping.Keys.ToList();
            choices.Add("Return to Stack Menu");

            var selectedCard = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a card to delete.")
                .AddChoices(choices));

            if (selectedCard == "Return to Stack Menu")
            {
                return;
            }

            int flashcardId = cardMapping[selectedCard];
            _databaseService.DeleteFlashcard(stackId, flashcardId);
        }

        private string GetFrontContent()
        {
            return _validator.GetFlashcardContent("front");
        }

        private string GetBackContent()
        {
            return _validator.GetFlashcardContent("back");
        }

    }
}
