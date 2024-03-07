using Flash.Helper.Renumber;
using System.Data.SqlClient;

namespace Flash.Helper.SubManageStacksHelper.EditFlashcardHelper;

internal class ShowFlashcardsInCurrentStack
{

    internal static void GetShowFlashcardsInCurrentStack(int currentWorkingFlashcardId)
    {

        try
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");

                string getCurrentFlashcardIdQuery =
                    $@"SELECT Flashcard_Primary_Id, Front, Back, Stack_Primary_Id  
                           FROM Flashcards 
                           WHERE Flashcard_Primary_Id = '{currentWorkingFlashcardId}'";

                using (SqlCommand getCurrentFlashcardIdCommand = new SqlCommand(getCurrentFlashcardIdQuery, connection))
                {
                    object result = getCurrentFlashcardIdCommand.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        List<FlashcardDto> flashcards = new List<FlashcardDto>();
                        using (SqlCommand command = new SqlCommand(getCurrentFlashcardIdQuery, connection))
                        {
                            command.Parameters.AddWithValue("@currentWorkingFlashcardId", currentWorkingFlashcardId);

                            // Execute the command and retrieve the data
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Check if any rows are returned
                                if (reader.HasRows)
                                {
                                    // Loop through each row and create DTOs
                                    while (reader.Read())
                                    {
                                        FlashcardDto flashcard = new FlashcardDto
                                        {
                                            Flashcard_Primary_Id = reader.GetInt32(0),
                                            Front = reader.GetString(1),
                                            Back = reader.GetString(2),
                                            Stack_Primary_Id = reader.GetInt32(3)
                                        };
                                        flashcards.Add(flashcard);
                                    }

                                    RenumberFlashcards.GetRenumberFlashcards(flashcards);

                                    // Display flashcards
                                    foreach (var flashcard in flashcards)
                                    {
                                        Console.WriteLine(@$"
                                            Flashcard_Primary_Id: {flashcard.Flashcard_Primary_Id},
                                            Front: {flashcard.Front}, 
                                            Back: {flashcard.Back}, 
                                            Stack_Primary_Id: {flashcard.Stack_Primary_Id}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No flashcards TO EDIT found.");
                                }
                            }
                        }
                    }
                }
            }
        }

        catch (Exception ex)
        {

            Console.WriteLine("Error: " + ex.Message);
        }

    }

}
