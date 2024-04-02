using System.Data.SqlClient;

namespace Flash.Helper.SubManageStacksHelper.CreateFlashcardHelper;
internal class CreateFlashcardsTable
{
    internal static void GetCreateFlashcardsTable()
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            string createFlashcardsTableQuery = @"
                CREATE TABLE Flashcards (
                Flashcard_Primary_Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                Front NVARCHAR(50) NOT NULL,
                Back NVARCHAR(50) NOT NULL,
                Stack_Primary_Id INT FOREIGN KEY REFERENCES Stacks(Stack_Primary_Id)
                )";

            using (SqlCommand createFlashcardsTableCommand = new SqlCommand(createFlashcardsTableQuery, connection))
            {
                createFlashcardsTableCommand.ExecuteNonQuery();
                Console.WriteLine("Table 'Flashcards' created successfully.");
            }
        }
    }

}
