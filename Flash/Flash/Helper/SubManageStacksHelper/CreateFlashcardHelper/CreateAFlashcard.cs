using System.Data.SqlClient;

namespace Flash.Helper.SubManageStacksHelper.CreateFlashcardHelper;
internal class CreateAFlashcard
{
    internal static void GetCreateAFlashcard(string currentWorkingStack)
    {

        int currentWorkingStackId;
        string getCurrentStackIdQuery =
            $@"SELECT Stack_Primary_Id 
            FROM Stacks 
            WHERE Name = '{currentWorkingStack}'";

        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            using (SqlCommand getCurrentStackIdCommand = new SqlCommand(getCurrentStackIdQuery, connection))
            {
                object result = getCurrentStackIdCommand.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    currentWorkingStackId = Convert.ToInt32(result);

                    Console.WriteLine("Write front");
                    string front = Console.ReadLine();
                    Console.WriteLine("Write back");
                    string back = Console.ReadLine();

                    string insertFlashcardQuery =
                        @"INSERT INTO Flashcards (Front, Back, Stack_Primary_Id)
                                          VALUES (@Front, @Back, @StackPrimaryId)";

                    using (SqlCommand insertFlashcardCommand = new SqlCommand(insertFlashcardQuery, connection))
                    {
                        insertFlashcardCommand.Parameters.AddWithValue("@Front", front);
                        insertFlashcardCommand.Parameters.AddWithValue("@Back", back);
                        insertFlashcardCommand.Parameters.AddWithValue("@StackPrimaryId", currentWorkingStackId);

                        insertFlashcardCommand.ExecuteNonQuery();
                        Console.WriteLine("Flashcard created successfully.");
                    }
                }
            }
        }
    }

}
