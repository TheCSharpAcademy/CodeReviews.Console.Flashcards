using Flashcards.Models;

namespace Flashcards.Repositories;
public interface IStackRepository : IBaseRepository<Stack> {
    Task<List<string>> GetStackNamesAsync();
    Task<Stack> GetStackByIdAsync(int id);
    Task<Stack?> GetStackByNameAsync(string name);
}

