using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace Flashcards.SamGannon.DTOs
{
    public class FlashcardDto
    {
        public int FlashcardId { get; set; }
        public string? Question { get; set; } = string.Empty;
        public string? Answer { get; set; } = string.Empty;

        public FlashcardDto() { }

        public FlashcardDto(Flashcard flashcard) 
        {
            FlashcardId = flashcard.FlashcardId;
            Question = flashcard.Question;
            Answer = flashcard.Answer;
        }

        public static List<FlashcardDto> ToDto(List<Flashcard> flashcards)
        {
            List<FlashcardDto> returnStack = new List<FlashcardDto>();

            foreach (var card in flashcards)
            {
                var FlashcardDto = new FlashcardDto(card);
                returnStack.Add(FlashcardDto);
            }

            return returnStack;
        }
    }
}
