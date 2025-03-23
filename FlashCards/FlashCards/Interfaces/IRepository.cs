namespace FlashCards
{
    internal interface IRepository <T>
    {
        bool Insert(T entity);
        bool Update(T entity);
        bool Delete(T entity);

        IEnumerable<T> GetAllRecords();

    }
}
