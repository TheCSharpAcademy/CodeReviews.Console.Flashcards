using FlashStudy.Models;

namespace FlashStudy.DTOs
{
    public class FlashCardToViewDTO
    {
        public string Title { get; set; }
        public string Answer { get; set; }

        public FlashCardToViewDTO(FlashCard flashCard)
        {
            Title = flashCard.Title;
            Answer = flashCard.Answer;
        }
    }
}
