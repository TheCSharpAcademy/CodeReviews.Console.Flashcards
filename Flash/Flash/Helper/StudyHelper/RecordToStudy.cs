using System.Data.SqlClient;

namespace Flash.Helper.StudyHelper;
internal class RecordToStudy
{
    internal static void GetRecordToStudy(int studyStack, int correctAnswer, int totalQuestions)
    {

        Console.WriteLine($"Your score of this study session is: {correctAnswer} out of {totalQuestions}");

        float floatScore = (float)correctAnswer / totalQuestions;

        string score = floatScore.ToString();

        DateTime date = DateTime.Now;

        Console.WriteLine($"score is {score}");
        Console.WriteLine($"date is {date}");

        Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            string insertStudyQuery =
            @"INSERT INTO Study (Date, Score, Stack_Primary_Id)
                            VALUES (@Date, @Score, @StackPrimaryId)";

            using (SqlCommand insertStudyCommand = new SqlCommand(insertStudyQuery, connection))
            {
                insertStudyCommand.Parameters.AddWithValue("@Date", date);
                insertStudyCommand.Parameters.AddWithValue("@Score", score);
                insertStudyCommand.Parameters.AddWithValue("@StackPrimaryId", studyStack);

                insertStudyCommand.ExecuteNonQuery();
                Console.WriteLine("Recorded StudySession Data successfully.");
            }
        }
    }
}
