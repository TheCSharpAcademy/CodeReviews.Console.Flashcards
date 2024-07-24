using Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Repositories;
public class StackRepository : BaseRepository<Stack>, IStackRepository {
    public StackRepository(AppDbContext _dbContext) : base(_dbContext) {
    }

    public List<string> GetStackNamesAsync() {
        var list = _dbContext.Stacks.ToList();
        return list.Select(x => x.Name).ToList();
    }
}

