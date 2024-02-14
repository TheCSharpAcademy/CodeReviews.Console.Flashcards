using Infrastructure.Repositoreis.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Repositoreis.Classes;


public class Repository<T> : IRepository<T> where T : class
{
    private IList<T> _data = new List<T>();
    public void Add(T entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(T entity)
    {
        throw new NotImplementedException();
    }

    public T Find(Expression<Func<T, bool>> predicate)
    {

        return _data.FirstOrDefault(predicate.Compile()) ?? default(T);
    }

    public IList<T> GetAll()
    {
        throw new NotImplementedException();
    }

    public T GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(T entity)
    {
        throw new NotImplementedException();
    }
}
