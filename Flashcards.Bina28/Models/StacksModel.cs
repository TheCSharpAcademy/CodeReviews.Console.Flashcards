

namespace Flashcards.Bina28.Models;

internal class StacksModel
{
	public long Stack_id { get; set; }
	public string Name { get; set; }

	public StacksModel(long stack_id, string name) 
	{ 
		Stack_id = stack_id;
		Name = name;
	}
}
