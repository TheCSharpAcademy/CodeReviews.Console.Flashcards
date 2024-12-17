

namespace Flashcards.Bina28.Models;

internal class StacksModel
{
	public int Stack_id { get; set; }
	public string Name { get; set; }

	public StacksModel(string name) 
	{ 
	
		Name = name;
	}
}
