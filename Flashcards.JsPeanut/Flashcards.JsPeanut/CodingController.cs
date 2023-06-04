using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.JsPeanut
{
    class CodingController
    {
        static string connectionString = "Data Source=(localdb)\\LocalDBDemo;Initial Catalog=Flashcards;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        public static List<Stack> Stacks = new();

        public static List<Flashcard> Flashcards = new();

        public static List<StudySession> StudySessions = new();

        public static void CreateStack()
        {
            string stackName = UserInput.GetStackName();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(
                        $"INSERT INTO dbo.stacks(stack_name) VALUES('{stackName}')", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    Console.WriteLine("Values couldn't be inserted into the table.");
                    UserInput.GetUserInput();
                }
                connection.Close();
                Console.WriteLine("You created a new stack succesfully.");
                UserInput.GetUserInput();
            }
        }

        public static void CreateFlashcard()
        {
            bool success = true;

            int stackId = UserInput.GetToWhichStackItCorresponds();

            string flashcardQuestion = UserInput.GetFlashcardQuestion();

            string flashcardAnswer = UserInput.GetFlashcardAnswer();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"INSERT INTO dbo.flashcards(flashcard_question, flashcard_answer, stack_id) VALUES('{flashcardQuestion}', '{flashcardAnswer}', {stackId})", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    Console.WriteLine($"Error: {ex}");
                    UserInput.GetUserInput();
                }
                if (success == true)
                {
                    Console.WriteLine("You have created a new flashcard succesfully!");
                    UserInput.GetUserInput();
                }
                connection.Close();
            }
        }

        public static void StudyZone()
        {
            RetrieveStacks("display");

            int stackToStudy = UserInput.GetStackIdForStudy();

            PopulateFlashcardsList();

            var flashcardsToStudy = Flashcards.Where(f => f.StackId == stackToStudy).ToList();

            for (int i = 0; i < flashcardsToStudy.Count; i++)
            {
                if (flashcardsToStudy[i].Rating == null)
                {
                    flashcardsToStudy[i].Rating = 0;
                }
            }

            var sortedFlashcards = flashcardsToStudy.OrderBy(flashcard => flashcard.Rating).ToList();

            for (int i = 0; i < sortedFlashcards.Count; i++)
            {
                Console.WriteLine($"Front of the card: {sortedFlashcards[i].Question}");
                string answer = Console.ReadLine();
                if (answer == sortedFlashcards[i].Answer)
                {
                    Console.WriteLine("Correct! \n\nHow was it?");
                }
                else
                {
                    Console.WriteLine($"Actual answer: {sortedFlashcards[i].Question}. \n\nHow was it?");
                }
                Console.OutputEncoding = Encoding.UTF8;
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Cyan;
                (int left, int top) = Console.GetCursorPosition();
                var option = 1;
                var decorator = $"✅ \u001b[32m";
                ConsoleKeyInfo key;
                bool isSelected = false;
                while (!isSelected)
                {
                    Console.SetCursorPosition(left, top);

                    Console.Write($"{(option == 1 ? decorator : "  ")}Hard \u001b[0m");
                    Console.Write($"{(option == 2 ? decorator : "  ")}Good \u001b[0m");
                    Console.Write($"{(option == 3 ? decorator : "  ")}Easy \u001b[0m");

                    key = Console.ReadKey(false);

                    switch (key.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            option = option == 1 ? 3 : option - 1;
                            break;

                        case ConsoleKey.RightArrow:
                            option = option == 3 ? 1 : option + 1;
                            break;

                        case ConsoleKey.Enter:
                            isSelected = true;
                            break;
                    }
                    string rateToAdd = "Difficulty";
                    int rateNumber = 0;
                    string[] rates = { "Hard", "Good", "Easy" };
                    switch (option)
                    {
                        case 1:
                            rateToAdd = rates[0];
                            rateNumber = 1;
                            sortedFlashcards[i].Rating = rateNumber;
                            break;
                        case 2:
                            rateToAdd = rates[1];
                            rateNumber = 2;
                            sortedFlashcards[i].Rating = rateNumber;
                            break;
                        case 3:
                            rateToAdd = rates[2];
                            rateNumber = 3;
                            sortedFlashcards[i].Rating = rateNumber;
                            break;
                    }
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand($"UPDATE dbo.flashcards SET rating = {rateNumber} WHERE flashcard_id = {(sortedFlashcards[i].Rating == null ? sortedFlashcards[i].Rating = 0 : sortedFlashcards[i].Rating = sortedFlashcards[i].Rating)}", connection))
                        {
                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                Console.WriteLine($"\n{decorator}You selected Option {option}");
                
                
                StudySession studySession = new();
                StudySessions.Add(studySession);
                Console.ResetColor();
            }
            UserInput.GetUserInput();
        }

        public static void RetrieveStacks(string displayOrRetrieve)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using(SqlCommand command = new SqlCommand("SELECT * FROM dbo.stacks", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Stacks.Add(new Stack
                        {
                            StackId = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
                var stacksCopy = Stacks.ToList();
                if (displayOrRetrieve == "display")
                {
                    foreach (var stack in stacksCopy)
                    {
                        Console.WriteLine($"{stack.StackId}, {stack.Name}");
                    }
                    stacksCopy.Clear();
                }
                else if (displayOrRetrieve == "retrieve")
                {
                    stacksCopy.Clear();
                }
                connection.Close();
            }
        }

        public static void PopulateFlashcardsList()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.flashcards", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        //Flashcards.Add(new Flashcard
                        //{
                        //    FlashcardId = reader.GetInt32(0),
                        //    Question = reader.GetString(1),
                        //    Answer = reader.GetString(2),
                        //    Rating = reader.GetInt32(3) == null ? Rating = 0 :,
                        //    StackId = reader.GetInt32(4)
                        //});
                        int flashcardId = reader.GetInt32(0);
                        string question = reader.GetString(1);
                        string answer = reader.GetString(2);
                        int? rating = reader.IsDBNull(3) ? rating = 0 : rating = reader.GetInt32(0);
                        int stackId = reader.GetInt32(4);
                        Flashcards.Add(new Flashcard() { FlashcardId = flashcardId, Question = question, Answer = answer, Rating = rating, StackId = stackId });
                    }
                }
                connection.Close();
            }
        }

        //public static void RetrieveStudySessions()
        //{
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.study_sessions", connection))
        //        {
        //            SqlDataReader reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                StudySessions.Add(new StudySession
        //                {
        //                    Id = reader.GetInt32(0),
        //                    Rate = reader.GetString(1),
        //                    RateId = reader.GetInt32(2),
        //                    FlashcardId = reader.GetInt32(3)
        //                });
        //            }
        //        }
        //        connection.Close();
        //    }
        //}
        //public static void RetrieveStudySessions()
        //{
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.study_sessions", connection))
        //        {
        //            SqlDataReader reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                StudySessions.Add(new StudySession
        //                {
        //                    Id = reader.GetInt32(0),
        //                    Rate = reader.GetString(1),
        //                    FlashcardId = reader.GetInt32(2)
        //                });
        //            }
        //        }
        //        connection.Close();
        //    }
        //}
    }
}
