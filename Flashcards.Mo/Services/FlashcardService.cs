using Flashcards.Domain.Entities;
using Flashcards.Domain.Interfaces;
using Flashcards.Domain.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Flashcards.Services
{
    public class FlashcardService
    {
        private readonly IFlashcardRepository _flashcardRepository;

        public FlashcardService(IFlashcardRepository flashcardRepository)
        {
            _flashcardRepository = flashcardRepository;
        }

        public IEnumerable<FlashcardDTO> GetFlashcardsForStack(int stackId)
        {
            var flashcards = _flashcardRepository.GetByStackId(stackId).OrderBy(f => f.DisplayOrder);
            return flashcards.Select((f, index) => new FlashcardDTO
            {
                FlashcardNumber = index + 1,
                Question = f.Question,
                Answer = f.Answer
            });
        }


        public void CreateFlashcard(int stackId, string question, string answer)
        {
            var flashcard = new Flashcard
            {
                StackId = stackId,
                Question = question,
                Answer = answer
            };
            _flashcardRepository.Add(flashcard);
        }

        public void DeleteFlashcard(int flashcardId)
        {
            var flashcard = _flashcardRepository.GetById(flashcardId);
            if (flashcard != null)
            {
                _flashcardRepository.Delete(flashcardId);
                RenumberFlashcards(flashcard.StackId);
            }
        }

        private void RenumberFlashcards(int stackId)
        {
            var flashcards = _flashcardRepository.GetByStackId(stackId).OrderBy(f => f.DisplayOrder).ToList();
            for (int i = 0; i < flashcards.Count; i++)
            {
                flashcards[i].DisplayOrder = i + 1; 
                _flashcardRepository.Update(flashcards[i]);
            }
        }

    }
}
