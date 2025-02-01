using Flashcards.FunRunRushFlush.Data.Model;

namespace Flashcards.FunRunRushFlush.Services.Interfaces
{
    public interface IUserInputValidationService
    {
        Flashcard ValidateUserFlashcardInput(Stack stack, Flashcard flashcard = null);
        Stack ValidateUserStackInput(Stack? stack = null);
    }
}