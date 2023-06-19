using System.Data;
using System.Data.SqlClient;
using TestingArea;
using ConsoleTableExt;

namespace Flashcards.JsPeanut
{
    class CodingController
    {
        static string connectionString = "Data Source=(localdb)\\LocalDBDemo;Initial Catalog=Flashcards;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        public static List<Stack> Stacks = new();

        public static List<Stack> DeletedStacks = new();

        public static List<Flashcard> Flashcards = new();

        public static List<Flashcard> DeletedFlashcards = new();

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
                    Console.WriteLine("You created a new stack succesfully!");
                }
                catch
                {
                    Console.WriteLine("Values couldn't be inserted into the table.");
                    UserInput.GetUserInput();
                }
                connection.Close();
                UserInput.GetUserInput();
            }
        }

        public static void CreateFlashcard()
        {
            int defaultDifficulty = 0;

            int stackId = UserInput.GetToWhichStackItCorresponds();

            string flashcardQuestion = UserInput.GetFlashcardQuestion();

            string flashcardAnswer = UserInput.GetFlashcardAnswer();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"INSERT INTO dbo.flashcards(flashcard_question, flashcard_answer, difficulty, stack_id) VALUES('{flashcardQuestion}', '{flashcardAnswer}', {defaultDifficulty}, {stackId})", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("You have created a new flashcard succesfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                    UserInput.GetUserInput();
                }
                connection.Close();
            }
        }

        public static void RemoveStack()
        {
            RetrieveStacks("display");

            int stackId = Convert.ToInt32(UserInput.GetStackIdForCRUD("Which stack do you want to delete?"));

            foreach (var deletedStack in DeletedStacks)
            {
                if (stackId > deletedStack.StackId)
                {
                    stackId--;
                }
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"DELETE FROM dbo.stacks WHERE stack_id = {stackId}", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("You have removed your stack successfully.");
                    DeletedStacks.Add(new Stack
                    {
                        StackId = stackId,
                        Name = Stacks.Where(s => s.StackId == stackId).Select(s => s.Name).First()
                    });
                    UserInput.GetUserInput();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                    UserInput.GetUserInput();
                }
                connection.Close();
            }
        }

        public static void UpdateStack()
        {
            RetrieveStacks("display");

            int stackId = Convert.ToInt32(UserInput.GetStackIdForCRUD("Which stack do you want to update?"));

            string stackName = UserInput.GetStackName();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"UPDATE dbo.stacks SET stack_name = {stackName} WHERE stack_id = {stackId}", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("You have updated your stack successfully.");
                    UserInput.GetUserInput();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                    UserInput.GetUserInput();
                }
                connection.Close();
            }
        }

        public static void RemoveFlashcard()
        {
            RetrieveStacks("display");

            int stackId = Convert.ToInt32(UserInput.GetStackIdForCRUD("To which stacks does belong the flashcard you want to delete?"));

            RetrieveFlashcards("display", stackId);

            int IdOfTheFcToDelete = UserInput.GetFlashcardIdForCRUD("Type the Id of the flashcard you want to delete.");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"DELETE FROM dbo.flashcards WHERE flashcard_id = {IdOfTheFcToDelete}", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("You have removed your stack successfully.");
                    UserInput.GetUserInput();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                    UserInput.GetUserInput();
                }
                connection.Close();
            }
        }

        public static void UpdateFlashcard()
        {
            RetrieveStacks("display");

            int stackId = Convert.ToInt32(UserInput.GetStackIdForCRUD("To which stacks does belong the flashcard you want to update?"));

            RetrieveFlashcards("display", stackId);

            int IdOfTheFcToDelete = UserInput.GetFlashcardIdForCRUD("Type the Id of the flashcard you want to update.");

            string fc_question = UserInput.GetFlashcardQuestion();

            string fc_answer = UserInput.GetFlashcardAnswer();

            int difficulty = 0;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"UPDATE dbo.flashcards SET flashcard_question = {fc_question}, flashcard_answer = {fc_answer}, difficulty = {difficulty} WHERE flashcard_id = {IdOfTheFcToDelete}", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("You have removed your stack successfully.");
                    UserInput.GetUserInput();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                    UserInput.GetUserInput();
                }
                connection.Close();
            }
        }

        public static void StudyZone()
        {
            Console.Clear();
            List<ConsoleKey> KeysToUse = new()
            {
                ConsoleKey.LeftArrow,
                ConsoleKey.RightArrow,
            };

            Console.ForegroundColor = ConsoleColor.Yellow;

            RetrieveStacks("display");
            Console.ForegroundColor = ConsoleColor.Cyan;
            int stackToStudy = UserInput.GetStackIdForStudy();

            RetrieveFlashcards("retrieve");
            var flashcardsToStudy = Flashcards.Where(f => f.StackId == stackToStudy).OrderBy(flashcard => flashcard.Difficulty).ToList();

            int hits = 0;
            int misses = 0;
            int flashcardId = 0;
            string difficultyToAdd = "Difficulty";
            int difficultyNumber = 0;
            string[] difficulties = { "Hard", "Good", "Easy" };

            for (int i = 0; i < flashcardsToStudy.Count; i++)
            {
                string answer = UserInput.GetFrontOfTheCard($"\nFront of the card: {flashcardsToStudy[i].Question}");
                if (answer == flashcardsToStudy[i].Answer)
                {
                    hits++;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nCorrect! \nHow was it? \n");
                }
                else
                {
                    misses++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nActual answer: {flashcardsToStudy[i].Question}. \nHow was it?\n");
                }

                if (i == flashcardsToStudy.Count)
                {
                    for (int j = 0; j < flashcardsToStudy.Count; j++)
                    {
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (SqlCommand command = new SqlCommand($"UPDATE dbo.flashcards SET difficulty = {5} WHERE flashcard_id = {flashcardsToStudy.Where(f => f.FlashcardId == flashcardsToStudy[j].FlashcardId).First().FlashcardId}", connection))
                            {
                                command.ExecuteNonQuery();
                            }
                            connection.Close();
                        }
                    }
                }
            }
            Console.ResetColor();

        var stacksWhereStackToStudy = Stacks.Where(s => s.StackId == stackToStudy).First();

        var nameOfTheStackToStudy = stacksWhereStackToStudy.Name;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"INSERT INTO dbo.study_sessions(Hits, Misses, Score, Month, stack_id, stack_name) VALUES({hits}, {misses}, {hits - misses}, {DateTime.Now.Month}, {stackToStudy}, '{nameOfTheStackToStudy}')", connection))
                {
                    command.ExecuteNonQuery();
                    UserInput.GetUserInput();
                }
            }
        }

        public static void RetrieveStacks(string displayOrRetrieve)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.stacks", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        Stacks.Clear();
                        while (reader.Read())
                        {
                            Stacks.Add(new Stack
                            {
                                StackId = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                        UserInput.GetUserInput();
                    }
                }
                var stacksCopy = Stacks.ToList();

                foreach (var stack in stacksCopy)
                {
                    foreach (var deletedStack in DeletedStacks)
                    {
                        if (stack.StackId > deletedStack.StackId)
                        {
                            stack.StackId--;
                        }
                    }
                }

                if (displayOrRetrieve == "display")
                {
                    Visualization.DisplayStacks(stacksCopy);
                }
                connection.Close();
                stacksCopy.Clear();
            }
        }

        public static void RetrieveFlashcards(string displayOrRetrieve, int IdOfTheStack = 0)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.flashcards", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int flashcardId = reader.GetInt32(0);
                        string question = reader.GetString(1);
                        string answer = reader.GetString(2);
                        int? difficulty = reader.IsDBNull(3) ? difficulty = 0 : difficulty = reader.GetInt32(0);
                        int stackId = reader.GetInt32(4);
                        Flashcards.Add(new Flashcard() { FlashcardId = flashcardId, Question = question, Answer = answer, Difficulty = difficulty, StackId = stackId });
                    }
                }
                var flashcardsCopy = Flashcards.ToList();
                List<FlashcardDTO> flashcardsDTO = new();
                foreach (var flashcard in Flashcards)
                {
                    flashcardsDTO.Add(new FlashcardDTO
                    {
                        FlashcardId = flashcard.FlashcardId,
                        Question = flashcard.Question,
                        Answer = flashcard.Answer,
                        Difficulty = flashcard.Difficulty,
                    });
                }
                if (displayOrRetrieve == "display")
                {
                    Visualization.DisplayFlashcards(flashcardsDTO);
                }
                else if (displayOrRetrieve == "display" && IdOfTheStack != 0)
                {
                    flashcardsCopy.RemoveAll(f => f.StackId != IdOfTheStack);
                    Visualization.DisplayFlashcards(flashcardsDTO);
                }
                flashcardsCopy.Clear();
                connection.Close();
            }
        }

        public static void RetrieveStudySessions(string displayOrRetrieve)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.study_sessions", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int hits = reader.GetInt32(1);
                        int misses = reader.GetInt32(2);
                        int score = reader.GetInt32(3);
                        int month = reader.GetInt32(4);
                        int stack_id = reader.GetInt32(5);
                        string stack_name = reader.GetString(6);
                        StudySessions.Add(new StudySession
                        {
                            Id = id,
                            Hits = hits,
                            Misses = misses,
                            Score = score,
                            Month = month,
                            StackId = stack_id,
                            StackName = stack_name
                        });
                    }
                }
                var studySessionsCopy = StudySessions.ToList();
                if (displayOrRetrieve == "display")
                {
                    Visualization.DisplayStudySessions(studySessionsCopy);
                }
                studySessionsCopy.Clear();
                connection.Close();

            }
        }
        public static void MonthlySessionsReport()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT stack_name, [1], [2], [3], [4], [5], [6] FROM (SELECT stack_name, Month, COUNT(*) AS row_count FROM study_sessions GROUP BY stack_name, Month) AS counts PIVOT (SUM(row_count) FOR Month IN ([1], [2], [3], [4], [5], [6])) AS PivotTable", connection))
                {
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new();
                    dataTable.Load(reader);
                    Console.WriteLine(ConsoleTableBuilder.From(dataTable).Export().ToString());
                }
                connection.Close();
            }
        }

        public static void AverageScorePerMonth()
        {
            List<PivotQuery> queryToPrint = new();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using(SqlCommand command = new SqlCommand("SELECT * FROM (SELECT stack_name, Score, Month FROM study_sessions) S PIVOT (SUM(Score) FOR [Month] in ([1], [2], [3], [4], [5], [6])) P;", connection))
                {
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new();
                    dataTable.Load(reader);

                    Console.WriteLine(ConsoleTableBuilder.From(dataTable).Export().ToString());

                }
                Visualization.DisplayPivotQuery(queryToPrint);
                connection.Close();
            }
        }
    }
}
