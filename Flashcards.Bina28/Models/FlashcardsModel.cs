
namespace Flashcards.Bina28.Models;

internal class FlashcardsModel
{  	public long Stack_id { get; set; }
	public string Name { get; set; }
	public string Question { get; set; }
	public string Answer { get; set; }

	public FlashcardsModel(string name, string question, string answer)
	{
		Name = name;
		Question = question;
		Answer = answer;
	}
}
