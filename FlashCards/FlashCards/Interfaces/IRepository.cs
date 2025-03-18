namespace FlashCards
{
    internal interface IRepository <T>
    {
        public string TableName { get; }
        bool Insert(T entity);
        bool Update(T entity);
        bool Delete(T entity);

        IEnumerable<T> GetAllRecords();

    }
}
