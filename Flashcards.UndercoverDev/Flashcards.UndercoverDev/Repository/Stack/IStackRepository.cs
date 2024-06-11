using Flashcards.UndercoverDev.Models;

namespace Flashcards.UndercoverDev.Repository
{
    public interface IStackRepository
    {
        void Post(StackDTO stack);
        void Put(string stackName, string newStackName);
        void Delete(Stack stack);
        List<Stack> GetStacks();
        List<string> GetStackNames();
        Stack GetStackByName(string name);
        Stack GetStackById(int id);
    }
}