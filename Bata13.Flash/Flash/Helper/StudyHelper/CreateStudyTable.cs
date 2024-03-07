using System.Data.SqlClient;

namespace Flash.Helper.StudyHelper;
internal class CreateStudyTable
{
    internal static void GetCreateStudyTable(int studyStack)
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            string checkTableQuery =
            @"SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Study'";

            using (SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, connection))
            {
                int tableCount = Convert.ToInt32(checkTableCommand.ExecuteScalar());
                if (tableCount == 0)
                {
                    string createStacksTableQuery =
                        @"CREATE TABLE Study (
                                Study_Primary_Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                                Date DATE NOT NULL,
                                Score NVARCHAR(50) NOT NULL,
                                Stack_Primary_Id INT FOREIGN KEY REFERENCES Stacks(Stack_Primary_Id)
                            )";
                    using (SqlCommand createTableCommand = new SqlCommand(createStacksTableQuery, connection))
                    {
                        createTableCommand.ExecuteNonQuery();
                        Console.WriteLine("Table 'Study' created successfully.");
                    }
                }
                else
                {
                    Console.WriteLine("Table 'Study' already exists in database 'DataBaseFlashCard'.");
                }
            }
            Console.ReadLine();
        }

    }


}
