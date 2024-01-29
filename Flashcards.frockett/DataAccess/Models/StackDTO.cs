
namespace Library.Models;

public class StackDto
{
        public string Name { get; set; }
        public int stackId { get; set; }
        public List<CardDto> Flashcards { get; set; }

        public StackDto(string stackName)
        {
            Name = stackName;
            Flashcards = new List<CardDto>();
        }

        public void AddFlashcard(CardDto flashcard)
        {
            Flashcards.Add(flashcard);
        }
}
