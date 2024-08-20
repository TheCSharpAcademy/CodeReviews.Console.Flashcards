using Flashcards.Domain.Entities;
using Flashcards.Domain.Interfaces;
using Flashcards.Data.Context;


namespace Flashcards.Data.Repositories
{
    public class FlashcardRepository : IFlashcardRepository
    {
        private readonly ApplicationDbContext _context;

        public FlashcardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public void Add(Flashcard flashcard)
        {
            _context.Flashcards.Add(flashcard);
            _context.SaveChanges();
        }

        
        public void Update(Flashcard flashcard)
        {
            _context.Flashcards.Update(flashcard);
            _context.SaveChanges();
        }

        
        public void Delete(int flashcardId)
        {
            var flashcard = _context.Flashcards.Find(flashcardId);
            if (flashcard != null)
            {
                _context.Flashcards.Remove(flashcard);
                _context.SaveChanges();
                RenumberFlashcards(flashcard.StackId);
            }
        }

        public void DeleteByStackId(int stackId)
        {
            var flashcards = GetByStackId(stackId);
            _context.Flashcards.RemoveRange(flashcards);
            _context.SaveChanges();
        }



        
        public Flashcard GetById(int flashcardId)
        {
            var flashcard = _context.Flashcards.Find(flashcardId);
            if (flashcard == null)
            {
                throw new KeyNotFoundException($"Flashcard with ID {flashcardId} was not found.");
            }
            return flashcard;
        }

        
        public IEnumerable<Flashcard> GetByStackId(int stackId)
        {
            return _context.Flashcards
                           .Where(f => f.StackId == stackId)
                           .ToList();
        }

        public void RenumberFlashcards(int stackId)
        {
            var flashcards = GetByStackId(stackId).OrderBy(f => f.Id).ToList();
            for (int i = 0; i < flashcards.Count; i++)
            {
                flashcards[i].Id = i + 1; 
                _context.Flashcards.Update(flashcards[i]);
            }
            _context.SaveChanges();
        }

    }
}
