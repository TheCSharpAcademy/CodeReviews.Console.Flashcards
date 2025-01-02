using System.Data.SqlClient;
using Dapper;
using FlashcardGame.Models;
using FlashcardGame.Views;

namespace FlashcardGame.Helpers
{
    internal class DatabaseHelpers
    {
        static string Connection = Helper.CnnVal("FlashcardDatabase");
        public static void InitializeDatabase()
        {
            string databaseName = "FlashcardDatabase";

            // Query to check if the database exists
            string checkDbSql = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{databaseName}'";

            // Query to create the database
            string createDbSql = $"CREATE DATABASE {databaseName}";

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection))
                {
                    conn.Open();

                    // Check if the database exists
                    using (SqlCommand checkCommand = new SqlCommand(checkDbSql, conn))
                    {
                        int databaseCount = (int)checkCommand.ExecuteScalar();
                        if (databaseCount > 0)
                        {
                            Console.WriteLine("Database already exists.");
                            return;
                        }
                    }

                    // Create the database if it does not exist
                    using (SqlCommand createCommand = new SqlCommand(createDbSql, conn))
                    {
                        createCommand.ExecuteNonQuery();
                        Console.WriteLine("Database created successfully.");
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }
        public static void InitializeTables()
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();


                    var com = conn.CreateCommand();

                    com.CommandText = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StacksTable' AND xtype='U')
                CREATE TABLE StacksTable (
                    stack_id INT IDENTITY(1,1) PRIMARY KEY,
                    stack_name NVARCHAR(MAX) NOT NULL
                );";
                    com.ExecuteNonQuery();


                    com.CommandText = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StudySessionsTable' AND xtype='U')
                CREATE TABLE StudySessionsTable (
                    studySession_id INT IDENTITY(1,1) PRIMARY KEY,
                    right_answers INT NOT NULL,
                    bad_answers INT NOT NULL,
                    stack_id INT NOT NULL,
                    study_date DATE NOT NULL
                );";
                    com.ExecuteNonQuery();

                    conn.Close();
                    Console.WriteLine("Tables initialized successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        public static void DeleteStack()
        {
            Console.Clear();
            Console.WriteLine("Write a name of stack that you want to Delete or press 0 to exit:");

            List<Stack> stacks = DataAccess.GetStacks();



            if (stacks.Count == 0)
            {
                Console.WriteLine("No stacks were found!");
                return;
            }

            for (int i = 0; i < stacks.Count; i++)
            {
                Stack stack = stacks[i];
                Console.WriteLine($"{i + 1}. {stack.stack_name}");
            }

            string? stackName = Console.ReadLine();

            if (stackName == "0")
            {
                return;
            }


            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                string deleteQuery =
                    $@"DELETE FROM StacksTable WHERE CAST(stack_name AS VARCHAR(MAX)) = ('{stackName}')";

                int count = connection.Execute(deleteQuery);

                if (count > 0)
                {
                    Console.WriteLine("Row was deleted!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("No row was deleted, because name doesn't match that you entered!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                }
            }
        }
        public static void AddStack()
        {
            Console.Clear();
            Console.WriteLine("Write a name of stack that you want to add or press 0 to exit:");

            string stackName = Console.ReadLine();

            if (stackName == "0")
            {
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(Connection))
                {
                    connection.Open();

                    // Insert the stack into the StacksTable and return the new Stack ID
                    string insertQuery = $"INSERT INTO StacksTable (stack_name) OUTPUT INSERTED.stack_id VALUES ('{stackName}')";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        int stackId = (int)cmd.ExecuteScalar();
                        Console.WriteLine($"Stack '{stackName}' added with ID {stackId}.");

                        // Initialize the table for this stack
                        InitializeStackTable(stackId);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        public static void AddFlashcard(int stackId)
        {
            Console.Clear();
            Console.WriteLine("Write a question of flashcard you want to put in or press 0 to exit:");

            var question = Console.ReadLine();

            if (question == "0")
            {
                return;
            }
            while (string.IsNullOrEmpty(question))
            {
                Console.WriteLine("Question can't be empty");
                question = Console.ReadLine();

            }

            Console.WriteLine("Write an answer of the question or press 0 to exit");

            var answer = Console.ReadLine();

            if (answer == "0")
            {
                return;
            }

            while (string.IsNullOrEmpty(question))
            {
                Console.WriteLine("Answer can't be empty");
                answer = Console.ReadLine();

            }

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                string insertQuery =
                    $@"INSERT INTO Stack{stackId}Table (flashCard_Question, flashcard_Answer, stack_Id ) VALUES ('{question}', '{answer}', '{stackId}');";

                connection.Execute(insertQuery);
            }
        }

        public static void DeleteFlashcard(int stackId)
        {
            ViewFlashcard(stackId);
            Console.WriteLine("Write a id of flashcard that you want to delete or press 0 to exit:");
            string? flashcardId = Console.ReadLine();
            int id;

            if (flashcardId == "0")
            {
                return;
            }

            while (!int.TryParse(flashcardId, out id))
            {
                Console.WriteLine("Please enter integer. Try again ");
                flashcardId = Console.ReadLine();
            }

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                string deleteQuery =
                    $@"DELETE FROM Stack{stackId}Table WHERE flashcard_id = ('{id}')";

                int count = connection.Execute(deleteQuery);

                if (count > 0)
                {
                    Console.WriteLine("Row was deleted!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("No row was deleted, because id doesn't match that you entered!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                }
                BalanceIds($"Stack{stackId}Table", stackId);
            }
        }

        public static void UpdateFlashcard(int stackId)
        {

            ViewFlashcard(stackId);

            Console.WriteLine("Write a id of flashcard that you want to edit or press 0 to exit:");
            string? flashcardId = Console.ReadLine();
            int id;

            if (flashcardId == "0")
            {
                return;
            }

            while (!int.TryParse(flashcardId, out id))
            {
                Console.WriteLine("Please enter integer. Try again ");
                flashcardId = Console.ReadLine();
            }
            Console.WriteLine("Write a new question or press 0 to exit");
            var newQuestion = Console.ReadLine();

            if (newQuestion == "0")
            {
                return;
            }
            while (string.IsNullOrEmpty(newQuestion))
            {
                Console.WriteLine("Question can't be empty");
                newQuestion = Console.ReadLine();

            }

            Console.WriteLine("Write an answer of the question or press 0 to exit");

            var newAnswer = Console.ReadLine();

            if (newQuestion == "0")
            {
                return;
            }
            while (string.IsNullOrEmpty(newAnswer))
            {
                Console.WriteLine("Answer can't be empty");
                newAnswer = Console.ReadLine();

            }

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                string insertQuery = $@"UPDATE Stack{stackId}Table
                SET flashCard_Question = '{newQuestion}' , flashcard_Answer = '{newAnswer}'
                WHERE flashCard_Id = {id}";

                connection.Execute(insertQuery);
            }
        }

        public static void ViewFlashcard(int stackId)
        {
            Console.Clear();
            Console.WriteLine("========== Flashcards ==========\n");

            var flashcards = DataAccess.GetFlashcards(stackId);
            var filteredFlashcards = FilterFlashcardsByStackId(stackId, flashcards);

            if (filteredFlashcards.Count == 0)
            {
                Console.WriteLine("No flashcards found in this stack.");
                Console.WriteLine("Press any key to return to menu");
                Console.ReadLine();
                StackMenu.RunStackMenu();
            }


            Console.WriteLine("  Id    |    Question                            |    Answer");
            Console.WriteLine("--------|----------------------------------------|-------------------");

            for (int i = 0; i < filteredFlashcards.Count; i++)
            {
                FlashcardDTO flashcard = filteredFlashcards.ElementAt(i).ConvertToDTO();

                Console.WriteLine(
                    $" {flashcard.flashcard_Id,-6} | {flashcard.flashCard_Question,-38} | {flashcard.flashcard_Answer,-15}");

                Console.WriteLine("--------|----------------------------------------|-------------------");
            }

            Console.WriteLine("\n============================================");

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

        public static List<Flashcard> FilterFlashcardsByStackId(int stackId, List<Flashcard> flashcards)
        {
            return flashcards.Where(flashcard => flashcard.stack_Id == stackId).ToList();
        }

        public static void ViewStudySessions()
        {
            var studySessions = DataAccess.GetStudySessions();

            if (studySessions == null || studySessions.Count == 0)
            {
                Console.WriteLine("No study sessions found.");
                return;
            }

            Console.WriteLine("========== Study Sessions ==========\n");

            Console.WriteLine(" Id   | Right Ans  | Wrong Ans  | Study Date            ");
            Console.WriteLine("------|------------|------------|-----------------------");

            foreach (var studySession in studySessions)
            {

                Console.WriteLine(studySession.ToString());
            }

            Console.WriteLine("\n====================================");

            Console.WriteLine("Press any key to continue:");
            Console.ReadLine();

        }

        public static void InitializeStackTable(int stackId)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();


                    var com = conn.CreateCommand();
                    com.CommandText = $@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Stack{stackId}Table' AND xtype='U')
                CREATE TABLE Stack{stackId}Table (
                    flashcard_id INT IDENTITY(1,1) PRIMARY KEY,
                    flashcard_question NVARCHAR(MAX) NOT NULL,
                    flashcard_answer NVARCHAR(MAX) NOT NULL,
                    stack_id INT NOT NULL
                );";
                    com.ExecuteNonQuery();
                    conn.Close();
                    Console.WriteLine("Tables initialized successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        public static void BalanceIds(string flashcardStackName, int stackId)
        {
            var flashcards = DataAccess.GetFlashcards(stackId);

            ClearFlashcardTable(stackId);

            foreach (var flashcard in flashcards) 
            {
                using (SqlConnection connection = new SqlConnection(Connection))
                {
                    connection.Open();
                    string insertQuery =
                        $@"INSERT INTO Stack{stackId}Table (flashCard_Question, flashcard_Answer, stack_Id ) VALUES ('{flashcard.flashCard_Question}', '{flashcard.flashcard_Answer}', '{stackId}');";

                    connection.Execute(insertQuery);

                }
            }
            
        }

        public static void ClearFlashcardTable(int stackId)
        {
            string deleteSql = $"DELETE FROM Stack{stackId}Table;";
            string resetIdentitySql = $"DBCC CHECKIDENT ('Stack{stackId}Table', RESEED, 0);";

            try
            {
                using (SqlConnection conn = new SqlConnection(Connection))
                {
                    conn.Open();

                    // Delete all rows
                    using (SqlCommand deleteCommand = new SqlCommand(deleteSql, conn))
                    {
                        int rowsDeleted = deleteCommand.ExecuteNonQuery();
                        Console.WriteLine($"Deleted {rowsDeleted} rows from Stack{stackId}Table.");
                    }

                    // Reset identity seed
                    using (SqlCommand resetCommand = new SqlCommand(resetIdentitySql, conn))
                    {
                        resetCommand.ExecuteNonQuery();
                        Console.WriteLine("Identity seed reset successfully.");
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    } 
}
