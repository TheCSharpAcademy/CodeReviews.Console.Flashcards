using Flashcards.Models;

//When adding a new flashcard or updating an existing one,
//use this DTO to collect the necessary information while abstracting away database details from the user.
namespace Flashcards.DTO
{
    public class FlashcardDTO
    {
        public int StackCardId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
//Understand DTO for next time