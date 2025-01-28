using FunRun.Flashcards.Data.Model;

namespace FunRun.Flashcards.Services.Interfaces
{
    public interface IUserInputValidationService
    {
        Stack ValidateUserSessionInput(Stack? stack = null);
    }
}