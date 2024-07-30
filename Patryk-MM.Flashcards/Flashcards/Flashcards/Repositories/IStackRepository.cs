using System.Linq.Expressions;
using Flashcards.Models;

namespace Flashcards.Repositories;
public interface IStackRepository : IBaseRepository<Stack> {
    Task<List<string>> GetStackNamesAsync(Expression<Func<Stack, bool>>? predicate = null);
    Task<Stack> GetStackByIdAsync(int id);
    Task<Stack?> GetStackByNameAsync(string name);
}

