using Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Repositories;
public class StackRepository : BaseRepository<Stack>, IStackRepository {
    public StackRepository(AppDbContext _dbContext) : base(_dbContext) {
    }

    public async Task<Stack> GetStackByNameAsync(string name) {
        return await _dbContext.Stacks
            .Include(x => x.Flashcards)
            .FirstAsync(x => x.Name == name);
    }

    public async Task<List<string>> GetStackNamesAsync() {
        return await _dbContext.Stacks
                               .Select(x => x.Name)
                               .ToListAsync();
    }

}

