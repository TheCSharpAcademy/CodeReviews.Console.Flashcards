using Flashcards.FunRunRushFlush.Data.Model;

namespace Flashcards.FunRunRushFlush.App.Interfaces
{
    public interface IFlashcardScreen
    {
        Task RunFlashcardSession(Stack selectedStack, SessionMode sessionMode);
        Task RunFlashcardsView(Stack stack);
    }
}