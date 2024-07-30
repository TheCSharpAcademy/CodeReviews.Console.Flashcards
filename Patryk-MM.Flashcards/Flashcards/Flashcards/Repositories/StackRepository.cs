using Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Repositories;
public class StackRepository : BaseRepository<Stack>, IStackRepository {
    public StackRepository(AppDbContext dbContext) : base(dbContext) {
    }

    public async Task<Stack> GetStackByIdAsync(int id) {
        return await DbSet
            .Include(x => x.Flashcards)
            .FirstAsync(x => x.Id == id);
    }

    public async Task<Stack?> GetStackByNameAsync(string name) {
        return await DbSet
            .Include(x => x.Flashcards)
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<string>> GetStackNamesAsync() {
        return await DbSet
            .Select(x => x.Name)
            .ToListAsync();
    }

}

