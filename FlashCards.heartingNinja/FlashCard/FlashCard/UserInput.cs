using ConsoleTableExt;
using System.Data;
using System.Data.SqlClient;

namespace FlashCardApp;

internal class UserInput
{
    internal static int stackID;
    static bool deleteStack;
    internal static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;

        while (closeApp == false)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to Choose Continent and Study.");
            Console.WriteLine("Type 2 to Show Past Study Sessions.");
            Console.WriteLine("Type 3 to Delete Stack and flashcards and Study Sessions with same ID");
            Console.WriteLine("------------------------------------------\n");
            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    ChooseStack();
                    break;
                case "2":
                    GetData.SetStudySessionDto();
                    GetAllRecords();
                    break;
                case "3":
                    deleteStack = true;
                    ChooseStack();
                    break;

                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 2.\n");
                    break;
            }
        }
    }

    static void ChooseStack()
    {
        string connectionString = GetData.connectionString;
        string stackQuery = GetData.stackQuery;
        string flashcardQuery = GetData.flashcardQuery;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string stackName = null;

            using (SqlCommand stackCommand = new SqlCommand(stackQuery, connection))
            {
                connection.Open();

                using (SqlDataReader reader = stackCommand.ExecuteReader())
                {
                    Console.WriteLine("Available Stacks:");

                    int counter = 1;
                    var stackNumberToIdMap = new Dictionary<int, int>();

                    while (reader.Read())
                    {
                        stackID = reader.GetInt32(reader.GetOrdinal("StackId"));
                        stackName = reader.GetString(reader.GetOrdinal("StackName"));
                        Console.WriteLine($"{counter}. {stackName}");
                        stackNumberToIdMap.Add(counter, stackID);
                        counter++;
                    }

                    if (!deleteStack)
                    {
                        Console.WriteLine("Choose Continent to Study");
                    }
                    else
                    {
                        Console.WriteLine("Choose Continent to Delete");
                    }

                    int stackNumber = int.Parse(Console.ReadLine());

                    if (!stackNumberToIdMap.TryGetValue(stackNumber, out stackID))
                    {
                        Console.WriteLine($"Invalid stack number: {stackNumber}");
                        return;
                    }
                }
                connection.Close();
            }

            using (SqlCommand stackNameCommand = new SqlCommand($"SELECT StackName FROM Stacks WHERE StackId = {stackID}", connection))
            {
                connection.Open();
                stackName = stackNameCommand.ExecuteScalar().ToString();
            }

            if (!deleteStack)
            {
                GetFlashCards(flashcardQuery, connection, stackID, stackName);
            }
            else
            {
                DeleteStack(stackID);
            }
        }
        Console.ReadLine();
    }

    static void GetFlashCards(string flashcardQuery, SqlConnection connection, int stackId, string stackName)
    {
        List<FlashcardDto> flashcards = new List<FlashcardDto>();

        using (SqlCommand flashcardCommand = new SqlCommand(flashcardQuery, connection))
        {
            flashcardCommand.Parameters.AddWithValue("@StackId", stackId);

            using (SqlDataReader reader = flashcardCommand.ExecuteReader())
            {
                Console.WriteLine($"Flashcards for Stack {stackName}:");

                while (reader.Read())
                {
                    string question = reader.GetString(reader.GetOrdinal("Question"));
                    string answer = reader.GetString(reader.GetOrdinal("Answer"));

                    FlashcardDto flashcard = new FlashcardDto
                    {
                        Question = question,
                        Answer = answer
                    };
                    flashcards.Add(flashcard);
                }
            }
        }
        FlashCardQuestions(flashcards, stackName);
    }

    static void FlashCardQuestions(List<FlashcardDto> flashcards, string stackName)
    {
        List<FlashcardDto> selectedFlashcards = new List<FlashcardDto>(flashcards);
        int n = selectedFlashcards.Count;

        while (n > 1)
        {
            n--;
            int k = new Random().Next(n + 1);
            FlashcardDto temp = selectedFlashcards[k];
            selectedFlashcards[k] = selectedFlashcards[n];
            selectedFlashcards[n] = temp;
        }

        int numCorrect = 0;
        int flashcardCount = 0;

        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                FlashcardDto flashcard = selectedFlashcards[flashcardCount % selectedFlashcards.Count];
                Console.WriteLine(flashcard.Question);
                string userAnswer = Console.ReadLine().Trim();

                if (userAnswer.ToLower() == flashcard.Answer.ToLower())
                {
                    Console.WriteLine("Correct!");
                    numCorrect++;
                }
                else
                {
                    Console.WriteLine($"Incorrect! The correct answer is {flashcard.Answer}.");
                }

                flashcardCount++;
            }

            if (flashcardCount == selectedFlashcards.Count)
            {
                flashcardCount = 0;
            }

            WriteNewRecord(numCorrect, stackName);
            Console.WriteLine($"You got {numCorrect} out of {flashcardCount} flashcards correct. Press any key to exit.");
            return;
        }
    }

    static void GetAllRecords()
    {
        Console.Clear();
        var tableData = GetData.sessionDtos
        .Select((x, index) => new
        {
            SessionId = index + 1,
            x.Name,
            x.Date,
            x.Score
        })
        .ToList();

        if (tableData.Any())
        {
            ConsoleTableBuilder
                .From(tableData)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine(TableAligntment.Center);
        }
        else
        {
            Console.WriteLine("No rows found");
        }
    }

    static void WriteNewRecord(int score, string stackName)
    {
        GetData.SetStudySessionDto();
        DateTime endTime = DateTime.Now;
        int sessionId = GetData.sessionDtos.Count > 0 ? GetData.sessionDtos.Max(s => s.SessionId) + 1 : 1;

        StudySessionDto newSession = new StudySessionDto
        {
            SessionId = sessionId,
            Name = stackName,
            StackId = stackID,
            Date = endTime,
            Score = score
        };

        GetData.sessionDtos.Add(newSession);

        using (var connection = new SqlConnection(GetData.connectionString))
        {
            connection.Open();
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = "INSERT INTO StudySessions (SessionId, Name, StackId, Date, Score) VALUES (@SessionId, @Name, @StackId, @Date, @Score)";
            insertCmd.Parameters.AddWithValue("@SessionId", newSession.SessionId);
            insertCmd.Parameters.AddWithValue("@Name", newSession.Name);
            insertCmd.Parameters.AddWithValue("@StackId", newSession.StackId);
            insertCmd.Parameters.AddWithValue("@Date", newSession.Date);
            insertCmd.Parameters.AddWithValue("@Score", newSession.Score);
            insertCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    static void DeleteStack(int stackId)
    {

        using (SqlConnection connection = new SqlConnection(GetData.connectionString))
        {
            connection.Open();
            string sql = "DELETE FROM StudySessions WHERE StackId = @StackId; " +
                         "DELETE FROM Flashcards WHERE StackId = @StackId; " +
                         "DELETE FROM Stacks WHERE StackId = @StackId;";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@StackId", stackId);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"Deleted {rowsAffected} rows.");
            }
        }
        deleteStack = false;
    }
}

