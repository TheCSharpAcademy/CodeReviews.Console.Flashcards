using Flashcards.UndercoverDev.Models;

namespace Flashcards.UndercoverDev.Repository
{
    public interface IStackRepository
    {
        void Post(StackDTO stack);
        void Delete(Stack stack);
        List<Stack> GetStacks();
        List<string> GetStackNames();
    }
}