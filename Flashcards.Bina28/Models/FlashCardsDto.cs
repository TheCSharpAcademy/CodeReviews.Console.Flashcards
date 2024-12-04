

namespace Flashcards.Bina28.Models
{
	internal class FlashCardsDto
	{
		public string Answer { get; set; }
		public long Flashcard_id { get; set; }
		public string Question { get; set; }

		public FlashCardsDto(long flashcard_id, string question, string answer)
		{
			Question = question;
			Answer = answer;
			Flashcard_id = flashcard_id;
		}
	}
}
