using FlashCards.Database;
using FlashCards.DTOs;
using FlashCards.Models;
using Spectre.Console;

namespace FlashCards.Controllers;
internal static class FlashcardController
{
    internal static void MainFlashcardMenu()
    {
        int menuChoiceNumber = -1;

        while (menuChoiceNumber != 0)
        {
            string menuChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("- Flashcards - \nWhat would you like to do?")
                    .PageSize(7)
                    .AddChoices(new[]
                    {
                        "1) View all flashcards",
                        "2) Edit a flashcard",
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
                    ShowAllFlashcardsAsTable();
                    break;
                case 2:
                    AnsiConsole.Clear();
                    EditAFlashcard();
                    break;
                default:
                    AnsiConsole.Clear();
                    AnsiConsole.WriteLine("An error has occurred while processing your request. Returning to the main menu.");
                    menuChoiceNumber = 0;
                    break;
            }
        }
    }

    internal static void ShowAllFlashcardsAsTable()
    {
        if (!FlashcardDBHelper.CheckForRecords()) return;

        List<FlashcardDto> flashcards = FlashcardDBHelper.GetAllFlashcards();
        List<Deck> decks = DeckDBHelper.GetAllDecks();

        AnsiConsole.WriteLine("\n~All Flashcards~");
        Table table = new Table();
        table.AddColumn(new TableColumn("Deck Name").Centered().NoWrap());
        table.AddColumn(new TableColumn("Front of Card").Centered().NoWrap());
        table.AddColumn(new TableColumn("Back of Card").Centered().NoWrap());
        foreach (FlashcardDto flashcard in flashcards)
        {
            Deck deck = decks.First(d => d.Id == flashcard.DeckId);
            table.AddRow(deck.Name, flashcard.Front, flashcard.Back);
        }
        table.Border(TableBorder.Heavy);
        table.ShowRowSeparators();
        AnsiConsole.Write(table);

        AnsiConsole.WriteLine();
    }
    
    internal static void DisplayFlashcardsInDeck(Deck deck, List<FlashcardDto> flashcards)
    {
        int tempId = 1;
        AnsiConsole.WriteLine($"\n~{deck.Name}~");
        Table table = new Table();
        table.AddColumn(new TableColumn("Id").Centered().NoWrap());
        table.AddColumn(new TableColumn("Front of Card").Centered().NoWrap());
        table.AddColumn(new TableColumn("Back of Card").Centered().NoWrap());
        foreach (FlashcardDto flashcard in flashcards)
        {
            table.AddRow(tempId.ToString(), flashcard.Front, flashcard.Back);
            tempId++;
        }
        table.Border(TableBorder.Heavy);
        table.ShowRowSeparators();
        AnsiConsole.Write(table);

        AnsiConsole.WriteLine();
    }

    internal static FlashcardDto? SelectAFlashcard()
    {
        List<FlashcardDto> allFlashcards = FlashcardDBHelper.GetAllFlashcards();

        if (allFlashcards.Count == 0)
        {
            AnsiConsole.WriteLine("\nThere were no flashcards to be found.\n");
            return null;
        }

        FlashcardDto selectedFlashcard = AnsiConsole.Prompt(
            new SelectionPrompt<FlashcardDto>()
                .Title("Select a flashcard:")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more flashcards)[/]")
                .AddChoices(allFlashcards.ToArray())
                );

        return selectedFlashcard;
    }

    internal static void CreateAFlashcard(int deckId)
    {
        Flashcard flashcard = new Flashcard();

        flashcard.Front = AnsiConsole.Prompt(new TextPrompt<string>("Enter the front text of the flashcard: "));
        flashcard.Back = AnsiConsole.Prompt(new TextPrompt<string>("Enter the back text of the flashcard: "));
        flashcard.DeckId = deckId;
        bool success = FlashcardDBHelper.InsertFlashcard(flashcard);

        if (success)
        {
            AnsiConsole.WriteLine("\nInsert successful\n");
        }
        else
        {
            AnsiConsole.WriteLine("\nThere was an error inserting that flashcard.");
        }
    }

    internal static void EditAFlashcard()
    {
        FlashcardDto flashcard = SelectAFlashcard();

        if (flashcard == null)
        {
            AnsiConsole.Clear();
            flashcard.Front = AnsiConsole.Prompt(new TextPrompt<string>("Enter the front text of the flashcard: "));
            flashcard.Back = AnsiConsole.Prompt(new TextPrompt<string>("Enter the back text of the flashcard: "));

            FlashcardDBHelper.UpdateFlashcardById(flashcard.Id, new Flashcard {
                Front = flashcard.Front,
                Back = flashcard.Back,
                DeckId = flashcard.DeckId
            });

            AnsiConsole.WriteLine("\nThe flashcard has been edited.\n");
        }
    }
}
