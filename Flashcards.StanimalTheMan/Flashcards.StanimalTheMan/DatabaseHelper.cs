using System.Data.SqlClient;

namespace Flashcards.StanimalTheMan;

internal class DatabaseHelper
{
    private static readonly string ConnectionString = "Data Source=(LocalDb)\\LocalDBDemo;Initial Catalog=Flashcards;Integrated Security=True";

    public static SqlConnection GetOpenConnection()
    {
        SqlConnection connection = new SqlConnection(ConnectionString);
        connection.Open();
        return connection;
    }

    public static void CloseConnection(SqlConnection connection)
    {
        if (connection != null && connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
    }

    public static void InitializeDatabase()
    {
        SqlConnection connection = null;

        string createStacksQuery = @"
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stacks')
BEGIN
CREATE TABLE Stacks (
    StackId INT PRIMARY KEY IDENTITY(1,1),
    StackName NVARCHAR(255) UNIQUE NOT NULL
);        
END
        ";

        string createFlashcardsQuery = @"
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Flashcards')
BEGIN
    CREATE TABLE Flashcards (
        FlashcardId INT PRIMARY KEY IDENTITY(1,1),
        StackId INT FOREIGN KEY REFERENCES Stacks(StackId),
        Front NVARCHAR(255) NOT NULL,
        Back NVARCHAR(255) NOT NULL
    );
END

";

        string createStudyQuery = @"
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Study')
BEGIN
    CREATE TABLE Study (
        StudySessionId INT PRIMARY KEY IDENTITY(1,1),
        StackId INT FOREIGN KEY REFERENCES Stacks(StackId),
        Date DATETIME NOT NULL,
        Score INT NOT NULL
    );
END

";
        try
        {
            connection = GetOpenConnection();
            using (SqlCommand command = new SqlCommand(createStacksQuery, connection))
            {
                try
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == -1)
                    {
                        Console.WriteLine("Stacks Table created successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Stacks Table already exists.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            using (SqlCommand command = new SqlCommand(createFlashcardsQuery, connection))
            {
                try
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == -1)
                    {
                        Console.WriteLine("Flashcards Table created successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Flashcards Table already exists.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            using (SqlCommand command = new SqlCommand(createStudyQuery, connection))
            {
                try
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == -1)
                    {
                        Console.WriteLine("Study Table created successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Study Table already exists.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            CloseConnection(connection);
        }
    }
}
