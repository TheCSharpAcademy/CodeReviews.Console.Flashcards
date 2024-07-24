using Flashcards.Models;

namespace Flashcards.Repositories;
public interface IStackRepository : IBaseRepository<Stack> {
    List<string> GetStackNamesAsync();
}

