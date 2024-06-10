using Dapper;
using System.Data.SqlClient;

public class StackRepository : IManageStacks
{

    private DatabaseManager _databaseManager;

    public StackRepository(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public void CreateStack(string name)
    {

        using (var conn = _databaseManager.GetConnection())
        {
            try
            {
                var query = "INSERT INTO Stacks (Name) VALUES (@Name)";
                conn.Execute(query, new { Name = name });
                Console.WriteLine("Stack inserted. \n");
            }
            catch (SqlException ex)
            {
                Console.Beep();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("SQL error occured while trying to insert data. Could not insert new stack. Program will crash after your next key press.");
                Console.ReadKey();
                throw;
            }

        }
    }

    public void DeleteStack(string name)
    {
        throw new NotImplementedException();
    }

    public List<Stack> GetStacks()
    {
        using (var conn = _databaseManager.GetConnection())
        {
            var query = "SELECT * FROM Stacks";
            return conn.Query<Stack>(query).ToList();
        }
    }
}