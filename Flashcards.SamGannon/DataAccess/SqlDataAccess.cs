using DataAccess.DTOs;
using Microsoft.Data.SqlClient;
using System.Text;

namespace DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        private readonly string _connectionString;

        // credit to Github user dnym for the connection string builder
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
                                          "Name NVARCHAR(50), " +
                                          "Content NVARCHAR(500))";

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
                                          "Date DATE DEFAULT GETDATE(), " +
                                          "Score INT)";

                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        public List<DtoStack> GetListOfStackNames()
        {
            List<DtoStack> listOfStackNames = new List<DtoStack>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT DISTINCT Name FROM dbo.Stack";
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                listOfStackNames.Add(
                                    new DtoStack
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

        public void AddStack(string? stackName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"INSERT INTO dbo.Stack (Name) Values ('{stackName}')";
                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        public List<DtoStack> GetAllStacks()
        {
            List<DtoStack> listOfStackNames = new List<DtoStack>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM dbo.Stack ORDER BY Id";
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                listOfStackNames.Add(
                                    new DtoStack
                                    {
                                        StackId = reader.GetInt32(0),
                                        StackName = reader.GetString(1)
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

        public void AddFlashcard(string? question, string? answer)
        {
            // i probably need to pass an object here.
        }

    }
}
