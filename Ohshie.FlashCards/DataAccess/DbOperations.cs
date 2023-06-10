using System.Data.SqlClient;
using Ohshie.FlashCards.DataAccess;

public class DbOperations : DbBase
{
    public void InitDb()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText = SqlCommands["createStackTable"];

            tableCommand.ExecuteNonQuery();

            tableCommand.CommandText = SqlCommands["createFlashcardsTable"];
    
            tableCommand.ExecuteNonQuery();
    
            connection.Close();
        }
    }

    public List<Stack> FetchAllStacks()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = SqlCommands["fetchAllStacks"];

            var reader = tableCommand.ExecuteReader();

            var stacksList = ReadFromDbToStacksList(reader);

            return stacksList;
        }
    }
    
    private List<Stack> ReadFromDbToStacksList(SqlDataReader reader)
    {
        List<Stack> stacksList = new();
        while (reader.Read())
        {
            stacksList.Add(new Stack()
            {
                Id = int.Parse(reader.GetString(0)),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
            });
        }
        return stacksList;
    }
}