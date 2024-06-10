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
        using (var conn = _databaseManager.GetConnection())
        {
            try
            {
                var query = "DELETE FROM Stacks Where Name = @Name";
                conn.Execute(query, new { Name = name });
                Console.WriteLine("Stack deleted. \n");
            }
            catch (SqlException ex)
            {
                Console.Beep();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("SQL error occured while trying to delete data. Could not delete stack. Program will crash after your next key press.");
                Console.ReadKey();
                throw;
            }

        }
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