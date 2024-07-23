using Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Repositories {
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext dbContext) {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task<T> GetByIdAsync(int Id) {
            return await _dbSet.FindAsync(Id);
        }
    }
}