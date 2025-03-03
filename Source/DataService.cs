using System.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;

namespace vcesario.Flashcards;

public static class DataService
{
    public static void DebugDeleteDatabase()
    {
        string? masterConnectionString = ConfigurationManager.AppSettings.Get("masterConnectionString");
        string? databaseName = ConfigurationManager.AppSettings.Get("databaseName");
        using (SqlConnection connection = new SqlConnection(masterConnectionString))
        {
            connection.Open();

            try
            {
            string checkDbQuery = $"DROP DATABASE {databaseName}";
            connection.Execute(checkDbQuery);
            Console.WriteLine("Database dropped.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            Console.ReadLine();
        }
    }

    public static void Initialize()
    {
        bool isNewDb = false;
        string? databaseName = ConfigurationManager.AppSettings.Get("databaseName");
        string? masterConnectionString = ConfigurationManager.AppSettings.Get("masterConnectionString");

        Console.WriteLine("Checking database...");
        using (SqlConnection connection = new SqlConnection(masterConnectionString))
        {
            connection.Open();

            string checkDbQuery = "SELECT database_id FROM sys.databases WHERE name = @DatabaseName";
            var result = connection.QueryFirstOrDefault(checkDbQuery, new { DatabaseName = databaseName });

            if (result == null)
            {
                string createDbQuery = $"CREATE DATABASE {databaseName}";
                connection.Execute(createDbQuery);
                Console.WriteLine($"Database '{databaseName}' created.");
                isNewDb = true;
            }
            else
            {
                Console.WriteLine($"Database '{databaseName}' found.");
            }
        }

        if (!isNewDb)
        {
            return;
        }

        Console.WriteLine("Creating table...");
        using (var connection = OpenConnection())
        {
            try
            {
                string createTablesQuery = @"
                    CREATE TABLE Stacks(
                        Id INT PRIMARY KEY IDENTITY,
                        Name VARCHAR(50) UNIQUE
                    );
                    
                    CREATE TABLE Cards(
                        Id INT PRIMARY KEY IDENTITY,
                        StackId INT FOREIGN KEY REFERENCES Stacks(Id)
                            ON DELETE CASCADE,
                        Front VARCHAR(50),
                        Back VARCHAR(50)
                    );
                    
                    CREATE TABLE StudySessions(
                        Id INT PRIMARY KEY IDENTITY,
                        StackId INT FOREIGN KEY REFERENCES Stacks(Id)
                            ON DELETE CASCADE,
                        Date DATETIME2,
                        Score INT
                    );";

                connection.Execute(createTablesQuery);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadLine();
            }
        }
    }

    public static SqlConnection OpenConnection()
    {
        string? dbConnectionString = ConfigurationManager.AppSettings.Get("dbConnectionString");
        var connection = new SqlConnection(dbConnectionString);
        connection.Open();

        return connection;
    }

    // public static void InsertSession(CodingSession session)
    // {
    //     using (var connection = OpenConnection())
    //     {
    //         string sql = @"INSERT INTO coding_sessions (start_date, end_date)
    //                         VALUES (@StartDateTime, @EndDateTime)";
    //         var anonymousSession = new
    //         {
    //             StartDateTime = session.Start.ToString("yyyy-MM-dd HH:mm:ss"),
    //             EndDateTime = session.End.ToString("yyyy-MM-dd HH:mm:ss")
    //         };

    //         try
    //         {
    //             connection.Execute(sql, anonymousSession);
    //         }
    //         catch (SQLiteException)
    //         {
    //             PrintDbError();
    //         }
    //     }
    // }

    // public static bool PromptSessionOverlap(CodingSession session)
    // {
    //     List<CodingSession> sessions = null;

    //     using (var connection = OpenConnection())
    //     {
    //         string sql = @"SELECT rowid, start_date, end_date FROM coding_sessions
    //                         WHERE start_date <= @EndDateTime AND end_date >= @StartDateTime AND rowid != @Id";
    //         var anonymousSession = new
    //         {
    //             StartDateTime = session.Start.ToString("yyyy-MM-dd HH:mm:ss"),
    //             EndDateTime = session.End.ToString("yyyy-MM-dd HH:mm:ss"),
    //             Id = session.Id
    //         };

    //         try
    //         {
    //             sessions = connection.Query<CodingSession>(sql, anonymousSession).ToList();
    //         }
    //         catch (SQLiteException)
    //         {
    //             PrintDbError();
    //             return false;
    //         }
    //     }

    //     if (sessions.Count == 0)
    //     {
    //         return true;
    //     }

    //     Console.WriteLine();
    //     Console.WriteLine(ApplicationTexts.DATASERVICE_OVERLAP_INFO);

    //     DateUtils.DrawSessionTable(sessions);

    //     Console.WriteLine();
    //     var userChoice = AnsiConsole.Prompt(
    //         new ConfirmationPrompt(ApplicationTexts.DATASERVICE_OVERLAP_PROMPT)
    //         {
    //             DefaultValue = false
    //         }
    //     );

    //     if (!userChoice)
    //     {
    //         return false;
    //     }

    //     userChoice = AnsiConsole.Prompt(
    //         new ConfirmationPrompt(ApplicationTexts.CONFIRM_AGAIN)
    //         {
    //             DefaultValue = false
    //         }
    //     );

    //     if (!userChoice)
    //     {
    //         return false;
    //     }

    //     using (var connection = OpenConnection())
    //     {
    //         foreach (var existingSession in sessions)
    //         {
    //             string sql = "DELETE FROM coding_sessions WHERE rowid=@Id";

    //             try
    //             {
    //                 connection.Execute(sql, new { Id = existingSession.Id });
    //             }
    //             catch (SQLiteException)
    //             {
    //                 PrintDbError();
    //                 return false;
    //             }
    //         }
    //     }

    //     return true;
    // }

    // public static void PrintDbError()
    // {
    //     AnsiConsole.MarkupLine("[red]" + ApplicationTexts.GENERIC_DB_ERROR + "[/]");
    //     Console.ReadLine();
    // }
}