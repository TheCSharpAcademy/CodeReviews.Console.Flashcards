using System.Data.SqlClient;

namespace Flash.Helper.ManageStacksHelper;
internal class CreateStacksTable
{
    internal static void GetCreateStacksTable()
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            string createStacksTableQuery = @"
                CREATE TABLE Stacks (
                Stack_Primary_Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                Name NVARCHAR(50) NOT NULL
                )";

            using (SqlCommand createStacksTableCommand = new SqlCommand(createStacksTableQuery, connection))
            {
                createStacksTableCommand.ExecuteNonQuery();
                Console.WriteLine("Table 'Stacks' created successfully.");
            }
        }
    }


}
