using System.Linq.Expressions;

namespace Infrastructure.Repositoreis.Interfaces;


public interface IRepository<T> where T : class
{
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    T GetById(int id);
    IList<T> GetAll();
    T Find(Expression<Func<T, bool>> predicate);
}
