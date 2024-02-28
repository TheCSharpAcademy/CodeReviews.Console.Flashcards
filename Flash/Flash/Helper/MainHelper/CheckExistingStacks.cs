using System.Data.SqlClient;

namespace Flash.Helper.MainHelper;
internal class CheckExistingStacks
{
    internal static void GetCheckExistingStacks(string currentWorkingStack)
    {
        string checkDuplicatedStackQuery =
            @$"SELECT COUNT(*) 
                    FROM dbo.Stacks 
                    WHERE Name = '{currentWorkingStack}'";

        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            using (SqlCommand checkDuplicatedStackCommand = new SqlCommand(checkDuplicatedStackQuery, connection))
            {
                int SameNameStacksCount = Convert.ToInt32(checkDuplicatedStackCommand.ExecuteScalar());
                if (SameNameStacksCount == 0)
                {
                    string insertStackQuery = $"INSERT INTO Stacks (Name) VALUES ('{currentWorkingStack}')";

                    using (SqlCommand insertStackCommand = new SqlCommand(insertStackQuery, connection))
                    {
                        insertStackCommand.ExecuteNonQuery();
                        Console.WriteLine($"Added {currentWorkingStack} to Stacks");
                    }
                }
                else
                {
                    Console.WriteLine($"Did not added {currentWorkingStack} to Stacks as it already exists");
                }
            }
        }
    }
}
