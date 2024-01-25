
namespace Library.Models;

public class StackDTO
{
        public string Name { get; set; }
        public List<CardDTO> Flashcards { get; set; }

        public StackDTO(string stackName)
        {
            Name = stackName;
            Flashcards = new List<CardDTO>();
        }

        public void AddFlashcard(CardDTO flashcard)
        {
            Flashcards.Add(flashcard);
        }
}
