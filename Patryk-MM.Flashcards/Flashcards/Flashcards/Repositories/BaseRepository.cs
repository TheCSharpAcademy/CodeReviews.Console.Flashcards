using System.Linq.Expressions;
using Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Repositories;
public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity {
    internal readonly AppDbContext _dbContext;
    internal readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext dbContext) {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int Id) {
        return await _dbSet.FindAsync(Id);
    }

    public async Task<List<T>> GetAllAsync() {
        return await _dbSet.ToListAsync();
    }

    public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate) {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity) {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity) {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task EditAsync(T entity) {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();
    }
}
