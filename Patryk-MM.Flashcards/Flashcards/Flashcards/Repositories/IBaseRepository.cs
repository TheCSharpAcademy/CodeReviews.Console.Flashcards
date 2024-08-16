﻿using Flashcards.Models;
using System.Linq.Expressions;

namespace Flashcards.Repositories;
public interface IBaseRepository<T> where T : BaseEntity {
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] include);
    Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task DeleteAsync(T entity);
    Task EditAsync(T entity);
}
