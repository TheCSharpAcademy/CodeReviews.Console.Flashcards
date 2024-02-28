using Flash.Helper.Renumber;
using Spectre.Console;
using System.Data.SqlClient;
using Flash.Helper.StudyHelper;
using Flash.Helper.MainHelper;

namespace Flash.Launching;
internal class Study
{   
    internal static void GetStudy()
    {
        Console.Clear();

        ShowBanner.GetShowBanner("Study", Color.Yellow);

        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");


            //see available stacks 
            try
            {
                string showAllStacks =
                    @"SELECT Stack_Primary_Id, Name
                        FROM Stacks";

                using (SqlCommand showAllStacksCommand = new SqlCommand(showAllStacks, connection))
                {
                    // Execute the command and obtain a data reader
                    using (SqlDataReader reader = showAllStacksCommand.ExecuteReader())
                    {
                        // Display the names of all tables
                        Console.WriteLine("Stacks in the 'Stacks' Table:");
                        while (reader.Read())
                        {
                            AnsiConsole.WriteLine($"ID: {reader.GetInt32(0)}, Name: {reader.GetString(1)}");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        int studyStack = SetUpStudyStack.GetSetUpStudyStack();

        CreateStudyTable.GetCreateStudyTable(studyStack);

        (int correctAnswer, int totalQuestions) = ShowFlashcardToStudy.GetShowFlashcardToStudy(studyStack);
        
        RecordToStudy.GetRecordToStudy(studyStack, correctAnswer, totalQuestions);

        ReturnComment.MainMenuReturnComments();
    }
}













