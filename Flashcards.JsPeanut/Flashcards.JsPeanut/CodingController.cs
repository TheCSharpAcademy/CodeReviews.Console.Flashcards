using System.Data;
using System.Data.SqlClient;
using TestingArea;
using ConsoleTableExt;
using System.Linq;

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

            int stackId = Convert.ToInt32(UserInput.GetRequiredId("Which stack do you want to delete?"));

            DeletedStacks.Add(Stacks.Where(s => s.StackId == stackId).First());

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

            int stackId = Convert.ToInt32(UserInput.GetRequiredId("Which stack do you want to update?"));

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

        public static void RetrieveDeletedFlashcards()
        {
            DeletedFlashcards.Clear();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"SELECT * FROM deletedFlashcards", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int flashcardId = reader.GetInt32(0);
                            string question = reader.GetString(1);
                            string answer = reader.GetString(2);
                            int? difficulty = reader.IsDBNull(3) ? difficulty = 0 : difficulty = reader.GetInt32(3);
                            int stackId = reader.GetInt32(4);
                            DeletedFlashcards.Add(new Flashcard() { FlashcardId = flashcardId, Question = question, Answer = answer, Difficulty = difficulty, StackId = stackId });
                        }
                    }
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
            Flashcards.Clear();
            RetrieveStacks("display");

            int idOfTheStack = Convert.ToInt32(UserInput.GetRequiredId("To which stacks does belong the flashcard you want to delete?"));

            RetrieveFlashcards("display", idOfTheStack);

            int IdOfTheFcToDelete = UserInput.GetRequiredId("Type the Id of the flashcard you want to delete.");

            var flashcardToDelete = Flashcards.Where(f => f.FlashcardId == IdOfTheFcToDelete).First();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"DELETE FROM dbo.flashcards WHERE flashcard_id = {IdOfTheFcToDelete}", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand($"INSERT INTO deletedFlashcards(flashcard_id, flashcard_question, flashcard_answer, difficulty, stack_id) VALUES({flashcardToDelete.FlashcardId}, '{flashcardToDelete.Question}', '{flashcardToDelete.Answer}', {flashcardToDelete.Difficulty}, {flashcardToDelete.StackId})", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("You have removed your flashcard successfully.");
                    UserInput.GetUserInput();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                    UserInput.GetUserInput();
                }
                connection.Close();
            }
            
            Flashcards.Clear();
        }

        public static void UpdateFlashcard()
        {
            RetrieveStacks("display");

            int stackId = Convert.ToInt32(UserInput.GetRequiredId("To which stacks does belong the flashcard you want to update?"));

            RetrieveFlashcards("display", stackId);

            int IdOfTheFcToDelete = UserInput.GetRequiredId("Type the Id of the flashcard you want to update.");

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
            Console.ForegroundColor = ConsoleColor.Yellow;
            List<ConsoleKey> KeysToUse = new()
            {
                ConsoleKey.LeftArrow,
                ConsoleKey.RightArrow,
            };

            RetrieveStacks("display");
            Console.ForegroundColor = ConsoleColor.Cyan;
            int stackToStudy = UserInput.GetRequiredId("Type the id of the stack you want to study. Type M to get back to the main menu.");
            RetrieveFlashcards("retrieve");

            var flashcardsToStudy = Flashcards.Where(f => f.StackId == stackToStudy).OrderBy(flashcard => flashcard.Difficulty).ToList();

            int hits = 0;
            int misses = 0;
            int difficultyNumber = 0;
            string[] difficulties = { "Hard", "Good", "Easy" };

            for (int i = 0; i < flashcardsToStudy.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
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
                    Console.WriteLine($"\nActual answer: {flashcardsToStudy[i].Answer}. \nHow was it?\n");
                }
                switch (Menu.ShowMenu(KeysToUse, "", "", difficulties))
                {
                    case 0:
                        difficultyNumber = 1;
                        break;
                    case 1:
                        difficultyNumber = 2;
                        break;
                    case 2:
                        difficultyNumber = 3;
                        break;
                }
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand($"UPDATE dbo.flashcards SET difficulty = {difficultyNumber} WHERE flashcard_id = {flashcardsToStudy.Where(f => f.FlashcardId == flashcardsToStudy[i].FlashcardId).First().FlashcardId}", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            Console.ResetColor();

        var stacksWhereStackToStudy = Stacks.Where(s => s.StackId == stackToStudy).First();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"INSERT INTO dbo.study_sessions(Hits, Misses, Score, Month, stack_id) VALUES({hits}, {misses}, {hits - misses}, {DateTime.Now.Month}, {stackToStudy})", connection))
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
            Flashcards.Clear();
            RetrieveDeletedFlashcards();
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
                        int? difficulty = reader.IsDBNull(3) ? difficulty = 0 : difficulty = reader.GetInt32(3);
                        int stackId = reader.GetInt32(4);
                        Flashcards.Add(new Flashcard() { FlashcardId = flashcardId, Question = question, Answer = answer, Difficulty = difficulty, StackId = stackId });
                    }
                }
                foreach (var fc in Flashcards)
                {
                    foreach (var deletedFc in DeletedFlashcards)
                    {
                        if (fc.FlashcardId > deletedFc.FlashcardId)
                        {
                            fc.FlashcardId--;
                        }
                    }
                }
                var flashcardsCopy = Flashcards.ToList();
                List<FlashcardDto> flashcardsDTO = new();
                foreach (var flashcard in Flashcards)
                {
                    flashcardsDTO.Add(new FlashcardDto
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
                using (SqlCommand command = new SqlCommand(@"SELECT 
study_sessions.Id AS Id,
study_sessions.Hits AS Hits,
study_sessions.Misses AS Misses,
study_sessions.Score AS Score,
study_sessions.Month AS month,
stacks.stack_name AS name
FROM study_sessions 
JOIN stacks ON study_sessions.stack_id=stacks.stack_id", connection))
                {
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new();
                    dataTable.Load(reader);
                    Console.WriteLine(ConsoleTableBuilder.From(dataTable).Export().ToString());
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
                using (SqlCommand command = new SqlCommand("SELECT stack_name, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] FROM (SELECT stack_name, Month, COUNT(*) AS row_count FROM study_sessions GROUP BY stack_name, Month) AS counts PIVOT (SUM(row_count) FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])) AS PivotTable", connection))
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
                using(SqlCommand command = new SqlCommand("SELECT * FROM (SELECT stack_name, Score, Month FROM study_sessions) S PIVOT (SUM(Score) FOR [Month] in ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])) P;", connection))
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
