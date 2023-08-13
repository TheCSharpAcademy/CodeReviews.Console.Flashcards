using Flashcards.MartinL_no.Controllers;

namespace Flashcards.MartinL_no.UserInterface;

internal class UserInput
{
    private readonly FlashcardController _controller;

	public UserInput(FlashcardController controller)
	{
        _controller = controller;
    }
}
