using FlashCards.Database;
using FlashCards.DTOs;
using FlashCards.Models;
using Spectre.Console;
using System.Runtime.InteropServices;

namespace FlashCards.Controllers
{
    internal static class StudySessionController
    {
        internal static void MainStudySessionMenu()
        {
            AnsiConsole.Clear();
            Deck selectedDeck = DeckController.SelectADeck("Which deck would you like to use?");

            int menuChoiceNumber = -1;

            while (menuChoiceNumber != 0)
            {
                
                string menuChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title($"\n-Study Session- ~{selectedDeck.Name}~\nWhat would you like to do?")
                        .PageSize(7)
                        .AddChoices(new[]
                        {
                            "1) Start studying",
                            "2) View previous study session results from this deck",
                            "3) Delete a study session",
                            "4) Select a different deck",
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
                        StudyDeck(selectedDeck);
                        break;
                    case 2:
                        AnsiConsole.Clear();
                        ShowPreviousStudySessionsOfSingleDeck(selectedDeck);
                        break;
                    case 3:
                        AnsiConsole.Clear();
                        DeleteAStudySession(selectedDeck);
                        break;
                    case 4:
                        AnsiConsole.Clear();
                        menuChoiceNumber = 0; // we want to break out when coming back to this from the next method call
                        MainStudySessionMenu();
                        break;
                    default:
                        AnsiConsole.Clear();
                        AnsiConsole.WriteLine("An error has occurred while processing your request. Returning to the main menu.");
                        menuChoiceNumber = 0;
                        break;
                }

            }
        }

        internal static void StudyDeck(Deck deck)
        {
            List<FlashcardDTO> flashcards = FlashcardDBHelper.GetAllFlashcardsInDeck(deck.Id);

            if (flashcards.Count == 0)
            {
                AnsiConsole.WriteLine("\nThere are no flashcards in this deck. Add some flashcards to study the deck.\n");
                return;
            }

            int numberOfCardsToStudy = PromptNumberOfCardsToStudy(flashcards.Count);

            if (numberOfCardsToStudy == 0) return; // "return to previous menu option

            // Shuffle the stack to get random flashcards every time
            Span<FlashcardDTO> flashcardSpan = CollectionsMarshal.AsSpan(flashcards);
            Random.Shared.Shuffle<FlashcardDTO>(flashcardSpan); // this call shuffles the related list. Feel free to use list flashcards again

            int correctAnswers = 0;
            for (int i = 0; i < numberOfCardsToStudy; i++)
            {
                AnsiConsole.WriteLine($"{deck.Name} - {i + 1} out of {numberOfCardsToStudy}");
                bool answerWasCorrect = PromptCard(flashcards[i]);

                if (answerWasCorrect) correctAnswers++;
            }

            StudySession studySession = new StudySession
            {
                DeckId = deck.Id,
                DeckName = deck.Name,
                Score = correctAnswers,
                CardsStudied = numberOfCardsToStudy
            };

            StudySessionDBHelper.InsertStudySession(studySession);

            AnsiConsole.Clear();
            AnsiConsole.WriteLine(@$"Results!
Date: {studySession.SessionDate.ToString("MM-dd-yyyy hh:mm tt")}
Deck: {deck.Name}
Score: {studySession.Score} / {studySession.CardsStudied}");

            AnsiConsole.WriteLine("\n\nPress any key to return to the main study session menu.");
            AnsiConsole.Console.Input.ReadKey(false);
            AnsiConsole.Clear();
        }

        internal static void ShowPreviousStudySessions()
        {
            List<StudySessionDTO> sessions = StudySessionDBHelper.GetAllStudySessions();

            if (sessions.Count == 0)
            {
                AnsiConsole.WriteLine("\nThere are no study sessions recorded.\n");
                return;
            }

            AnsiConsole.WriteLine($"\n~Study Sessions~");
            Table table = new Table();
            table.AddColumn(new TableColumn("Date").Centered().NoWrap());
            table.AddColumn(new TableColumn("Deck").Centered().NoWrap());
            table.AddColumn(new TableColumn("Score").Centered().NoWrap());
            foreach (StudySessionDTO ss in sessions)
            {
                table.AddRow(ss.SessionDate.ToString("MM-dd-yyyy hh:mm tt"), ss.DeckName, $"{ss.Score.ToString()}/{ss.CardsStudied}");
            }
            table.Border(TableBorder.Heavy);
            table.ShowRowSeparators();
            AnsiConsole.Write(table);

            AnsiConsole.WriteLine();
        }

        internal static void ShowPreviousStudySessionsOfSingleDeck(Deck deck)
        {
            List<StudySessionDTO> sessions = StudySessionDBHelper.GetAllStudySessionsOfDeck(deck);

            if (sessions.Count == 0)
            {
                AnsiConsole.WriteLine("\nThere are no study sessions recorded for this deck.\n");
                return;
            }

            AnsiConsole.WriteLine($"\n~Study Sessions~");
            Table table = new Table();
            table.AddColumn(new TableColumn("Date").Centered().NoWrap());
            table.AddColumn(new TableColumn("Deck").Centered().NoWrap());
            table.AddColumn(new TableColumn("Score").Centered().NoWrap());
            foreach (StudySessionDTO ss in sessions)
            {
                table.AddRow(ss.SessionDate.ToString(), ss.DeckName, ss.Score.ToString());
            }
            table.Border(TableBorder.Heavy);
            table.ShowRowSeparators();
            AnsiConsole.Write(table);

            AnsiConsole.WriteLine();
        }

        internal static void DeleteAStudySession(Deck deck)
        {
            List<StudySessionDTO> studySessions = StudySessionDBHelper.GetAllStudySessionsOfDeck(deck);

            if (studySessions.Count == 0)
            {
                AnsiConsole.WriteLine("\nThere are no study sessions recorded for this deck.\n");
                return;
            }

            StudySessionDTO selectedSession = AnsiConsole.Prompt(
                new SelectionPrompt<StudySessionDTO>()
                    .Title("Select a study session:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more study sessions)[/]")
                    .AddChoices(studySessions.ToArray())
                    );

            StudySessionDBHelper.DeleteStudySessionById(selectedSession.Id);

            AnsiConsole.WriteLine("\nThe study session has been deleted.\n");
        }

        internal static int PromptNumberOfCardsToStudy(int deckSize)
        {
            int numberOfCardsToStudy = -1;

            while ( numberOfCardsToStudy < 0 || numberOfCardsToStudy > deckSize)
            {
                numberOfCardsToStudy = AnsiConsole.Prompt
                    (new TextPrompt<int>($"There are {deckSize} cards in this deck. How many cards would you like to study? Enter '0' to return to the main study menu."));

                if (numberOfCardsToStudy == 0)
                {
                    AnsiConsole.Clear();
                    return 0;
                }
                if (numberOfCardsToStudy < 0) AnsiConsole.WriteLine("\nYou need to study at least one flashcard.\n");
                if (numberOfCardsToStudy > deckSize) AnsiConsole.WriteLine("\nThere aren't that many cards in this deck.\n");
            }

            return numberOfCardsToStudy;
        }

        internal static bool PromptCard(FlashcardDTO flashcard)
        {
            string response = AnsiConsole.Ask<string>($"What is the answer to this card?\n\n{flashcard.Front}\n");

            if ( response.ToLower() != flashcard.Back.ToLower())
            {
                AnsiConsole.WriteLine($"Incorrect! The correct answer was: {flashcard.Back}\n");
                AnsiConsole.WriteLine("\nPress any key to continue.");
                AnsiConsole.Console.Input.ReadKey(false);
                AnsiConsole.Clear();
                return false;
            }
            else
            {
                AnsiConsole.WriteLine($"Correct!\n");
                AnsiConsole.WriteLine("\nPress any key to continue.");
                AnsiConsole.Console.Input.ReadKey(false);
                AnsiConsole.Clear();
                return true;
            }
        }
    }
}
