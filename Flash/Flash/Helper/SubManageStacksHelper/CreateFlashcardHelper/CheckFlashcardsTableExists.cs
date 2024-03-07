using System.Data.SqlClient;

namespace Flash.Helper.SubManageStacksHelper.CreateFlashcardHelper;
internal class CheckFlashcardsTableExists
{
    internal static int GetCheckFlashcardsTableExists()
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            // Check if 'Flashcards' table exists
            string checkFlashcardsTableQuery =
                @"SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Flashcards'";

            using (SqlCommand checkFlashcardsTableCommand = new SqlCommand(checkFlashcardsTableQuery, connection))
            {
                int flashcardsTableCount = Convert.ToInt32(checkFlashcardsTableCommand.ExecuteScalar());
                return flashcardsTableCount;
            }
        }
    }
}
