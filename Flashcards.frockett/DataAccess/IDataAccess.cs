namespace DataAccess;

public interface IDataAccess
{
    public void InitDatabase();
    //public string GetConnectionString();
    public void InsertCard();
    public void DeleteCard();
    public void InsertStack();
    public void DeleteStack();
}
