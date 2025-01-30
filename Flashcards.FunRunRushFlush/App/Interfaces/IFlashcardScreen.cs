using Flashcards.FunRunRushFlush.Data.Model;

namespace Flashcards.FunRunRushFlush.App.Interfaces
{
    public interface IFlashcardScreen
    {
        Task RunFlashcardsView(Stack stack);
    }
}