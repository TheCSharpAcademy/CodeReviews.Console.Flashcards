using Flashcards.Models;

namespace Flashcards.Repositories;
public interface IStackRepository : IBaseRepository<Stack> {
    Task<List<string>> GetStackNamesAsync();
}

