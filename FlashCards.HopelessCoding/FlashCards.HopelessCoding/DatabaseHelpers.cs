using System.Configuration;
using System.Data.SqlClient;

namespace DbHelpers.HopelessCoding;

public class DatabaseHelpers
{
    public static string initializationConnectionString = ConfigurationManager.ConnectionStrings["InitializationConnectionString"].ConnectionString;
    public static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;

    public static void InitializeDatabase()
    {
        try
        {
            CreateDatabase();
            CreateTables();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during database initialization: {ex.Message}\nPress any key to continue.");
            Console.ReadLine();
        }
    }

    private static void CreateDatabase()
    {
        string createDatabaseQuery = "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FlashCardsData') " +
                                         "BEGIN " +
                                         "    CREATE DATABASE FlashCardsData; " +
                                         "END";

        using (SqlConnection connection = new SqlConnection(initializationConnectionString))
        using (SqlCommand createDatabaseCommand = new SqlCommand(createDatabaseQuery, connection))
        {
            connection.Open();
            createDatabaseCommand.ExecuteNonQuery();
        }
    }

    private static void CreateTables()
    {
        string[] tableCreationQueries =
        {
            "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stacks') " +
            "BEGIN " +
            "    CREATE TABLE Stacks ( " +
            "        StackName NVARCHAR(50) PRIMARY KEY " +
            "    ); " +
            "END",

            "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FlashCards') " +
            "BEGIN " +
            "    CREATE TABLE FlashCards ( " +
            "        Id INT IDENTITY(1,1) PRIMARY KEY, " +
            "        Stack NVARCHAR(50) FOREIGN KEY REFERENCES Stacks(StackName) ON DELETE CASCADE ON UPDATE CASCADE, " +
            "        Front NVARCHAR(50) NOT NULL, " +
            "        Back NVARCHAR(50) NOT NULL " +
            "    ); " +
            "END",

            "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudySessions') " +
            "BEGIN " +
            "    CREATE TABLE StudySessions ( " +
            "        Id INT IDENTITY(1,1) PRIMARY KEY, " +
            "        Stack NVARCHAR(50) FOREIGN KEY REFERENCES Stacks(StackName) ON DELETE CASCADE ON UPDATE CASCADE, " +
            "        Date DATETIME NOT NULL, " +
            "        Score INT NOT NULL " +
            "    ); " +
            "END"
        };

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            connection.ChangeDatabase("FlashCardsData");

            foreach (var query in tableCreationQueries)
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public static bool StackExists(string stackName)
    {
        string checkQuery = "SELECT COUNT(1) " +
                            "FROM Stacks " +
                            "WHERE StackName = @StackName";

        using (var connection = new SqlConnection(DatabaseHelpers.connectionString))
        {
            using (SqlCommand command = new SqlCommand(checkQuery, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@StackName", stackName);

                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
        }
    }
}