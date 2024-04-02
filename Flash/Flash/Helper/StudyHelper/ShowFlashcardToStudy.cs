using Flash.Helper.Renumber;
using System.Data.SqlClient;

namespace Flash.Helper.StudyHelper;
internal class ShowFlashcardToStudy
{
    internal static (int, int) GetShowFlashcardToStudy(int studyStack)
    {
        int correctAnswer = 0;
        int wrongAnswer = 0;
        int totalQuestions = 0;

        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            int totalNumberOfFlashCardsInThatStack;

            string totalNumberOfFlashCardsInThatStackString =
                        @$"SELECT COUNT(*) 
                                FROM Flashcards
                                WHERE Stack_Primary_ID = '{studyStack}'";

            using (SqlCommand totalNumberOfFlashCardsInThatStackStringCommand = new SqlCommand(totalNumberOfFlashCardsInThatStackString, connection))
            {
                totalNumberOfFlashCardsInThatStack = (int)totalNumberOfFlashCardsInThatStackStringCommand.ExecuteScalar();
                Console.WriteLine($"This is the total number = {totalNumberOfFlashCardsInThatStack}");
            }

            string selectQuery =
                    $@"SELECT Flashcard_Primary_Id, Front, Back, Stack_Primary_Id
                                        FROM Flashcards 
                                        WHERE Stack_Primary_Id = @studyStackId";

            List<FlashcardDto> flashcards = new List<FlashcardDto>();

            using (SqlCommand command = new SqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@studyStackId", studyStack);

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
                            Console.WriteLine($"Front: {flashcard.Front}");
                            Console.WriteLine("What's the back");

                            string answer = Console.ReadLine();

                            if (answer == flashcard.Back)
                            {
                                Console.WriteLine("Correct!");
                                correctAnswer++;
                                totalQuestions++;
                            }
                            else
                            {
                                Console.WriteLine("Incorrect!");
                                wrongAnswer++;
                                totalQuestions++;
                            }
                        }
                        Console.WriteLine($"you got {correctAnswer} correct out of {totalQuestions}");

                    }
                }

            }
        }
        return (correctAnswer, totalQuestions);
    }
}
