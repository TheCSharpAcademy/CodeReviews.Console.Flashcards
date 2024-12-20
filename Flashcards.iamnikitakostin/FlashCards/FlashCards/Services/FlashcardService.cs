using FlashCards.Controllers;
using FlashCards.Data;
using FlashCards.Models;
using FlashCards.View;
using Spectre.Console;
using static FlashCards.Enums;

namespace FlashCards.Services
{
    internal class FlashcardService : ConsoleController
    {
        private readonly DataContext _context;

        public FlashcardService(DataContext context)
        {
            _context = context;
        }

        public static List<string> AllToChoicesList(List<Flashcard> flashcards)
        {
            return flashcards.Select(s => s.FrontText).ToList();
        }

        public Flashcard SelectFlashcardToManage()
        {
            var flashcards = GetAll();

            if (flashcards.Count == 0)
            {
                ErrorMessage("No stacks available.");
                return null;
            }

            var flashcardChoices = AllToChoicesList(flashcards);
            string selectedChoice = UserInterface.ManageAllItemsMenu(flashcardChoices);

            Flashcard flashcard = flashcards.FirstOrDefault(s => s.FrontText == selectedChoice);

            if (flashcard == null)
            {
                ErrorMessage("Flashcard was not found");
            }

            return flashcard;
        }

        public static void ShowFull(Flashcard flashcard)
        {
            try
            {
                Table table = DrawFlashcardTable();
                table.AddRow(flashcard.Id.ToString(), flashcard.FrontText, flashcard.BackText, flashcard.Stack.Name, flashcard.CreationTime.ToString());
                AnsiConsole.Write(table);

                UserInterface.StandardMenu();
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }

        public void Add(Flashcard flashcard)
        {
            try
            {
                flashcard.NextTimeToReview = DateTime.Now.AddSeconds(1);
                _context.Flashcards.Add(flashcard);
                _context.SaveChanges();
                SuccessMessage($"The stack with a front text of {flashcard.FrontText} has been added successfully.");
            }
            catch (Exception ex)
            {
                ErrorMessage($"There has been an error while adding the flashcard: {ex.Message}");
            }
        }

        public void Edit(ManageFlashcardEditOptions editChoice, Flashcard flashcard)
        {
            try
            {
                Flashcard savedFlashcard = GetById(flashcard.Id) ?? throw new Exception("Flashcard was not found, or the database was not connected properly.");

                switch (editChoice)
                {
                    case ManageFlashcardEditOptions.Front:
                        savedFlashcard.FrontText = flashcard.FrontText;
                        break;
                    case ManageFlashcardEditOptions.Back:
                        savedFlashcard.BackText = flashcard.BackText;
                        break;
                    default:
                        break;
                }

                _context.SaveChanges();
                SuccessMessage($"The flashcard with a front text of {flashcard.FrontText} has been updated successfully.");
            }
            catch (Exception ex)
            {
                ErrorMessage($"There has been an error while editing the flashcard: {ex.Message}");
            }
        }

        public void Update(Flashcard flashcard)
        {
            try
            {
                Flashcard savedFlashcard = GetById(flashcard.Id) ?? throw new Exception("Flashcard was not found, or the database was not connected properly.");

                savedFlashcard.NextTimeToReview = flashcard.NextTimeToReview;
                savedFlashcard.LastTimeReviewed = flashcard.LastTimeReviewed;
                savedFlashcard.ReviewBreakInSeconds = flashcard.ReviewBreakInSeconds;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorMessage($"There has been an error while editing the flashcard: {ex.Message}");
            }
        }

        public void Delete(int id)
        {
            try
            {
                Flashcard flashcard = GetById(id);
                _context.Flashcards.Remove(flashcard);
                _context.SaveChanges();

                SuccessMessage($"The flashcard with an id of {id} has been deleted successfully.");
            }
            catch (Exception ex)
            {
                ErrorMessage($"There has been an error while deleting the flashcard: {ex.Message}");
                return;
            }
        }

        public Flashcard GetById(int id)
        {
            return _context.Flashcards.FirstOrDefault(s => s.Id == id);
        }

        public List<Flashcard> GetAll()
        {
            return _context.Flashcards.ToList();
        }

        internal static List<BreakPeriodInSecondsOptions> GetNewShowtimes(Flashcard flashcard)
        {
            BreakPeriodInSecondsOptions currentReviewTime = (BreakPeriodInSecondsOptions)flashcard.ReviewBreakInSeconds;

            List<BreakPeriodInSecondsOptions> reviewTimeOptions = new List<BreakPeriodInSecondsOptions>();

            if (currentReviewTime != BreakPeriodInSecondsOptions.Minute)
            {
                BreakPeriodInSecondsOptions previousPeriod = Extensions.Previous(currentReviewTime);
                reviewTimeOptions.Add(previousPeriod);
            }
            reviewTimeOptions.Add(currentReviewTime);

            BreakPeriodInSecondsOptions nextPeriod = Extensions.Next(currentReviewTime);
            reviewTimeOptions.Add(nextPeriod);

            return reviewTimeOptions;
        }

        internal Flashcard? GetNextFlashcardToReview(int stackId)
        {
            Flashcard nextFlashcard = _context.Flashcards.Where(f=>f.StackId == stackId).Where(f => f.NextTimeToReview < DateTime.Now).FirstOrDefault();
            if (nextFlashcard == null)
            {
                DateTime? nextTimeToReview = _context.Flashcards.OrderBy(f => f.NextTimeToReview).Select(f => f.NextTimeToReview).FirstOrDefault();
                if (nextTimeToReview == null)
                {
                    ErrorMessage("There are no cards in the stack, please add some to study");
                } else
                {
                    ErrorMessage($"There is no more flashcards to review, please try again after {nextTimeToReview}");
                }
                return null;
            }
            return nextFlashcard;
        }
    }
}
