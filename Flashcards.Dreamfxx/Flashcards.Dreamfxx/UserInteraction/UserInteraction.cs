using Flashcards.Dreamfxx.Validations;

namespace Flashcards.Dreamfxx.UserInteraction;
public class UserInteraction
{
    private readonly ValidateInput _validateInput;

    public UserInteraction(ValidateInput validateInput)
    {
        _validateInput = validateInput ?? throw new ArgumentNullException(nameof(validateInput));
    }

    // Add methods for user interaction
}
