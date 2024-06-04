using Flashcards.UndercoverDev.Models;

namespace Flashcards.UndercoverDev.Repository
{
    public interface IStackRepository
    {
        void Post(StackDTO stack);
    }
}