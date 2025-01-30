using Flashcards.FunRunRushFlush.Data.Model;

namespace Flashcards.FunRunRushFlush.Services.Interfaces
{
    public interface IUserInputValidationService
    {
        Stack ValidateUserSessionInput(Stack? stack = null);
    }
}