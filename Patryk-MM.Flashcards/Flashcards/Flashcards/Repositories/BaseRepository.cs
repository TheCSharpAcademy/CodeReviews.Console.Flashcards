using System.Linq.Expressions;
using Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Repositories;
public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity {
    internal readonly AppDbContext DbContext;
    internal readonly DbSet<T> DbSet;

    public BaseRepository(AppDbContext dbContext) {
        DbContext = dbContext;
        DbSet = dbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id) {
        return await DbSet.FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes) {
        IQueryable<T> query = DbSet;

        foreach (var include in includes) {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }

    public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate) {
        return await DbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity) {
        await DbSet.AddAsync(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity) {
        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task EditAsync(T entity) {
        DbSet.Update(entity);
        await DbContext.SaveChangesAsync();
    }
}
