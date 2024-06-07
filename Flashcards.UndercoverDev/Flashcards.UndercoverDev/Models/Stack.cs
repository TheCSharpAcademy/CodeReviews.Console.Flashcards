
namespace Flashcards.UndercoverDev.Models
{
    public class Stack
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        // Collection of related Flashcards
        public List<Flashcard> Flashcards { get; set; } = [];

        public static StackDTO ToStackDTO(Stack stack)
        {
            var stackDTO = new StackDTO
            {
                Name = stack.Name
            };

            foreach (var flashcard in stack.Flashcards)
            {
                stackDTO.Flashcards.Add(Flashcard.ToFlashcardDTO(flashcard));
            }

            return stackDTO;
        }
    }

    public class StackDTO
    {
        public string Name { get; set; } = "";
        public List<FlashcardDTO> Flashcards { get; set; } = [];
    }
}