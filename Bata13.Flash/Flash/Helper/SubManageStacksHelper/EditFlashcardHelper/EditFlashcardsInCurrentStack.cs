using System.Data.SqlClient;

namespace Flash.Helper.SubManageStacksHelper.EditFlashcardHelper;
internal class EditFlashcardsInCurrentStack
{
    internal static void GetEditFlashcardsInCurrentStack(int currentWorkingFlashcardId)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");

                Console.WriteLine("Update Front");
                string updatedFront = Console.ReadLine();
                Console.WriteLine("Update Back");
                string updatedBack = Console.ReadLine();

                // Update the flashcard with the new front and back
                string updateFlashcardQuery =
                    @"UPDATE Flashcards 
                        SET Front = @UpdatedFront, Back = @UpdatedBack 
                        WHERE Flashcard_Primary_Id = @FlashcardId";

                using (SqlCommand updateCommand = new SqlCommand(updateFlashcardQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@UpdatedFront", updatedFront);
                    updateCommand.Parameters.AddWithValue("@UpdatedBack", updatedBack);
                    updateCommand.Parameters.AddWithValue("@FlashcardId", currentWorkingFlashcardId);

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) updated.");
                }
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
