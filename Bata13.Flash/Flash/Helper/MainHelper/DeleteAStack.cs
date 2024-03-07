using System.Data.SqlClient;

namespace Flash.Helper.MainHelper;

internal class DeleteAStack
{
    internal static void ExecuteDeleteAStack(int stackIdToDelete)
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            string deleteFlashcardsQuery = @"
                DELETE FROM Flashcards 
                WHERE Stack_Primary_Id = @StackId";

            using (SqlCommand deleteFlashcardsCommand = new SqlCommand(deleteFlashcardsQuery, connection))
            {
                deleteFlashcardsCommand.Parameters.AddWithValue("@StackId", stackIdToDelete);
                int flashcardsDeleted = deleteFlashcardsCommand.ExecuteNonQuery();
                Console.WriteLine($"Deleted {flashcardsDeleted} flashcard(s) associated with the stack.");
            }

            string deleteStudyDataQuery =
                @"DELETE FROM Study 
                      WHERE Stack_Primary_Id = @StackId";

            using (SqlCommand deleteStudyDataCommand = new SqlCommand(deleteStudyDataQuery, connection))
            {
                deleteStudyDataCommand.Parameters.AddWithValue("@StackId", stackIdToDelete);
                int studyDataDeleted = deleteStudyDataCommand.ExecuteNonQuery();
                Console.WriteLine($"Deleted {studyDataDeleted} study data record(s) associated with the stack.");
            }

            string deleteStackQuery =
                @"DELETE FROM Stacks 
                      WHERE Stack_Primary_Id = @StackId";

            using (SqlCommand deleteStackCommand = new SqlCommand(deleteStackQuery, connection))
            {
                deleteStackCommand.Parameters.AddWithValue("@StackId", stackIdToDelete);
                int stacksDeleted = deleteStackCommand.ExecuteNonQuery();
                Console.WriteLine($"Deleted stack with Stack_Primary_Id: {stackIdToDelete}");
            }
        }
    }
}
