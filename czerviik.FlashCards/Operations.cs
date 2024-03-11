using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards
{
    public static class Operations
    {
        public static int GetFlashcardDbId(int userId, List<Flashcard> flashcards, List<FlashcardReviewDto> flashcardDtos)
        {
            var index = flashcardDtos.FindIndex(f => f.DisplayId == userId);
            return flashcards[index].Id;
        }

        public static List<FlashcardReviewDto> ConvertToDto(List<Flashcard> flashcards)
        {
            var flashcardDtos = new List<FlashcardReviewDto>();
            int startId = 1;
            foreach (var flashcard in flashcards)
            {
                flashcardDtos.Add(new FlashcardReviewDto
                {
                    DisplayId = startId++,
                    Question = flashcard.Question,
                    Answer = flashcard.Answer,
                    StackId = flashcard.StackId
                });
            }
            return flashcardDtos;
        }

        public static string[] StackListToNamesArray(List<Stack> stacks)
        {
            var stacksArray = new string[stacks.Count];
            for (int i = 0; i < stacks.Count; i++)
            {
                stacksArray[i] = stacks[i].Name;
            }
            return stacksArray;
        }
    }
}
