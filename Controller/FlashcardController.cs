using Flashcards.TwilightSaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.TwilightSaw.Controller
{
    internal class FlashcardController(AppDbContext context)
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
