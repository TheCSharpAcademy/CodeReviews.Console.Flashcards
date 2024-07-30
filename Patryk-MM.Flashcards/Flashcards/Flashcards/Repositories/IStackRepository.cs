using Flashcards.Models;
using System.Linq.Expressions;

namespace Flashcards.Repositories;
public interface IStackRepository : IBaseRepository<Stack> {
    Task<List<string>> GetStackNamesAsync(Expression<Func<Stack, bool>>? predicate = null);
    Task<Stack?> GetStackByNameAsync(string name);
}

