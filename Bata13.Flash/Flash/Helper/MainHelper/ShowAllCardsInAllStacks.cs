using Flash.Helper.Renumber;
using System.Data.SqlClient;

namespace Flash.Helper.MainHelper;

internal class ShowAllCardsInAllStacks
{

    internal static void GetShowAllCardsInAllStacks()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");

                string selectQuery =
                    $@"SELECT Flashcard_Primary_Id , Front, Back, Stack_Primary_Id
                            FROM Flashcards";

                List<FlashcardDto> flashcards = new List<FlashcardDto>();
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
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
                            Console.WriteLine("No flashcards found.");
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
