
using System.Data.SqlClient;
using FlashCards.Ibrahim.Database_Access;
using FlashCards.Ibrahim.Database_Acess;
using FlashCards.Ibrahim.UI;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string serverConnection = configuration.GetConnectionString("DefaultConnection");
        using (SqlConnection sqlConnection = new SqlConnection(serverConnection))
        {
            sqlConnection.Open();
            SqlCommand command = sqlConnection.CreateCommand();
            command.CommandText = @"
                                    IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'MyFlashcardDb')
                                    CREATE DATABASE MyFlashcardDb;";
            command.ExecuteNonQuery();
        }
        string connectionString = configuration.GetConnectionString("FlashcardDb");
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = connection.CreateCommand();

            command.CommandText = @"
                                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stacks_Table')
                                    BEGIN
                                        CREATE TABLE Stacks_Table (
                                            Id INT IDENTITY(1,1) PRIMARY KEY,
                                            Name NVARCHAR(100) NOT NULL
                                        )
                                    END";
            command.ExecuteNonQuery();
            command.CommandText = @"
                                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Flashcards_Table')
                                    BEGIN
                                        CREATE TABLE Flashcards_Table (
                                            Id INT IDENTITY(1,1) PRIMARY KEY,
                                            Stacks_Id INT NOT NULL,
                                            FRONT NVARCHAR(MAX) NOT NULL,
                                            BACK NVARCHAR(MAX) NOT NULL,
                                            FOREIGN KEY (Stacks_Id) REFERENCES Stacks_Table(Id) ON DELETE CASCADE
                                        )
                                    END";
            command.ExecuteNonQuery();
            command.CommandText = @"
                                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudySession_Table')
                                    BEGIN
                                        CREATE TABLE StudySession_Table (
                                            Id INT IDENTITY(1,1) PRIMARY KEY,
                                            Stacks_Id INT NOT NULL,
                                            Score INT,
                                            Date DATETIME,
                                            FOREIGN KEY (Stacks_Id) REFERENCES Stacks_Table(Id) ON DELETE CASCADE
                                        )
                                    END";
            command.ExecuteNonQuery();
        }

        Flashcard_DB_Access flashcard_DB_Access = new Flashcard_DB_Access(connectionString);
        Stacks_DB_Access stacks_DB_Access = new Stacks_DB_Access(connectionString);
        StudySession_DB_Access studySession_DB_Access = new StudySession_DB_Access(connectionString);

        UserMenu.ShowMenu();
    }
}