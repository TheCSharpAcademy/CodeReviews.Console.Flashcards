using ConsoleTableExt;
using System.Data;
using System.Data.SqlClient;

namespace FlashCardApp;

internal class UserInput
{
    static int stackID;
    static bool deleteStack;
    static bool editStack;

    static bool addFlashCard;
    static bool editFlashCard;
    static bool deleteFlashCard;
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
            Console.WriteLine($"\n\nChanges to have all the adds/edits/deletes");
            Console.WriteLine("Type 4 to add a Flashcard");
            Console.WriteLine("Type 5 to edit a Flashcard");
            Console.WriteLine("Type 6 to delete a Flashcard");
            Console.WriteLine("Type 7 to add a Stack");
            Console.WriteLine("Type 8 to edit a Stack");
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
                case "4":
                    addFlashCard = true;
                    ChooseStack();
                    break;
                case "5":
                    editFlashCard = true;
                    ChooseStack();
                    break;
                case "6":
                    deleteFlashCard = true;
                    ChooseStack();
                    break;
                case "7":
                    AddStack();
                    break;
                case "8":
                    editStack = true;
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

                    if (!deleteStack && !addFlashCard && !editFlashCard && !deleteFlashCard && !editStack)
                    {
                        Console.WriteLine("Choose Continent to Study");
                    }

                    if(deleteStack)
                    {
                        Console.WriteLine("Choose Continent to Delete");
                    }

                    if(addFlashCard)
                    {
                        Console.WriteLine("Choose Stack to add Flashcards");
                    }

                    if(editFlashCard)
                    {
                        Console.WriteLine("Edit Flashcard");
                    }

                    if(deleteFlashCard)
                    {
                        Console.WriteLine("Delete Flashcard");
                    }
                    
                    if(editStack)
                    {
                        Console.WriteLine("Edit Stack");
                    }

