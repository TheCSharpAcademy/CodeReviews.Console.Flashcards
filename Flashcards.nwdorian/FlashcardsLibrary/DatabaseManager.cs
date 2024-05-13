using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace FlashcardsLibrary;
public class DatabaseManager
{
    private string? connectionStringDb;
    private string? connectionString;

    public DatabaseManager()
    {
        connectionStringDb = AppConfig.GetDbConnectionString();
        connectionString = AppConfig.GetFullConnectionString();
    }

    public void InitDatabase()
    {
        CreateDatabase();
        CreateTables();
        SeedData();
    }

    private void CreateDatabase()
    {
        try
        {
            using var connection = new SqlConnection(connectionStringDb);

            var createDatabaseSql = """
                                        IF NOT EXISTS(SELECT *
                                                    FROM   sys.databases
                                                    WHERE  NAME = 'FlashcardsDb')
                                        CREATE DATABASE FlashcardsDb
                                        """;
            connection.Execute(createDatabaseSql);
        }
        catch (Exception e)
        {
            AnsiConsole.Write($"Error! Details: {e.Message}\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private void CreateTables()
    {
        try
        {
            using var connection = new SqlConnection(connectionString);

            var createTablesSql = """
                                    IF NOT EXISTS (SELECT NAME
                                                   FROM   sys.tables
                                                   WHERE  NAME = 'Stack')
                                      CREATE TABLE Stack
                                        (
                                           Id   INT PRIMARY KEY IDENTITY(1, 1),
                                           Name NVARCHAR(100) NOT NULL
                                        );

                                    IF NOT EXISTS (SELECT NAME
                                                   FROM   sys.tables
                                                   WHERE  NAME = 'Flashcard')
                                      CREATE TABLE Flashcard
                                        (
                                           Id       INT PRIMARY KEY IDENTITY(1, 1),
                                           Stackid  INT FOREIGN KEY REFERENCES stack(id) ON DELETE CASCADE,
                                           Question NVARCHAR(100) NOT NULL,
                                           Answer   NVARCHAR(100) NOT NULL
                                        );

                                    IF NOT EXISTS (SELECT NAME
                                                   FROM   sys.tables
                                                   WHERE  NAME = 'StudySession')
                                      CREATE TABLE Studysession
                                        (
                                           Id      INT PRIMARY KEY IDENTITY(1, 1),
                                           Stackid INT FOREIGN KEY REFERENCES stack(id) ON DELETE CASCADE,
                                           Date    DATETIME NOT NULL,
                                           Score   INT NOT NULL
                                        )
                                    """;
            connection.Execute(createTablesSql);
        }
        catch (Exception e)
        {
            AnsiConsole.Write($"Error! Details: {e.Message}\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private void SeedData()
    {
        try
        {
            using var connection = new SqlConnection(connectionString);

            var seedDataSql = """
                              IF NOT EXISTS(SELECT 1 FROM Stack)
                              BEGIN
                              INSERT INTO Stack (name) VALUES ('German')
                              INSERT INTO Stack (name) VALUES ('French')
                              INSERT INTO Stack (name) VALUES ('Spanish')
                              END

                              IF NOT EXISTS(SELECT 1 FROM Flashcard)
                              BEGIN
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (1, 'Haus', 'House')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (1, 'Hund', 'Dog')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (1, 'Katze', 'Cat')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (1, 'Buch', 'Book')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (1, 'Wasser', 'Water')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (1, 'Essen', 'Food')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (1, 'Freund', 'Friend')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (1, 'Sonne', 'Sun')
                              
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (2, 'Maison', 'House')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (2, 'Chien', 'Dog')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (2, 'Chat', 'Cat')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (2, 'Livre', 'Book')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (2, 'Eau', 'Water')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (2, 'Nourriture', 'Food')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (2, 'Ami', 'Friend')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (2, 'Soleil', 'Sun')

                              INSERT INTO Flashcard (stackid, question, answer) VALUES (3, 'Casa', 'House')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (3, 'Perro', 'Dog')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (3, 'Gato', 'Cat')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (3, 'Libro', 'Book')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (3, 'Agua', 'Water')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (3, 'Comida', 'Food')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (3, 'Amigo', 'Friend')
                              INSERT INTO Flashcard (stackid, question, answer) VALUES (3, 'Sol', 'Sun')
                              END

                              IF NOT EXISTS(SELECT 1 FROM StudySession)
                              BEGIN
                              INSERT INTO Studysession (stackid, date, score) VALUES (1, '2024-05-10 22:21:35', 8)
                              INSERT INTO Studysession (stackid, date, score) VALUES (1, '2024-05-09 08:23:15', 7)
                              INSERT INTO Studysession (stackid, date, score) VALUES (1, '2024-05-08 14:45:31', 5)
                              INSERT INTO Studysession (stackid, date, score) VALUES (2, '2024-05-07 09:12:05', 8)
                              INSERT INTO Studysession (stackid, date, score) VALUES (2, '2024-05-06 16:38:51', 6)
                              INSERT INTO Studysession (stackid, date, score) VALUES (2, '2024-05-05 15:47:56', 6)
                              INSERT INTO Studysession (stackid, date, score) VALUES (3, '2024-05-04 12:56:13', 8)
                              INSERT INTO Studysession (stackid, date, score) VALUES (3, '2024-05-03 13:02:50', 6)
                              INSERT INTO Studysession (stackid, date, score) VALUES (3, '2024-05-02 20:41:34', 5)
                              INSERT INTO Studysession (stackid, date, score) VALUES (3, '2024-05-01 20:23:15', 4)
                              END
                              """;
            connection.Execute(seedDataSql);
        }
        catch (Exception e)
        {
            AnsiConsole.Write($"Error! Details: {e.Message}\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
