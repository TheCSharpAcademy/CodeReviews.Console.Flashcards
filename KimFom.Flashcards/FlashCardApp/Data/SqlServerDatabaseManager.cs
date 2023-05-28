using System.Configuration;
using System.Data.SqlClient;
using FlashCardApp.DTO;
using FlashCardApp.Models;

namespace FlashCardApp.Data;

public class SqlServerDatabaseManager : IDatabaseManager
{
    private string _connectionString = ConfigurationManager.AppSettings.Get("connectionString")!;

    public void CreateStackTable()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "IF OBJECT_ID(N'Stack', N'U') IS NULL " +
                                      "CREATE TABLE Stack (Id INT PRIMARY KEY IDENTITY(1,1), " +
                                      "Name NVARCHAR(50) UNIQUE)";

                command.ExecuteNonQuery();
            }
        }
    }

    public void CreateFlashCardTable()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "IF OBJECT_ID(N'FlashCard', N'U') IS NULL " +
                                      "CREATE TABLE FlashCard (Id INT PRIMARY KEY IDENTITY(1,1), " +
                                      "StackId INT FOREIGN KEY REFERENCES Stack(Id) ON DELETE CASCADE, " +
                                      "Name NVARCHAR(50), " +
                                      "Content NVARCHAR(500))";

                command.ExecuteNonQuery();
            }
        }
    }

    public void CreateStudyAreaTable()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "IF OBJECT_ID(N'StudyArea', N'U') IS NULL " +
                                      "CREATE TABLE StudyArea (Id INT PRIMARY KEY IDENTITY(1,1), " +
                                      "StackId INT FOREIGN KEY REFERENCES Stack(Id) ON DELETE CASCADE, " +
                                      "Date DATE DEFAULT GETDATE(), " +
                                      "Score INT)";

                command.ExecuteNonQuery();
            }
        }
    }


    // Stack Operations
    public void AddNewStack(Stack newStack)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "INSERT INTO Stack (Name) " +
                                      $"VALUES ('{newStack.Name}')";

                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateStack(Stack oldStack, Stack newStack)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "UPDATE Stack " +
                                      $"SET Name = '{newStack.Name}' " +
                                      $"WHERE Name = '{oldStack.Name}'";

                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteStack(Stack stackToDelete)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "DELETE FROM Stack " +
                                      $"WHERE Name = '{stackToDelete.Name}'";

                command.ExecuteNonQuery();
            }
        }
    }

    public List<StackDTO> GetStacks()
    {
        List<StackDTO> stackList = new();
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "SELECT * FROM Stack";

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    stackList.Add(new StackDTO
                    {
                        StackName = reader.GetString(1) // The second ordinal is StackName column
                    });
                }
            }
        }

        return stackList;
    }

    private int GetStackId(Stack stack)
    {
        int id = -1;
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "SELECT Id FROM Stack " +
                                      $"WHERE Name = '{stack.Name}'";

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = (int)reader["Id"];
                    }
                }
            }
        }

        return id;
    }

    // FlashCard Operations
    public void AddNewFlashCard(FlashCard flashCard, Stack stack)
    {
        var stackId = GetStackId(stack);
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "INSERT INTO FlashCard (StackId, Name, Content) " +
                                      $"VALUES ({stackId}, '{flashCard.Name}','{flashCard.Content}') ";

                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateFlashCard(FlashCard oldFlashCard, FlashCard newFlashCard, Stack stack)
    {
        var stackId = GetStackId(stack);
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "UPDATE FlashCard " +
                                      $"SET Name = '{newFlashCard.Name}', " +
                                      $"Content = '{newFlashCard.Content}' " +
                                      $"WHERE Name = '{oldFlashCard.Name}' " +
                                      $"AND StackId = {stackId}";

                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateFlashCardName(FlashCard oldFlashCard, FlashCard newFlashCard, Stack stack)
    {
        var stackId = GetStackId(stack);
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "UPDATE FlashCard " +
                                      $"SET Name = '{newFlashCard.Name}' " +
                                      $"WHERE Name = '{oldFlashCard.Name}' " +
                                      $"AND StackId = {stackId}";

                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateFlashCardContent(FlashCard oldFlashCard, FlashCard newFlashCard, Stack stack)
    {
        var stackId = GetStackId(stack);
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "UPDATE FlashCard " +
                                      $"SET Content = '{newFlashCard.Content}' " +
                                      $"WHERE Name = '{oldFlashCard.Name}' " +
                                      $"AND StackId = {stackId}";

                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteFlashCard(FlashCard flashCardToDelete, Stack stack)
    {
        var stackId = GetStackId(stack);
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "DELETE FROM FlashCard " +
                                      $"WHERE Name = '{flashCardToDelete.Name}' " +
                                      $"AND StackId = {stackId}";

                command.ExecuteNonQuery();
            }
        }
    }

    public List<FlashCardDTO> GetFlashCardsOfStack(Stack stack)
    {
        var stackId = GetStackId(stack);
        var id = 0;
        List<FlashCardDTO> flashCardList = new();

        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "SELECT * FROM FlashCard " +
                                      $"WHERE StackId = {stackId}";

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    flashCardList.Add(new FlashCardDTO
                    {
                        Id = ++id,
                        Name = reader.GetString(2), // The second ordinal is Name column
                        Content = reader.GetString(3), // The third ordinal is Content column
                    });
                }
            }
        }

        return flashCardList;
    }
    
    // StudyArea Operations
    public void SaveScore(StudyArea studyArea, Stack stack)
    {
        var stackId = GetStackId(stack);
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "INSERT INTO StudyArea(StackId, Score) " +
                                      $"VALUES ({stackId}, {studyArea.Score})";

                command.ExecuteNonQuery();
            }
        }
    }

    public List<StudyAreaDto> GetScoresHistory()
    {
        List<StudyAreaDto> history = new();
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "SELECT SA.Date, SA.Score, St.Name " +
                                      "FROM StudyArea AS SA " +
                                      "LEFT JOIN Stack AS St " +
                                      "ON SA.StackId = St.Id";
                
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    history.Add(new StudyAreaDto
                    {
                        Date = (DateTime)reader["Date"],
                        Score = (int)reader["Score"],
                        Stack = (string)reader["Name"]
                    });
                }
            }
        }

        return history;
    }
}