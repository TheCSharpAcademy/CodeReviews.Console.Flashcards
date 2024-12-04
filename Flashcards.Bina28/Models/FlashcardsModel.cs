
namespace Flashcards.Bina28.Models;

internal class FlashcardsModel
{
	public long Stack_id { get; set; }
	public string Question { get; set; }
	public string Answer { get; set; }

	public FlashcardsModel(long stack_id, string question, string answer)
	{
		Stack_id = stack_id;
		Question = question;
		Answer = answer;
	}
}
