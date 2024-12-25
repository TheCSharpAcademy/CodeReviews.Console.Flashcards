using System.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;

namespace flashcard_app
{
    /// <summary>
    /// Responsible for handling all interactions with the database (DB)
    /// </summary>
    class DBController
    {
        static string? serverName = @ConfigurationManager.AppSettings.Get("ServerName");
        static string? connectionString = $"Server={serverName};Database=FlashcardApp;Trusted_Connection=True;Encrypt=False;";

        internal static void InitialiseDB()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Database connection successful.");

                // Create the 'Stack' table
                string createStackTableQuery = @"
                CREATE TABLE Stacks (
                    StackID INT IDENTITY(1,1) PRIMARY KEY,
                    StackName NVARCHAR(100) NOT NULL
                );";

                // Create the 'Flashcards' table
                string createFlashcardsTableQuery = @"
                CREATE TABLE Flashcards (
                    FlashcardID INT IDENTITY(1,1) PRIMARY KEY,
                    FrontText NVARCHAR(MAX) NOT NULL,
                    BackText NVARCHAR(MAX) NOT NULL,
                    StackID INT NOT NULL,
                    FOREIGN KEY (StackID) REFERENCES Stacks(StackID)
                );";

                // Create the 'Study Session' table
                string createStudySessionTableQuery = @"
                CREATE TABLE StudySessions (
                    StudySessionID INT IDENTITY(1,1) PRIMARY KEY,
                    SessionDate DATETIME NOT NULL,
                    Score INT NOT NULL,
                    ScoreMax INT NOT NULL,
                    StackID INT NOT NULL,
                    FOREIGN KEY (StackID) REFERENCES Stacks(StackID)
                );";

                // Execute the commands
                ExecuteSqlCommand(createStackTableQuery, connection, "Stacks table created.");
                ExecuteSqlCommand(createFlashcardsTableQuery, connection, "Flashcards table created.");
                ExecuteSqlCommand(createStudySessionTableQuery, connection, "Study sessions table created.");
            }
        }

        static void ExecuteSqlCommand(string sqlQuery, SqlConnection connection, string successMessage)
        {
            using (var command = new SqlCommand(sqlQuery, connection))
            {
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine(successMessage);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }

        internal static List<Stack> GetAllStacks()
        {
            var stacks = new List<Stack>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT StackId, StackName FROM Stacks;";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return stacks; // Return an empty list
                        }

                        while (reader.Read())
                        {
                            var stack = new Stack
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };
                            stacks.Add(stack);
                        }
                    }
                }
            }

            return stacks;
        }

        internal static string GetStackNameFromID(int stackID)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT StackName FROM Stacks WHERE StackID = @StackId;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StackID", stackID);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Advance to the first record
                        {
                            return reader.GetString(0); // Get the value from the first column
                        }
                        else
                        {
                            return "N/A"; // No rows found
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the ID of the newly-generated stack, or -1 if it was unsuccessful
        /// </summary>
        internal static int AddNewStack(string? stackName)
        {
            int newStackId = -1;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Stacks (StackName) VALUES (@StackName) SELECT CAST(SCOPE_IDENTITY() AS INT);";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StackName", stackName);

                    // Get ID
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        newStackId = Convert.ToInt32(result);
                        Console.WriteLine($"New stack '{stackName}' added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add the stack. Please try again.");
                    }
                }
            }

            return newStackId;
        }

        internal static List<FlashcardDTO> GetFlashcardsByStackId(int? stackId)
        {
            List<FlashcardDTO> flashcards = new List<FlashcardDTO>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT FlashcardID, FrontText, BackText FROM Flashcards WHERE StackID = @StackId;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StackID", stackId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return flashcards; // Return an empty list
                        }

                        while (reader.Read())
                        {
                            var flashcard = new FlashcardDTO
                            {
                                FlashcardID = reader.GetInt32(0),
                                FrontText = reader.GetString(1),
                                BackText = reader.GetString(2)
                            };
                            flashcards.Add(flashcard);
                        }
                    }
                }
            }

            return flashcards;
        }

        internal static List<FlashcardDTO> GetAllFlashcards()
        {
            List<FlashcardDTO> flashcards = new List<FlashcardDTO>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT FlashcardID, FrontText, BackText FROM Flashcards;";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No flashcards found in the stack.");
                            return flashcards; // Return an empty list
                        }

                        while (reader.Read())
                        {
                            var flashcard = new FlashcardDTO
                            {
                                FlashcardID = reader.GetInt32(0),
                                FrontText = reader.GetString(1),
                                BackText = reader.GetString(2)
                            };
                            flashcards.Add(flashcard);
                        }
                    }
                }
            }

            return flashcards;
        }

        internal static void CreateNewFlashcard(string? frontText, string? backText, int? stackID)
        {
            Console.Clear();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery =
                    "INSERT INTO Flashcards (StackID, FrontText, BackText) VALUES (@StackID, @FrontText, @BackText);";

                using (var command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@StackID", stackID);
                    command.Parameters.AddWithValue("@FrontText", frontText);
                    command.Parameters.AddWithValue("@BackText", backText);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Flashcard created successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create the flashcard.");
                    }
                }
            }
        }

        internal static void UpdateFlashcard(int? flashcardID, string? frontText, string? backText)
        {
            Console.Clear();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE Flashcards SET FrontText = @FrontText, BackText = @BackText WHERE FlashcardID = @FlashcardID;";

                using (var command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@FlashcardID", flashcardID);
                    command.Parameters.AddWithValue("@FrontText", frontText);
                    command.Parameters.AddWithValue("@BackText", backText);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Flashcard updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"No flashcard found with that ID. Update failed.");
                    }
                }
            }
        }

        internal static void DeleteFlashcard(int? flashcardID)
        {
            Console.Clear();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Flashcards WHERE FlashcardID = @FlashcardID;";

                using (var command = new SqlCommand(deleteQuery, connection))
                {
                    // Add parameter to prevent SQL injection
                    command.Parameters.AddWithValue("@FlashcardID", flashcardID);

                    // Execute the delete query
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Flashcard deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"No flashcard found with that ID. Deletion failed.");
                    }
                }
            }
        }

        internal static void CreateStudySession(int stackID, int score, int scoreMax)
        {
            DateTime todaysDate = DateTime.Now;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery =
                    "INSERT INTO StudySessions (StackID, Score, ScoreMax, SessionDate) VALUES (@StackID, @Score, @ScoreMax, @SessionDate);";

                using (var command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@StackID", stackID);
                    command.Parameters.AddWithValue("@Score", score);
                    command.Parameters.AddWithValue("@ScoreMax", scoreMax);
                    command.Parameters.AddWithValue("@SessionDate", todaysDate);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"study session created successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create the study session.");
                    }
                }
            }
        }

        internal static List<StudySession> GetAllStudySessions()
        {
            List<StudySession> studySessions = new List<StudySession>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT StackId, Score, ScoreMax, SessionDate FROM StudySessions;";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return studySessions; // Return an empty list
                        }

                        while (reader.Read())
                        {
                            var session = new StudySession
                            {
                                StackID = reader.GetInt32(0),
                                Score = reader.GetInt32(1),
                                ScoreMax = reader.GetInt32(2),
                                SessionDate = reader.GetDateTime(3)
                            };
                            studySessions.Add(session);
                        }
                    }
                }
            }

            return studySessions;
        }

        internal static bool StackAlreadyExists(string stackName)
        {
            List<Stack> allStacks = GetAllStacks();

            foreach (Stack stack in allStacks)
            {
                if (stack.Name == stackName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}