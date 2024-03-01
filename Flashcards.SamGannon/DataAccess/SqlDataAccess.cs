using DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Text;

namespace DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        private readonly string _connectionString;

        public SqlDataAccess(string connectionString)
        {
            _connectionString = connectionString;

            Task.Run(() =>
            {
                try
                {
                    var builder = new SqlConnectionStringBuilder(_connectionString)
                    {
                        ConnectTimeout = 5
                    };
                    using var connection = new SqlConnection(builder.ConnectionString);
                    connection.Open();
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    Console.Clear();
                    Console.WriteLine($"Database connection error: {ex.Message}\n\nSuggestion: verify your connection string.\n\nAborting!");
                    Environment.Exit(1);
                }
            });
        }

        public void CreateStackTable()
        {
            using (var connection = new  SqlConnection(_connectionString))
            {
                using(var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "IF OBJECT_ID(N'Stack', N'U') IS NULL " +
                                          "CREATE TABLE Stack (Id INT PRIMARY KEY IDENTITY(1,1), " +
                                          "Name NVARCHAR(50) UNIQUE)";

                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        public void CreateFlashcardTable()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "IF OBJECT_ID(N'FlashCard', N'U') IS NULL " +
                                          "CREATE TABLE FlashCard (Id INT PRIMARY KEY IDENTITY(1,1), " +
                                          "StackId INT FOREIGN KEY REFERENCES Stack(Id) ON DELETE CASCADE, " +
                                          "Question NVARCHAR(50), " +
                                          "Answer NVARCHAR(500))";

                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        public void CreateStudyTable()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "IF OBJECT_ID(N'StudyArea', N'U') IS NULL " +
                                          "CREATE TABLE StudyArea (Id INT PRIMARY KEY IDENTITY(1,1), " +
                                          "StackId INT FOREIGN KEY REFERENCES Stack(Id) ON DELETE CASCADE, " +
                                          "StartTime DATETIME DEFAULT GETDATE(), " +
                                          "EndTime DATETIME DEFAULT GETDATE(), " +
                                          "Score DECIMAL(18,0))";

                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        public bool CheckStackName(string stackName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "SELECT 1 FROM dbo.Stack WHERE Name = @StackName";
                    tableCmd.Parameters.AddWithValue("@StackName", stackName);

                    // Execute the query and check if any rows were returned
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }

        public void AddStack(string? stackName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = $"INSERT INTO dbo.Stack (Name) Values (@StackName)";
                    tableCmd.Parameters.AddWithValue("@StackName", stackName);

                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        public List<StackModel> GetAllStacks()
        {
            List<StackModel> listOfStackNames = new List<StackModel>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "SELECT * FROM dbo.Stack ORDER BY Id";
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                listOfStackNames.Add(
                                    new StackModel
                                    {
                                        StackId = reader.GetInt32(0),
                                        StackName = reader.GetString(1)
                                    }); ; ;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n\nNo rows found");
                        }
                    }

                    return listOfStackNames;
                }
            }
        }

        public List<StackModel> GetListOfStackNames()
        {
            List<StackModel> listOfStackNames = new List<StackModel>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = $"SELECT DISTINCT Name FROM dbo.Stack";
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                listOfStackNames.Add(
                                    new StackModel
                                    {
                                        // StackId = reader.GetInt32(0),
                                        StackName = reader.GetString(0)
                                    });
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n\nNo rows found");
                        }
                    }

                    return listOfStackNames;
                }
            }
        }

        public StackModel GetStackByName(string stackName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = $"SELECT * FROM Stack WHERE Name = @stackName";
                    tableCmd.Parameters.AddWithValue("@stackName", stackName);
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        StackModel selectedStack = new();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            selectedStack.StackId = reader.GetInt32(0);
                            selectedStack.StackName = reader.GetString(1);
                        }

                        Console.WriteLine("\n\n");

                        return selectedStack;
                    }
                }
            }
        }

        public void DeleteStack(string stackName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                {
                    using var tableCmd = connection.CreateCommand();
                    {
                        // delete flashcards associated with stack while the stack still exist
                        tableCmd.CommandText = "DELETE FROM dbo.FlashCard WHERE StackId IN (SELECT Id FROM dbo.Stack WHERE Name = @stackName)";
                        tableCmd.Parameters.AddWithValue("@stackName", stackName);

                        tableCmd.ExecuteNonQuery();

                        // now delete the stack
                        tableCmd.CommandText = "DELETE FROM dbo.Stack WHERE Name = @Name";
                        tableCmd.Parameters.AddWithValue("@Name", stackName);

                        tableCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void EditStackname(string oldStackName, string? newStackName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "UPDATE dbo.Stack SET Name = @newStackName WHERE Name = @oldStackName";
                    tableCmd.Parameters.AddWithValue("@newStackName", newStackName);
                    tableCmd.Parameters.AddWithValue("@oldStackName", oldStackName);

                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        public void AddFlashcard(string? question, string? answer, int StackId)
        {
            using (var connection = new SqlConnection (_connectionString)) 
            {
                connection.Open();

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "INSERT INTO dbo.Flashcard (StackId, Question, Answer) VALUES (@StackId, @Question, @Answer)";
                    tableCmd.Parameters.AddWithValue("@StackId", StackId);
                    tableCmd.Parameters.AddWithValue("@Question", question);
                    tableCmd.Parameters.AddWithValue("@Answer", answer);

                    tableCmd.ExecuteNonQuery();
                }
               
            }
        }

        public List<Flashcard> GetFlashcardsByStackId(int stackId)
        {
            List<Flashcard> flashcards = new List<Flashcard>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "SELECT * FROM dbo.Flashcard WHERE StackId = @StackId";
                    tableCmd.Parameters.AddWithValue("@StackId", stackId);

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Flashcard flashcard = new Flashcard
                                {
                                    FlashcardId = reader.GetInt32(0),
                                    StackId = reader.GetInt32(1),
                                    Question = reader.GetString(2),
                                    Answer = reader.GetString(3)
                                };

                                flashcards.Add(flashcard);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                    }
                }
            }

            return flashcards;
        }

        public void DeleteFlashcard(int FlashcardId, string stackName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var tableCmd = connection.CreateCommand())
                {
                    // The following SQL DELETE statement is designed to delete a flashcard with a specific Id
                    // from the dbo.Flashcard table only if the flashcard is associated with a stack identified by
                    // the given @StackName parameter. The subquery within the IN clause ensures that the flashcard
                    // is not only matched by its unique Id but also belongs to the specified stack, preventing
                    // unintentional deletions of flashcards that may share the same Id but exist in different stacks

                    tableCmd.CommandText = "DELETE FROM dbo.Flashcard WHERE Id = @FlashcardId AND StackId IN (SELECT Id FROM dbo.Stack WHERE Name = @StackName)";
                    tableCmd.Parameters.AddWithValue("@FlashcardId", FlashcardId);
                    tableCmd.Parameters.AddWithValue("@StackName", stackName);

                    tableCmd.ExecuteNonQuery();

                    int rowsAffected = tableCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Successfully deleted {rowsAffected} row(s).");
                    }
                    else
                    {
                        Console.WriteLine("No rows were affected.");
                    }
                }
            }
        }

        public List<Flashcard> GetAllFlashcards(string stackName)
        {
            List<Flashcard> flashcards = new List<Flashcard>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "SELECT * FROM dbo.Flashcard WHERE StackId IN (SELECT Id FROM dbo.Stack WHERE Name = @stackName)";
                    tableCmd.Parameters.AddWithValue("@stackname", stackName);

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Flashcard flashcard = new Flashcard
                                {
                                    FlashcardId = reader.GetInt32(0),
                                    StackId = reader.GetInt32(1),
                                    Question = reader.GetString(2),
                                    Answer = reader.GetString(3)
                                };

                                flashcards.Add(flashcard);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                    }
                }
            }

            return flashcards;
        }

        public Flashcard GetFlashcardsById(int flashcardId)
        {
            Flashcard flashcard = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "SELECT * FROM dbo.Flashcard WHERE Id = @flashcardID";
                    tableCmd.Parameters.AddWithValue("@flashcardID", flashcardId);

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                flashcard = new Flashcard
                                {
                                    FlashcardId = reader.GetInt32(0),
                                    StackId = reader.GetInt32(1),
                                    Question = reader.GetString(2),
                                    Answer = reader.GetString(3)
                                };
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                    }
                }
            }

            return flashcard;
        }

        public int GetStackId(string stackName)
        {
            int stackId = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "SELECT Id FROM dbo.Stack WHERE Name = @stackName";
                    tableCmd.Parameters.AddWithValue("@stackName", stackName);

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            {
                                stackId = reader.GetInt32(0);
                                return stackId;
                            }
                            
                        }

                        return stackId;
                    }
                }
            }
        }

        public void InsertStudySession(int stackId, DateTime startTime, DateTime endTime, double score)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var  tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = $"INSERT INTO dbo.StudyArea (StackId, StartDate, EndDate, Score) Values (@StackId, @StartTime, @EndTime, @Score)";
                    tableCmd.Parameters.AddWithValue("@StackId", stackId);
                    tableCmd.Parameters.AddWithValue("@StartTime", startTime);
                    tableCmd.Parameters.AddWithValue("@EndTime", endTime);
                    tableCmd.Parameters.AddWithValue("@Score", score);

                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        public List<StudyHistory> GetHistroyReport()
        {
            List<StudyHistory> reports = new List<StudyHistory>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using(var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "SELECT SA.Id, StackId, Name, StartDate, EndDate, Score " +
                        "FROM dbo.StudyArea AS SA " +
                        "INNER JOIN dbo.Stack AS ST ON SA.StackId = ST.Id";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                reports.Add(
                                    new StudyHistory
                                    {
                                        Id = reader.GetInt32(0),
                                        StackId = reader.GetInt32(1),
                                        StackName = reader.GetString(2),
                                        StartTime = reader.GetDateTime(3),
                                        EndTime = reader.GetDateTime(4),
                                        Score = reader.GetDecimal(5),
                                    });
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n\nNo rows found");
                        }
                    }

                    return reports;

                }
            }
        }

    }
}
