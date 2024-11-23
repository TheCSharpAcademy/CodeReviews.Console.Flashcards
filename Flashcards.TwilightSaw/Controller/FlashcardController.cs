using Flashcards.TwilightSaw.Helpers;
using Flashcards.TwilightSaw.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.TwilightSaw.Controller
{
    public class FlashcardController(AppDbContext context)
    {
        public void Create(string front, string back, int cardStackId)
        {
            context.Add(new Flashcard(front, back, cardStackId));
            context.SaveChanges();
        }

        public List<Flashcard> Read(int cardStackId)
        {
           return context.Flashcards.Where(t => t.CardStackId == cardStackId).AsNoTracking().ToList();
        }

        public void Delete(int flashcardId, int chosenStackCardStackId)
        {
            context.Flashcards.Where(flashcard => flashcard.CardStackId == chosenStackCardStackId && flashcard.Id == flashcardId).ExecuteDelete();
            context.SaveChanges();
        }

        public Flashcard? GetFlashcard(FlashcardController flashcardController, CardStack stack)
        {
            var flashcards = flashcardController.Read(stack.CardStackId);
            return flashcards.Count == 0 ? null : UserInput.ChooseFlashcard(flashcards);
        }

        public void Update(int flashcardId, int chosenStackCardStackId, (string Side, string Text) updatedProp)
        {
            context.Flashcards.Where(flashcard => flashcard.CardStackId == chosenStackCardStackId && flashcard.Id == flashcardId).
                ExecuteUpdate(f => f.SetProperty(
                    flashcard => updatedProp.Side == "Front side" ? flashcard.Front : flashcard.Back,
                    flashcard => updatedProp.Text));
            context.SaveChanges();
        }
    }
}