                    int stackNumber;
                    while (true)
                    {
                        Console.WriteLine("Enter the stack number or 'b' to go back:");
                        string input = Console.ReadLine();

                        if (input.ToLower() == "b")
                        {
                            Console.WriteLine("Returning to previous menu...");
                            return;
                        }

                        if (!int.TryParse(input, out stackNumber) || stackNumber < 1 || stackNumber > stackNumberToIdMap.Count)
                        {
                            Console.WriteLine("Invalid stack number. Please enter a valid number or 'b' to go back.");
                            continue;
                        }

                        if (!stackNumberToIdMap.TryGetValue(stackNumber, out stackID))
                        {
                            Console.WriteLine($"Invalid stack number: {stackNumber}");
                            return;
                        }

                        break;
                    }
                }
                connection.Close();
            }

            using (SqlCommand stackNameCommand = new SqlCommand($"SELECT StackName FROM Stacks WHERE StackId = {stackID}", connection))
            {
                connection.Open();
                stackName = stackNameCommand.ExecuteScalar().ToString();
            }

            if (!deleteStack && !addFlashCard && !editStack|| editFlashCard || deleteFlashCard)
            {
                GetFlashCards(flashcardQuery, connection, stackID, stackName);
            }

            if(deleteStack)
            {
                DeleteStack(stackID);
            }

            if(addFlashCard)
            {
                AddFlashcard(stackID);
            }

            if(editFlashCard)
            {
                EditStack(stackID);
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
                    int flashcardId = reader.GetInt32(reader.GetOrdinal("FlashcardId"));
                    string question = reader.GetString(reader.GetOrdinal("Question"));
                    string answer = reader.GetString(reader.GetOrdinal("Answer"));

                    FlashcardDto flashcard = new FlashcardDto
                    {
                        FlashcardId = flashcardId,
                        Question = question,
                        Answer = answer
                    };
                    flashcards.Add(flashcard);
                }
            }
        }



        if(editFlashCard)
        {
            EditFlashcard(flashcards);
        }
       
        if(!editFlashCard && !deleteFlashCard)
        {
            FlashCardQuestions(flashcards, stackName);
        }

        if(deleteFlashCard)
        {
            DeleteFlashCard(flashcards);
        }
        
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
        deleteStack = false;

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

        Console.WriteLine("Hit enter to continue");
        Console.ReadLine();
        GetUserInput();
    }

    static void AddStack()
    {
        Console.WriteLine("Enter the name of the new stack:");
        string stackName = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(GetData.connectionString))
        {
            connection.Open();
            string sql = "INSERT INTO Stacks (StackName) VALUES (@StackName);";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@StackName", stackName);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"Added {rowsAffected} stack hit enter to restart.");
            }
        }

        Console.WriteLine($"new stack named {stackName} created");
        Console.ReadLine();
        GetUserInput();
    }

    static void AddFlashcard(int stackId)
    {
        addFlashCard = false;
        Console.WriteLine("Enter the question:");
        string question = Console.ReadLine();

        Console.WriteLine("Enter the answer:");
        string answer = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(GetData.connectionString))
        {
            connection.Open();
            string sql = "INSERT INTO Flashcards (StackId, Question, Answer) VALUES (@StackId, @Question, @Answer);";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@StackId", stackId);
                command.Parameters.AddWithValue("@Question", question);
                command.Parameters.AddWithValue("@Answer", answer);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"Added {rowsAffected} flashcard.");
            }
        }

        Console.WriteLine($"{question} flashcard added, Hit enter to return.");
        GetUserInput();
    }

    static void EditFlashcard(List<FlashcardDto> flashcards)
    {
        editFlashCard = false;
        if (flashcards.Count == 0)
        {
            Console.WriteLine("No flashcards found.");
            return;
        }

        var tableData = new List<object[]>();
        for (int i = 0; i < flashcards.Count; i++)
        {
            FlashcardDto flashcard = flashcards[i];
            tableData.Add(new object[] { i + 1, flashcard.Question, flashcard.Answer });
        }

        ConsoleTableBuilder
            .From(tableData)
            .WithColumn("Number", "Question", "Answer")
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine(TableAligntment.Center);

        Console.WriteLine("Enter the number of the flashcard to edit:");
        int flashcardNumber;
        while (!int.TryParse(Console.ReadLine(), out flashcardNumber) || flashcardNumber < 1 || flashcardNumber > flashcards.Count)
        {
            Console.WriteLine("Invalid flashcard number. Please enter a valid number:");
        }

        FlashcardDto selectedFlashcard = flashcards[flashcardNumber - 1];

        Console.WriteLine("Enter the new question:");
        string newQuestion = Console.ReadLine();

        Console.WriteLine("Enter the new answer:");
        string newAnswer = Console.ReadLine();

        selectedFlashcard.Question = newQuestion;
        selectedFlashcard.Answer = newAnswer;
        

        Console.WriteLine($"{selectedFlashcard.FlashcardId} is current flashcardID");

        using (SqlConnection connection = new SqlConnection(GetData.connectionString))
        {
            connection.Open();

            string sql = "UPDATE Flashcards SET Question = @Question, Answer = @Answer WHERE FlashcardId = @FlashcardId;";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Question", newQuestion);
                command.Parameters.AddWithValue("@Answer", newAnswer);
                command.Parameters.AddWithValue("@FlashcardId", selectedFlashcard.FlashcardId);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"Flashcard updated. Rows affected: {rowsAffected}");
            }
        }

        Console.WriteLine("Flashcard updated successfully");
        Console.ReadLine();
        GetUserInput();
    }

    static void DeleteFlashCard(List<FlashcardDto> flashcards)
    {
        Console.WriteLine("Choose flashcard to delete");
        deleteFlashCard = false;
        if (flashcards.Count == 0)
        {
            Console.WriteLine("No flashcards found.");
            return;
        }

        var tableData = new List<object[]>();
        for (int i = 0; i < flashcards.Count; i++)
        {
            FlashcardDto flashcard = flashcards[i];
            tableData.Add(new object[] { i + 1, flashcard.Question, flashcard.Answer });
        }

        ConsoleTableBuilder
            .From(tableData)
            .WithColumn("Number", "Question", "Answer")
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine(TableAligntment.Center);

        Console.WriteLine("Enter the number of the flashcard to delete:");
        int flashcardNumber;
        while (!int.TryParse(Console.ReadLine(), out flashcardNumber) || flashcardNumber < 1 || flashcardNumber > flashcards.Count)
        {
            Console.WriteLine("Invalid flashcard number. Please enter a valid number:");
        }

        FlashcardDto selectedFlashcard = flashcards[flashcardNumber - 1];

        using (SqlConnection connection = new SqlConnection(GetData.connectionString))
        {
            connection.Open();

            string sql = "DELETE FROM Flashcards WHERE FlashcardId = @FlashcardId;";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@FlashcardId", selectedFlashcard.FlashcardId);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"Flashcard deleted. Rows affected: {rowsAffected}");
            }
        }

        Console.WriteLine("Flashcard deleted successfully, Hit enter to return.");
        Console.ReadLine();
        GetUserInput();
    }

    static void EditStack(int stackId)
    {
        editStack = false;
        Console.WriteLine("Enter the new stack name:");
        string newStackName = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(GetData.connectionString))
        {
            connection.Open();

            string sql = "UPDATE Stacks SET StackName = @StackName WHERE StackId = @StackId;";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@StackName", newStackName);
                command.Parameters.AddWithValue("@StackId", stackId);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"Stack updated. Rows affected: {rowsAffected}");
            }
        }

        Console.WriteLine("Stack updated successfully.");
        Console.WriteLine("Hit enter to continue");
        Console.ReadLine();
        GetUserInput();
    }

}

