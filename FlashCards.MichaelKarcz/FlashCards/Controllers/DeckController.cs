using FlashCards.Database;
using FlashCards.DTOs;
using FlashCards.Models;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace FlashCards.Controllers
{
    internal static class DeckController
    {
        internal static void MainDeckMenu()
        {
            int menuChoiceNumber = -1;

            while (menuChoiceNumber != 0)
            {
                string menuChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("- Decks - \nWhat would you like to do?")
                        .PageSize(7)
                        .AddChoices(new[]
                        {
                            "1) Create a deck",
                            "2) View all cards in a deck",
                            "3) Add cards to a deck",
                            "4) Remove cards from a deck",
                            "5) Delete a deck",
                            "0) [grey]Return to the main menu[/]"
                        }));

                menuChoiceNumber = Int32.Parse(menuChoice.Substring(0, 1));

                switch (menuChoiceNumber)
                {
                    case 0:
                        AnsiConsole.Clear();
                        break;
                    case 1:
                        AnsiConsole.Clear();
                        CreateADeck();
                        break;
                    case 2:
                        AnsiConsole.Clear();
                        ShowAllCardsInDeck();
                        break;
                    case 3:
                        AnsiConsole.Clear();
                        AddCardsToDeck();
                        break;
                    case 4:
                        AnsiConsole.Clear();
                        RemoveCardsFromDeck();
                        break;
                    case 5:
                        AnsiConsole.Clear();
                        DeleteADeck();
                        break;
                    default:
                        AnsiConsole.Clear();
                        AnsiConsole.WriteLine("An error has occurred while processing your request. Returning to the main menu.");
                        menuChoiceNumber = 0;
                        break;
                }
            }
        }

        internal static void CreateADeck()
        {
            Deck deck = new Deck();

            deck.Name = AnsiConsole.Prompt(new TextPrompt<string>("What is the name of this deck?"));
            
            while(!CheckIfDeckNameIsUnique(deck.Name))
            {
                AnsiConsole.WriteLine("That deck name already exists. Please enter a unique name for this deck.\n");
                deck.Name = AnsiConsole.Prompt(new TextPrompt<string>("What is the name of this deck?"));
            }
            
            int deckId = DeckDBHelper.InsertDeck(deck);

            AddCardsToDeck(deckId);
        }

        internal static void ShowAllCardsInDeck()
        {
            Deck deck = SelectADeck("Which deck would you like to view?");
            List<FlashcardDto> flashcards = FlashcardDBHelper.GetAllFlashcardsInDeck(deck.Id);
            FlashcardController.DisplayFlashcardsInDeck(deck, flashcards);
        }

        internal static void AddCardsToDeck()
        {
            Deck deck = SelectADeck("Which deck would you like to add cards to?");

            AddCardsToDeck(deck.Id);
        }

        internal static void AddCardsToDeck(int id)
        {
            int cardsToCreate = AnsiConsole.Prompt(new TextPrompt<int>("\nHow many cards would you like to add to this deck?"));

            for (int i = 1; i <= cardsToCreate; i++)
            {
                AnsiConsole.Clear();
                AnsiConsole.WriteLine($"Card {i}:");
                FlashcardController.CreateAFlashcard(id);
            }
        }

        internal static void RemoveCardsFromDeck()
        {
            Deck deck = SelectADeck("Which deck would you like to remove cards from?");

            List<FlashcardDto> flashcards = FlashcardDBHelper.GetAllFlashcardsInDeck(deck.Id);

            List<FlashcardDto> selectedFlashcards = AnsiConsole.Prompt(
                new MultiSelectionPrompt<FlashcardDto>()
                    .Title("Which flashcards would you like to remove?")
                    .NotRequired()
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more flashcards)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to select a flashcard, " +
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(flashcards.ToArray())
                    );

            foreach(FlashcardDto flashcard in selectedFlashcards)
            {
                FlashcardDBHelper.DeleteFlashcardById(flashcard.Id);
            }

            AnsiConsole.WriteLine("\nThe flashcards have been deleted.\n");
        }

        internal static void DeleteADeck()
        {
            Deck deck = SelectADeck("Which deck would you like to delete?");

            bool confirmDelete = AnsiConsole.Prompt(
                new TextPrompt<bool>("Are you sure you'd like to delete this deck? This will delete all associated flashcards and study sessions as well.")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(false)
                .WithConverter(choice => choice ? "yes" : "no"));

            if(confirmDelete)
            {
                FlashcardDBHelper.DeleteAllFlashcardsInDeck(deck.Id);
                StudySessionDBHelper.DeleteAllStudySessionsOfDeck(deck.Id);
                DeckDBHelper.DeleteDeckById(deck.Id);

                AnsiConsole.WriteLine("\nThe deck and all related records have been deleted successfully.");
            }
            else
            {
                AnsiConsole.WriteLine("\nThe deck and related records were not deleted.");
            }
        }

        internal static Deck SelectADeck(string prompt)
        {
            List<Deck> decks = DeckDBHelper.GetAllDecks();

            Deck selectedDeck = AnsiConsole.Prompt(
                        new SelectionPrompt<Deck>()
                        .Title(prompt)
                        .PageSize(10)
                        .AddChoices(decks.ToArray()));

            return selectedDeck;
        }

        internal static bool CheckIfDeckNameIsUnique(string name)
        {
            List<string> deckNames = DeckDBHelper.GetAllDeckNames();
            
            if (deckNames.IsNullOrEmpty()) return true;
            if (deckNames.Contains(name)) return false;

            return true;
        }
    }
}
