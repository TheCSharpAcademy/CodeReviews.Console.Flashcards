using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Flashcards
{
    class StacksController
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        
        internal List<StackModel> ViewStacks()
        {
            GetUserInput getUserInput = new();
            var stacks = new List<StackModel>();
            TableVisualisation tableVisualisation = new();
            using(var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM Stacks";

                stacks = connection.Query<StackModel>(query).ToList();

                connection.Close();
            }
            if (stacks.Count > 0) tableVisualisation.ShowStacks(stacks);
        
            return stacks;
        }
        internal void InsertStack(StackModel stack)
        {
            using(var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO Stacks(name) VALUES('{stack.Name}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal int RemoveStack(int id)
        {
            int rowCount;
            using(var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                var tableCmd = conn.CreateCommand();

                tableCmd.CommandText = $"DELETE FROM Stacks WHERE StackId = {id}";

                rowCount = tableCmd.ExecuteNonQuery();

                conn.Close();
            }
            return rowCount;
        }

        internal int UpdateStack(int stackId, StackModel stack)
        {
            int rowsAffected;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"UPDATE Stacks SET Name = '{stack.Name}' WHERE stackId = '{stackId}'";

                rowsAffected = tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            return rowsAffected;
        }
    }
}