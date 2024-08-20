using Flashcards.Domain.Entities;


namespace Flashcards.Domain.Interfaces
{
    public interface IStackRepository
    {
        void Add(Stack stack);
        void Update(Stack stack);
        void Delete(int stackId);
        Stack? GetById(int id);
        Stack? GetByName(string name);
        IEnumerable<Stack> GetAll();
    }
}
